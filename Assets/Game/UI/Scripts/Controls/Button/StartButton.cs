using UnityEngine;
using SUG_UnityCore;
using System;

public class StartButton : ControlBase
{
    // ===================
    // Event
    // ===================
    protected override void OnHoverEnter()
    {
        RaiseTrigger(InteractionTrigger.HoverEnter, ControlType.Start);
    }

    protected override void OnHoverExit()
    {
        RaiseTrigger(InteractionTrigger.HoverExit, ControlType.Start);
    }

    protected override void OnClickEnter()
    {
        RaiseTrigger(InteractionTrigger.Selected, ControlType.Start);
    }
}