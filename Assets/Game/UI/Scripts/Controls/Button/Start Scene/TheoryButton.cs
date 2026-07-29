using SUG.Essentials;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TheoryButton : UIButton
{
    // Runtime
    private bool isSelected = false;

    #region Life cycle

    private void Awake()
    {

    }

    #endregion 

    #region Event

    /// <summary>
    ///  取消被选中的状态
    /// </summary>
    public void OnDeSelect()
    {
        isSelected = false;
        RaiseTrigger(InteractionTrigger.DeSelect);
    }

#   endregion

#   region Override

    /// <summary>
    /// Click Enter 重写
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerClick(PointerEventData eventData)
    {
        // 点击后记录为选中状态
        base.OnPointerClick(eventData);
        isSelected = true;
    }

    /// <summary>
    /// Hover Exit重写
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerExit(PointerEventData eventData)
    {
        // 当没有被点击选中才可以执行HoverExit事件
        if (isSelected == false)
            base.OnPointerExit(eventData);
    }

#endregion
}
