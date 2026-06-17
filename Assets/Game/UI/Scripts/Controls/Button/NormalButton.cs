using SUG_UnityCore;

public class NormalButton : ControlBase
{
    // ===================
    // Event
    // ===================
    protected override void OnHoverEnter()
    {
        RaiseTrigger(InteractionTrigger.HoverEnter, ControlType.Normal);
    }

    protected override void OnHoverExit()
    {
        RaiseTrigger(InteractionTrigger.HoverExit, ControlType.Normal);
    }

    protected override void OnClickEnter()
    {
        RaiseTrigger(InteractionTrigger.Selected, ControlType.Normal);
    }
}
