using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUG_UnityCore;

public class TheoryPanel : UIBase
{
    // —— Runtime variable ——
    [SerializeField] private UIButton _backBtn;
    [SerializeField] private UIButton _safeBtn;

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
        _safeBtn.onHoverEnter += HoverEnterSafeBtn;
        _backBtn.onHoverEnter += HoverEnterBackBtn;
    }

    // ======================
    // Event
    // ======================
    private void HoverEnterSafeBtn()
    {
        _safeBtn.RaiseTrigger(InteractionTrigger.HoverEnter);
    }

    private void HoverEnterBackBtn()
    {
        _backBtn.RaiseTrigger(InteractionTrigger.HoverEnter);
    }
}
