using UnityEngine;
using SUG_UnityCore;
using System;

public sealed class TheoryElementButton : UIButton
{
    // —— Event variable ——
    public event Action<TheoryMode> onSelect;

    [Header("理论模式类型")]
    public TheoryMode currMode;

    // ================
    // Life cycle
    // ================
    private void Start()
    {
        onClickEnter += () => onSelect?.Invoke(currMode);
        //onHoverEnter += () => RaiseTrigger(InteractionTrigger.HoverEnter);
        //onHoverExit  += () => RaiseTrigger(InteractionTrigger.HoverExit);
    }
}