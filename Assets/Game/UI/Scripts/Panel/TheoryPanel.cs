using DG.Tweening;
using SUG.Essentials;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class TheoryPanel : UIBase
{
    // —— UI variable ——
    [SerializeField] private TheoryBackButton _backBtn;
    [SerializeField] private List<TheoryElementButton> _theoryBtns = new List<TheoryElementButton>();
    [SerializeField] private UIButton _startTheory; // 开始考核

    [Header("考核UI成员")]
    [Header("答题区域")]
    [SerializeField] private UIPanel _assPanel; // 考核面板
    [SerializeField] private TextMeshProUGUI _titleContext; // 题目
    [SerializeField] private UIButton _prevBtn, _nextBtn, _submitBtn; // 上一题按钮，下一题按钮，提交按钮
    [SerializeField] private ToggleGroup _optionGroup; // 考核选择按钮父物体
    [SerializeField] private AssOption _optionPrefab; // 考核选择按钮预制体
    [SerializeField] private RectTransform _assRect; // 【考试面板】的RectTransform, 用来刷新layout，不让其更新内容后UI控件错位。

    [Header("答题引导区域")]
    [SerializeField] private TextMeshProUGUI _currCnt; // 当前完成的题目数量
    [SerializeField] private TextMeshProUGUI _totalCnt; // 全部题目数量
    [SerializeField] private Slider _percentSlider; // 答题进度条
    [SerializeField] private Transform _titlementContent; // 答题题号按钮父类
    [SerializeField] private QuestionNavigatorItem _titlementPrefab; // 答题题号预制体
    private QuestionNavigatorItem _currNavigatorItem; // 当前的题号item
    [SerializeField] private RectTransform _naviRect; // 【考试引导面板】的RectTransform, 用来刷新layout，不让其更新内容后UI控件错位。

    [Header("考试结果面板区域")]
    [SerializeField] private UIPanel _passPanel; // 通过面板
    [SerializeField] private UIPanel _failPanel; // 不通过面板
    [SerializeField] private UIButton _passOkBtn; // 考核结束考核按钮

    // Data container

    // 题目引导管理容器
    private readonly List<AssOption> _assOptions = new List<AssOption>();
    private readonly List<QuestionNavigatorItem> _navigationItems = new List<QuestionNavigatorItem>();

    // Inject
    [Inject] private IGameService  _gameMgr;
    [Inject] private ISceneService _sceneMgr;
    [Inject] private IAssService _assMgr;

    // Assessment data.
    private QuestionData _currData; // 当前题目内容数据
    private bool _isAssing = false; // 是否在考试

    #region 生命周期函数

    // ======================
    // Life cycle
    // ======================
    private void Start()
    {
        DataInitialization();
        EventInitialization();
        StartCoroutine(Initializaction());
    }

    #endregion

    #region 初始化
    // ======================
    // Initialized
    // ======================

    /// <summary>
    /// 数据初始化
    /// </summary>
    private void DataInitialization()
    {
        _currData = _assMgr.GetCurrQuestion();

        // 当前题目初始化考核UI面板
        LoadData(_currData);

        // 题目列表初始化考核引导面板
        for (int i = 0; i < _assMgr.questionList.Count; ++i)
        {
            var item = Essentials.Instantiate(_titlementPrefab, _titlementContent);
            item.Setup(i);
            item.gameObject.SetActive(true);
            _navigationItems.Add(item);
        }

        // 设置当前选择的题号为0
        if (_navigationItems.Count > 0)
        {
            _currNavigatorItem = _navigationItems[0];
            _currNavigatorItem.SetNaviStateAndRefresh(NavigationState.Answering);
        }
    }

    /// <summary>
    /// 成员变量/控件初始化
    /// </summary>
    /// <returns></returns>
    IEnumerator Initializaction()
    {
        yield return null;

        // 初始化理论考核按钮
        foreach (TheoryElementButton btn in _theoryBtns) { btn.TogglePanel(); }
        _theoryBtns[0]?.OnPointClick();

        // 初始化考核面板
        SetAssPanelActive(false);

        // 考试结果面板隐藏
        _passPanel.RaiseTrigger(InteractionTrigger.DeSelect);
        _passPanel.gameObject.SetActive(false);

        _failPanel.RaiseTrigger(InteractionTrigger.DeSelect);
        _failPanel.gameObject.SetActive(false);

        // 提交按钮初始化时需要隐藏
        _nextBtn.gameObject.SetActive(true);
        _submitBtn.gameObject.SetActive(false);
    }

    /// <summary>
    /// 事件初始化
    /// </summary>
    private void EventInitialization()
    {
        foreach (TheoryElementButton btn in _theoryBtns)
        {
            btn.onSelect += OnClickTheoryButton;
        }

        foreach (var item in _navigationItems)
            item.onSwitchTopics += SwitchTopics;

        _backBtn.onSelected += OnTheoryBackClick;
        _nextBtn.onClickEnter += NextQuestion;
        _submitBtn.onClickEnter += SubmitTheAnswer;
        _prevBtn.onClickEnter += PrevQuestion;
        _startTheory.onClickEnter += StartAssessment;
        _passOkBtn.onClickEnter += BackStartScene;
    }

    /// <summary>
    /// 载入当前题目信息至UI面板。
    /// </summary>
    /// <param name="data"></param>
    private void LoadData(QuestionData data)
    {
        // 清理内存
        foreach (var option in _assOptions)
        {
            option.gameObject.SetActive(false);
            //Destroy(option);
        }
        _assOptions.Clear();

        // 根据新的data更新UI
        var pkg = _assMgr.recordArr[_assMgr.currIdx];
        _titleContext.text = data.title;
        foreach (var op in data.options)
        {
            // 当前题目选项创建
            AssOption option = Essentials.Instantiate(_optionPrefab, _optionGroup.transform);
            option.Setup(op.isAnswer, op.content, data.isSingle, _optionGroup);
            option.onTrigger += OnOptionSelected;
            option.SetActive(true);
            option.Verify(pkg?.selectContents);

            // 将创建好的option添加到管理列表中
            _assOptions.Add(option);
        }

        // 更新题目进度窗口的【已答数/总共数】控件的信息内容。
        _totalCnt.text = _assMgr.GetTotalQuestion().ToString();
        _currCnt.text = _assMgr.GetFinishQestionCount().ToString();
        RefreshProgress();
        
        // 更新Canvas，使面板的Layout可以正常的排布UI控件
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_assRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_naviRect);
    }

    #endregion

    #region Reset

    // 考核内容重置
    private void AssReset()
    {
        // 重置AssManager
        _assMgr.ResetData();

        // 重新加载题目
        LoadData(_assMgr.GetCurrQuestion());

        // 重置引导面板中的控件
        foreach (var ui in _navigationItems)
            ui.SetNaviStateAndRefresh(NavigationState.NotAnswered);

        // 设置当前选择的题号为0
        if (_navigationItems.Count > 0)
        {
            _currNavigatorItem = _navigationItems[0];
            _currNavigatorItem.SetNaviStateAndRefresh(NavigationState.Answering);
        }
    }

    #endregion

    #region 事件
    // ======================
    // Event
    // ======================

    // 返回开始选择界面
    private void BackStartScene()
    {
        var _gameCfg = _gameMgr.gameSetting;

        _sceneMgr.LoadSceneAsync(_gameCfg.startScene);
    }

    // 放弃考试时
    private void LetgoAss()
    {
        // 标记为【未考试】状态
        _isAssing = false;

        // 关闭考试界面, 返回考核模式进入界面面板
        SetAssPanelActive(false);
        OnClickTheoryButton(TheoryMode.TheoryAssessment);
    }

    private void OnTheoryBackClick(TheoryBackMode mode)
    {
        if (mode == TheoryBackMode.Normal) BackStartScene();
        else if (mode == TheoryBackMode.Assess) LetgoAss();
        else { }
    }

    // 点击左边按钮列表选择不同的理论面板
    private void OnClickTheoryButton(TheoryMode mode)
    {
        // 如果当前在考试不可对其进行点击
        if (_isAssing == true) return;

        // 在不同的理论模式下，同一个【返回】按钮执行的功能不同
        // 所以需要设置【返回】按钮的功能
        _gameMgr.currTheoryBackMode = (mode & TheoryMode.TheoryAssessment) == 0 ? TheoryBackMode.Normal : TheoryBackMode.Assess;

        // 切换不同的理论按钮
        foreach (var btn in _theoryBtns)
        {
            if ((mode & btn.currMode) != 0)
            {
                btn.RaiseTrigger(InteractionTrigger.Selected);
                btn.SetPanelActive(true);
            }
            else
            {
                btn.RaiseTrigger(InteractionTrigger.DeSelect);
                btn.SetPanelActive(false);
            }
        }
    }
    
    // 点击开始考核按钮
    private void StartAssessment()
    {
        // 更新左侧的理论按钮
        foreach (TheoryElementButton btn in _theoryBtns)
        {
            btn.RaiseTrigger(InteractionTrigger.DeSelect);
            btn.SetPanelActive(false);
        }

        // 重置考核界面
        SetAssPanelActive(true);
        AssReset();

        // 标记为正在考试
        _isAssing = true;
    }

    // 当前题目用户给出答案时
    private void OnOptionSelected(bool isOn, bool isAnswer, string content)
    {
        // 记录
        int currIdx = _assMgr.currIdx;
        int answer = isAnswer ? 1 : 2;
        bool isSingle = _assMgr.GetCurrQuestion().isSingle;

        if (_assMgr.recordArr[currIdx] == null)
            _assMgr.recordArr[currIdx] = new TopicRecordPkg();

        _assMgr.recordArr[currIdx].Record(isOn, content, isSingle);

        // 判断该题目的对错
        _assMgr.ValidateIndexTitle(currIdx);

        // 回答了最后一道题后，出现提交按钮
        if (_assMgr.GetFinishQestionCount() == _assMgr.GetTotalQuestion()) 
        {
            _nextBtn.gameObject.SetActive(false);
            _submitBtn.gameObject.SetActive(true);
        }

        // 更新相关UI
        _currCnt.text = _assMgr.GetFinishQestionCount().ToString();
        RefreshProgress();
    }

    // 切换题目
    private void SwitchTopics(int topicIndex)
    {
        // 设置考试管理器的当前的题目列表索引
        // 设置新的题目/UI更新
        _assMgr.SetQuestionIndex(topicIndex);
        LoadData(_assMgr.GetCurrQuestion());

        // 将上一个题号的控件状态恢复到初始状态
        // 设置新的题号item
        _currNavigatorItem.OnDeClickedItem();
        _currNavigatorItem = _navigationItems[topicIndex];
        _currNavigatorItem.SetNaviStateAndRefresh(NavigationState.Answering);
    }

    // 下一题
    private void NextQuestion()
    {
        int idx = _assMgr.NextQuestion();
        SwitchTopics(idx);
    }

    // 上一题
    private void PrevQuestion()
    {
        int idx = _assMgr.PrevQuestion();
        SwitchTopics(idx);
    }

    // 提交答题(提交按钮点击事件)
    private void SubmitTheAnswer()
    {
        // 检查答题情况
        bool isAllCorrect = true;
        foreach (var i in _assMgr.recordArr)
            isAllCorrect &= (i?.mark == 1);

        // 考核通过
        if (isAllCorrect)
        {
            // 开启实训模式
            _gameMgr.isPar = true;

            //显示通过面板
            _passPanel.RaiseTrigger(InteractionTrigger.Selected);
            _passPanel.gameObject.SetActive(true);
            return;
        }
        
        // 考核不通过。
    }

    #endregion

    #region 工具方法

    // 设置考核面板显示/关闭特效
    public void SetAssPanelActive(bool active)
    {
        InteractionTrigger trigger = active ? InteractionTrigger.Selected : InteractionTrigger.DeSelect;
        _assPanel.RaiseTrigger(trigger);
        _assPanel.gameObject.SetActive(active);
    }

    /// <summary>
    /// 更新考题进度条控件
    /// </summary>
    private void RefreshProgress()
    {
        float target = (float)_assMgr.GetFinishQestionCount() / _assMgr.GetTotalQuestion();
        _percentSlider.DOValue(target, 1.5f).SetEase(Ease.OutCubic);
    }

    #endregion
}
