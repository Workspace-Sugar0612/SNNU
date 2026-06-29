using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUG.UnityCore;
using UnityEngine.SceneManagement;

public enum TheoryMode 
{
    None = 0, 
    CourseIntro = 1 << 0, // 课程介绍
    SafetySpec = 1 << 1, // 安全规范
    TheoryKnowledge = 1 << 2, // 理论规范
    TheoryAssessment = 1 << 3 // 理论考核
}

public class TheoryPanel : UIBase
{
    // —— UI variable ——
    [SerializeField] private UIButton _backBtn;
    [SerializeField] private List<TheoryElementButton> _theoryBtns = new List<TheoryElementButton>();

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

        _backBtn.onClickEnter += OnClickBack;
    }

    // ======================
    // Event
    // ======================

    private void OnClickBack()
    {
        var _gameCfg =  ConfigManager.Get().GetConfig<GameGlobalSetting>();
        SceneManager.LoadSceneAsync(_gameCfg.startScene);
    }

    // Theory buttons selected event.
    private void OnClickTheoryButton(TheoryMode mode)
    {
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
