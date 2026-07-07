using SUG.Essentials;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectPanel : UIBase
{
    // —— UI Component ——
    [Header("UI组件")]
    [SerializeField] private UIButton _startBtn;
    [SerializeField] private UIButton _theoryBtn;
    [SerializeField] private ParcticeButton _parcticeBtn;

    // —— Runtime variable ——
    private GameGlobalSettingSO _gameCfg;

    // Inject
    [EInject] private ICfgService _cfgMgr;
    [EInject] private IGameService _gameMgr;
    [EInject] private ISceneManagementService _sceneMgr;

    // ===================
    // Life cycle
    // ===================
    private void Start()
    {
        ComponentInitialized();
        EventInitialized();
    }

    // ===================
    // Initialized
    // ===================
    
    private void ComponentInitialized()
    {
        // 实训按钮初始化
        bool unlock = _gameMgr.isPar;
        _parcticeBtn.Refresh(unlock);
    }

    private void EventInitialized()
    {
        _startBtn.onClickEnter += OnStartSelected;
        _parcticeBtn.onClickEnter += OnPracticeSelected;
        _theoryBtn.onClickEnter   += OnTheorySelected;
    }

    // ===================
    // Event
    // ===================

    private void OnStartSelected()
    {
        _startBtn.RaiseTrigger(InteractionTrigger.Selected);

        if (_gameCfg == null) _gameCfg = _gameMgr.gameSetting;
        if (_gameMgr.currGameMode == GameType.Theory) _sceneMgr.LoadSceneAsync(_gameCfg.theoryScene);
        else if (_gameMgr.currGameMode == GameType.Parctice) _sceneMgr.LoadSceneAsync(_gameCfg.parcitcScene);
        else {}
    }

    private void OnTheorySelected()
    {
        _theoryBtn.RaiseTrigger(InteractionTrigger.Selected);
        _parcticeBtn.RaiseTrigger(InteractionTrigger.DeSelect);
        _gameMgr.currGameMode = GameType.Theory;
    }

    private void OnPracticeSelected()
    {
        _theoryBtn.RaiseTrigger(InteractionTrigger.DeSelect);
        if (_gameMgr.isPar == false)
        {
            _parcticeBtn.Refresh(false);
            _parcticeBtn.RaiseTrigger(InteractionTrigger.UnSelctable);
            _gameMgr.currGameMode = GameType.None;
            return;
        }

        _parcticeBtn.Refresh(true);
        _parcticeBtn.RaiseTrigger(InteractionTrigger.Selected);
        _gameMgr.currGameMode = GameType.Parctice;
    }
}