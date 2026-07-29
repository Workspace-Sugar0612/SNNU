using DG.Tweening;
using SUG.Essentials;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private UITMPText _titleContext; // 题目
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
    [SerializeField] private Image _scoreSilder; // 分数进度条
    [SerializeField] private TextMeshProUGUI _finalScoreTx, _correctTx, _wrongTx; // 总分数，正确和错误题目数量文本控件
    [SerializeField] private FailOptionItem _failPrefab; // 答题情况区域的预制体
    [SerializeField] private Transform _failParent; // 生成模型的父物体
    [SerializeField] private TextMeshProUGUI _analysisTx; // 解析题目内容
    [SerializeField] private TextMeshProUGUI _analysisNumTx; // 解析面板序号文本
    [SerializeField] private Image _analysisNumImg; // 解析面板序号ICON
    [SerializeField] private AnalysisItem _analysisItemPrefab; // 分析题目面板Item预制体
    [SerializeField] private Transform _analysisItemParent; // 分析题目面板Item预制体父类
    [SerializeField] private TextMeshProUGUI _playOptionsTx, _questionScore; // 玩家该题的选择答案, 改题目分数
    [SerializeField] private TextMeshProUGUI _analysisAnswerTx; // 答案解析文本
    [SerializeField] private GameObject _analysisAnswerPanel; // 答案解析面板
    [SerializeField] private UIButton _analysisAnswerButton; // 答案解析按钮

    [Header("UI 素材")]
    [SerializeField] private Sprite _correctSprite;
    [SerializeField] private Sprite _wrongSprite;
    [SerializeField] private Sprite _analysisNormalSprite; // 分析选项表示默认icon
    [SerializeField] private Sprite _analysisCorrectSprite; // 分析选项表示正确的icon

    // Data container

    // 题目引导管理容器
    private readonly List<AssOption> _assOptions = new List<AssOption>();
    private readonly List<QuestionNavigatorItem> _navigationItems = new List<QuestionNavigatorItem>();
    private readonly List<FailOptionItem> _failOptions = new List<FailOptionItem>();
    private readonly List<AnalysisItem> _analysisItems = new List<AnalysisItem>();

    // Inject
    [Inject] private IGameService  _gameMgr;
    [Inject] private ISceneService _sceneMgr;
    [Inject] private IAssService _assMgr;

    // Assessment data.
    private QuestionData _currData; // 当前题目内容数据
    private bool _isAssing = false; // 是否在考试
    private int _analysisIndex = 0;
    private bool _assCanTwitch = false; // 是否允许切换题目

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
        _theoryBtns[0]?.RaiseTrigger(InteractionTrigger.Selected);

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
        _analysisAnswerButton.onHoverEnter += DisplayAnalysisAnswerPanel;
        _analysisAnswerButton.onHoverExit += () => { _analysisAnswerPanel.gameObject.SetActive(false); };
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
        IEnumerator InitializtionOptions()
        {
            // 生成选项并隐藏
            foreach (var op in data.options)
            {
                // 当前题目选项创建
                AssOption option = Essentials.Instantiate(_optionPrefab, _optionGroup.transform);
                option.Setup(op.isAnswer, op.content, data.isSingle, _optionGroup);
                option.onTrigger += OnOptionSelected;
                option.Verify(pkg?.selectContents);

                option.SetActiveAnimVer(false, 0.0f);
                option.SetActive(false);

                // 将创建好的option添加到管理列表中
                _assOptions.Add(option);
            }

            yield return new WaitForSeconds(0.1f);

            // 渐变展示options
            foreach (var op in _assOptions)
            {
                op.SetActive(true);
                op.SetActiveAnimVer(true, 1.0f);
            }

            // 刷新界面，使得排布对齐
            RefreshCanvas(_optionGroup.transform as RectTransform);

            // 允许切换题目
            _assCanTwitch = true;
        }

        _titleContext.SetText(data.title, TextDisplayMode.Wbw, () => StartCoroutine(InitializtionOptions()));

        // 更新题目进度窗口的【已答数/总共数】控件的信息内容。
        _totalCnt.text = _assMgr.GetTotalQuestion().ToString();
        _currCnt.text = _assMgr.GetFinishQestionCount().ToString();
        RefreshProgress();

        // 更新Canvas，使面板的Layout可以正常的排布UI控件
        RefreshCanvas(_assRect);
        RefreshCanvas(_naviRect);
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

    /// <summary>
    /// 返回开始选择界面
    /// </summary>
    private void BackStartScene()
    {
        var _gameCfg = _gameMgr.gameSetting;

        _sceneMgr.LoadSceneAsync(_gameCfg.startScene);
    }

    /// <summary>
    /// 放弃考试时
    /// </summary>
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

    /// <summary>
    /// 点击左边按钮列表选择不同的理论面板
    /// </summary>
    /// <param name="mode"></param>
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

    /// <summary>
    ///  点击开始考核按钮
    /// </summary>
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

    /// <summary>
    /// 当前题目用户给出答案时
    /// </summary>
    /// <param name="isOn"></param>
    /// <param name="isAnswer"></param>
    /// <param name="content"></param>
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

    /// <summary>
    /// 切换题目
    /// </summary>
    /// <param name="topicIndex"></param>
    private void SwitchTopics(int topicIndex)
    {
        if (!_assCanTwitch)
            return;

        _assCanTwitch = false;

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

    /// <summary>
    /// 下一题
    /// </summary>
    private void NextQuestion()
    {
        if (_assCanTwitch == false)
            return;

        int idx = _assMgr.NextQuestion();
        SwitchTopics(idx);
    }

    /// <summary>
    /// 上一题
    /// </summary>
    private void PrevQuestion()
    {
        if (_assCanTwitch == false)
            return;

        int idx = _assMgr.PrevQuestion();
        SwitchTopics(idx);
    }

    /// <summary>
    /// 提交答题(提交按钮点击事件)
    /// </summary>
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
            _assPanel.gameObject.SetActive(false);
            return;
        }

        // 考核不通过。

        // 获取分数、正确题数、错误题数
        int wrong        = _assMgr.wrongCount;
        int correct      = _assMgr.correctCount;
        float finalScore = _assMgr.finalScore;

        // 更新UI界面，显示考试不通过界面，播放控件动画
        _assPanel.gameObject.SetActive(false);
        _failPanel.RaiseTrigger(InteractionTrigger.Selected);

        _failPanel.gameObject.SetActive(true);
        foreach (TheoryElementButton btn in _theoryBtns)
        {
            btn.RaiseTrigger(InteractionTrigger.DeSelect);
            btn.SetPanelActive(false);
            btn.gameObject.SetActive(false);
        }

        DOTween.To(() => 0f, x => _wrongTx.text = Mathf.RoundToInt(x).ToString(), wrong, 1.5f);
        DOTween.To(() => 0f, x => _correctTx.text = Mathf.RoundToInt(x).ToString(), correct, 1.5f);
        DOTween.To(() => 0f, x => { _finalScoreTx.text = x.ToString("F1"); _scoreSilder.fillAmount = (x / 100.0f); }, finalScore, 1.5f);

        // 左侧控件按钮生成
        _failOptions.Clear();
        for (int i = 0; i < _assMgr.recordArr.Count(); ++i)
        {
            var foi = Essentials.Instantiate(_failPrefab, _failParent);
            foi.Setup(_assMgr.recordArr[i].mark == 2, i, _correctSprite, _wrongSprite);
            foi.gameObject.SetActive(true);
            foi.openThisQuestion += DisplayQuestionAnalysis;
            _failOptions.Add(foi);
        }

        // 默认显示第一个
        _analysisIndex = 0;
        DisplayQuestionAnalysis(0);
    }

    /// <summary>
    /// 展示题目解析
    /// </summary>
    private void DisplayQuestionAnalysis(int index)
    {
        if (index < 0 || index >= _assMgr.questionList.Count
            || index >= _assMgr.recordArr.Count())
            return;

        _analysisIndex = index; // 记录解析题目索引

        // 获取此index的题目信息和答题记录
        var data = _assMgr.questionList[index];
        var record = _assMgr.recordArr[index];

        // 初始化解析面板
        _analysisNumTx.text    = index.ToString();
        _analysisNumImg.sprite = record.mark == 1 ? _correctSprite : _wrongSprite;
        _analysisTx.text = data.title;

        // 初始化题目选项
        foreach (var item in _analysisItems)
            item.gameObject.SetActive(false);

        _analysisItems.Clear();
        foreach (var op in data.options)
        {
            Sprite sp = op.isAnswer ? _analysisCorrectSprite : _analysisNormalSprite;
            Color tc = op.isAnswer ? new Color(0.0f, 0.8f, 0.5f) : new Color(1.0f, 1.0f, 1.0f);
            var aio = Essentials.Instantiate(_analysisItemPrefab, _analysisItemParent);
            aio.Setup(op.content, sp, tc);
            aio.gameObject.SetActive(true);
            _analysisItems.Add(aio);

            // 刷新UI，让item适配当前文本内容
            RefreshCanvas(aio.analysisTxRect);
        }

        // 还原玩家该题的选项
        string options = "";
        foreach (var sc in record.selectContents)
            options += _assMgr.GetOptionLetter(sc);

        _playOptionsTx.text = options;
        RefreshCanvas(_playOptionsTx.transform as RectTransform);

        // 显示该题目分数
        _questionScore.text = data.score.ToString("F1");
        RefreshCanvas(_questionScore.transform as RectTransform);

        // 刷新整个Options的Rect
        RefreshCanvas(_analysisItemParent as RectTransform);
    }

    /// <summary>
    /// 显示答案解析面板
    /// </summary>
    private void DisplayAnalysisAnswerPanel()
    {
        // 获取题目解析数据，并更新UI
        var data = _assMgr.questionList[_analysisIndex];
        _analysisAnswerTx.text = data.analysis;
        _analysisAnswerPanel.gameObject.SetActive(true);
        RefreshCanvas(_analysisAnswerPanel.transform as RectTransform);
    }

    #endregion

    #region 工具方法

    /// <summary>
    /// 设置考核面板显示/关闭特效
    /// </summary>
    /// <param name="active"></param>
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

    /// <summary>
    /// 更新Canvas，使面板的Layout可以正常的排布UI控件
    /// </summary>
    /// <param name="rect"></param>
    private void RefreshCanvas(RectTransform rect)
    {
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    #endregion
}
