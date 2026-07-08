using SUG.Essentials;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheoryPanel : UIBase
{
    // —— UI variable ——
    [SerializeField] private TheoryBackButton _backBtn;
    [SerializeField] private List<TheoryElementButton> _theoryBtns = new List<TheoryElementButton>();
    [SerializeField] private UIButton _startTheory; // 开始考核

    // Inject
    [EInject] private IGameService  _gameMgr;
    [EInject] private ISceneService _sceneMgr;

    // ======================
    // Life cycle
    // ======================
    private void Start()
    {
        EventInitialization();
        StartCoroutine(Initializaction());
    }

    // ======================
    // Initialized
    // ======================

    IEnumerator Initializaction()
    {
        yield return null;
        foreach (TheoryElementButton btn in _theoryBtns) { btn.TogglePanel(); }
        _theoryBtns[0]?.OnPointClick();
    }

    private void EventInitialization()
    {
        foreach (TheoryElementButton btn in _theoryBtns)
        {
            btn.onSelect += OnClickTheoryButton;
        }

        _backBtn.onSelected += OnTheoryBackClick;
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
    

    public void OnHoverEnter()
    {
        Debug.Log("Enter");
    }
}
