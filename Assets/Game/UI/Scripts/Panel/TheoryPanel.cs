using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUG_UnityCore;

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
    }

    // ======================
    // Initialized
    // ======================
    private void EventInitialization()
    {
        foreach (TheoryElementButton btn in _theoryBtns)
        {
            btn.onSelect += OnClickTheoryButton;
        }
    }

    // ======================
    // Event
    // ======================

    // Theory buttons selected event.
    private void OnClickTheoryButton(TheoryMode mode)
    {
        foreach (var btn in _theoryBtns)
        {
            if ((mode & btn.currMode) != 0) btn.RaiseTrigger(InteractionTrigger.Selected);
            else btn.RaiseTrigger(InteractionTrigger.DeSelect);
        }
    }
    
}
