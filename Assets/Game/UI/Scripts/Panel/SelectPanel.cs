using SUG_UnityCore;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : UIBase
{
    // —— UI Component ——
    [Header("UI组件")]
    [SerializeField] private ControlBase _startBtn;
    [SerializeField] private ControlBase _theoryBtn, _parcticeBtn;

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

    private void EventInitialized()
    {
        _parcticeBtn.onClickEnter += OnPracticeClickEvent;
        _theoryBtn.onClickEnter += OnTheoryClickEvent;
    }

    private void ComponentInitialized()
    {
        
    }


    // ===================
    // Event
    // ===================
    private void OnStartClickEvent()
    {

    }

    private void OnTheoryClickEvent()
    {
       _parcticeBtn.RaiseTrigger(InteractionTrigger.DeSelect);
    }

    private void OnPracticeClickEvent()
    {
        _theoryBtn.RaiseTrigger(InteractionTrigger.DeSelect);
    }
}