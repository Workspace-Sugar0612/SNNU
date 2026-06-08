using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UGUI的UI控件交互基类
/// </summary>
public abstract class ButtonInteractive : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // —— Interactive Event ——
    public event Action onHoverEnter;
    public event Action onHoverExit;
    public event Action onClickEnter;

    // ===================
    // Interface
    // ===================
    public void OnPointerEnter(PointerEventData eventData) => onHoverEnter?.Invoke();
    public void OnPointerExit(PointerEventData eventData) => onHoverExit?.Invoke();
    public void OnPointerClick(PointerEventData eventData) => onClickEnter?.Invoke();
}
