using SUG.Essentials;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    [Header("答题进度区域")]
    [SerializeField] private TextMeshProUGUI _currCnt; // 当前完成的题目数量
    [SerializeField] private TextMeshProUGUI _totalCnt; // 全部题目数量
    [SerializeField] private Slider _percentSlider; // 答题进度条
    [SerializeField] private Transform _titlementContent; // 答题题号按钮父类
    [SerializeField] private GameObject _titlementPrefab; // 答题题号预制体

    // Inject
    [EInject] private IGameService  _gameMgr;
    [EInject] private ISceneService _sceneMgr;
    [EInject] private IAssService _assMgr;

    // Assessment data.
    private QuestionData _currData;

    // ======================
    // Life cycle
    // ======================
    private void Start()
    {
        DataInitialization();
        EventInitialization();
        StartCoroutine(Initializaction());
    }

    // ======================
    // Initialized
    // ======================

    private void DataInitialization()
    {
        _currData = _assMgr.GetCurrQuestion();
        LoadData(_currData);
    }

    IEnumerator Initializaction()
    {
        yield return null;

        // 初始化理论考核按钮
        foreach (TheoryElementButton btn in _theoryBtns) { btn.TogglePanel(); }
        _theoryBtns[0]?.OnPointClick();

        // 初始化考核面板
        SetAssPanelActive(false);
    }

    private void EventInitialization()
    {
        foreach (TheoryElementButton btn in _theoryBtns)
        {
            btn.onSelect += OnClickTheoryButton;
        }

        _backBtn.onSelected += OnTheoryBackClick;
        _startTheory.onClickEnter += StartAssessment;
    }

    private void LoadData(QuestionData data)
    {
        _titleContext.text = data.title;
        foreach (var op in data.options)
        {
            AssOption option = Essentials.Instantiate(_optionPrefab, _optionGroup.transform);
            option.Setup(op.isAnswer, op.content);
        }
    }

    // ======================
    // Event
    // ======================

    // Theory back button selected event.
    private void OnNormalBack()
    {
        var _gameCfg = _gameMgr.gameSetting;

        _sceneMgr.LoadSceneAsync(_gameCfg.startScene);
    }

    private void OnAssBack()
    {

    }

    private void OnTheoryBackClick(TheoryBackMode mode)
    {
        if ((mode & TheoryBackMode.Normal) != 0) OnNormalBack();
        else if (mode == TheoryBackMode.Assess) OnAssBack();
        else { }
    }

    // Theory buttons selected event.
    private void OnClickTheoryButton(TheoryMode mode)
    {
        _gameMgr.currTheoryBackMode = (mode & TheoryMode.TheoryAssessment) == 0 ? TheoryBackMode.Normal : TheoryBackMode.Assess;
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
    
    // Theory assessment start.
    private void StartAssessment()
    {
        foreach (var btn in _theoryBtns)
        {
            btn.RaiseTrigger(InteractionTrigger.DeSelect);
            btn.SetPanelActive(false);
        }
        SetAssPanelActive(true);
    }

    // 设置考核面板显示/关闭特效
    public void SetAssPanelActive(bool active)
    {
        InteractionTrigger trigger = active ? InteractionTrigger.Selected : InteractionTrigger.DeSelect;
        _assPanel.RaiseTrigger(trigger);
        _assPanel.gameObject.SetActive(active);
    }

    public void OnHoverEnter()
    {
        Debug.Log("Enter");
    }
}
