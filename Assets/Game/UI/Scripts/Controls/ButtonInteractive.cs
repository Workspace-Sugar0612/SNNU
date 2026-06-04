using SUG_UnityCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UGUI的UI控件交互基类
/// </summary>
public abstract class ButtonInteractive : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // —— Interactive Event ——
    protected event Action onHoverEnter;
    protected event Action onHoverExit;
    protected event Action onClickEnter;

    // ===================
    // Interface
    // ===================
    public void OnPointerEnter(PointerEventData eventData) => onHoverEnter?.Invoke();
    public void OnPointerExit(PointerEventData eventData) => onHoverExit?.Invoke();
    public void OnPointerClick(PointerEventData eventData) => onClickEnter?.Invoke();

}
