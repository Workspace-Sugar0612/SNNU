using SUG_UnityCore;
using UnityEngine;

public class SelectPanel : UIBase
{
    // —— UI Component ——
    [Header("UI组件")]
    [SerializeField] private ButtonBase _startBtn;
    [SerializeField] private ButtonBase _theoryBtn, _parcticeBtn;

    // —— Runtime variable ——
    private ImageEdgeFlowEffect _theoryBtnEffect, _practiceBtnEffect;

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
        _startBtn.onClickEnter += () => OnStartClickEvent();
        _theoryBtn.onClickEnter += () => OnTheoryClickEvent();
        _parcticeBtn.onClickEnter += () => OnPracticeClickEvent();
    }

    private void ComponentInitialized()
    {
        _theoryBtnEffect = _theoryBtn.GetComponentInChildren<ImageEdgeFlowEffect>();
        if (_theoryBtnEffect != null)   _theoryBtnEffect.StopFlow();

        _practiceBtnEffect = _parcticeBtn.GetComponentInChildren<ImageEdgeFlowEffect>();
        if (_practiceBtnEffect != null) _practiceBtnEffect.StopFlow();
    }


    // ===================
    // Event
    // ===================
    private void OnStartClickEvent()
    {

    }

    private void OnTheoryClickEvent()
    {
       if (_theoryBtnEffect != null)   _theoryBtnEffect.PlayFlow();
       if (_practiceBtnEffect != null) _practiceBtnEffect.StopFlow();
    }

    private void OnPracticeClickEvent()
    {
       if (_theoryBtnEffect != null)   _theoryBtnEffect.StopFlow();
       if (_practiceBtnEffect != null) _practiceBtnEffect.PlayFlow();
    }
}