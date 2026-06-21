using SUG_UnityCore;
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
    private GameGlobalSetting _gameCfg;

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
        bool unlock = GameMode.Get().isPar;
        _parcticeBtn.Refresh(unlock);
    }

    private void EventInitialized()
    {
        _startBtn.onHoverEnter += OnStartHoverEnter;
        _startBtn.onHoverExit  += OnStartHoverExit;
        _startBtn.onClickEnter += OnStartSelected;
        _parcticeBtn.onClickEnter += OnPracticeSelected;
        _theoryBtn.onClickEnter   += OnTheorySelected;
    }

    // ===================
    // Event
    // ===================
    private void OnStartHoverEnter()
    {
        _startBtn.RaiseTrigger(InteractionTrigger.HoverEnter);
    }

    private void OnStartHoverExit()
    {
        _startBtn.RaiseTrigger(InteractionTrigger.HoverExit);
    }

    private void OnStartSelected()
    {
        _startBtn.RaiseTrigger(InteractionTrigger.Selected);

        if (_gameCfg == null) _gameCfg =  ConfigManager.Get().GetConfig<GameGlobalSetting>();
        if (GameMode.Get().currGameMode == GameMode.GameType.Theory) SceneManager.LoadSceneAsync(_gameCfg.theoryScene);
        else if (GameMode.Get().currGameMode == GameMode.GameType.Parctice) SceneManager.LoadSceneAsync(_gameCfg.parcitcScene);
        else {}
    }

    private void OnTheorySelected()
    {
        _theoryBtn  .RaiseTrigger(InteractionTrigger.Selected);
        _parcticeBtn.RaiseTrigger(InteractionTrigger.DeSelect);
        GameMode.Get().currGameMode = GameMode.GameType.Theory;
    }

    private void OnPracticeSelected()
    {
        _theoryBtn.RaiseTrigger(InteractionTrigger.DeSelect);
        if (GameMode.Get().isPar == false)
        {
            _parcticeBtn.Refresh(false);
            _parcticeBtn.RaiseTrigger(InteractionTrigger.UnSelctable);
            return;
        }

        _parcticeBtn.Refresh(true);
        _parcticeBtn.RaiseTrigger(InteractionTrigger.Selected);
        GameMode.Get().currGameMode = GameMode.GameType.Parctice;
    }
}