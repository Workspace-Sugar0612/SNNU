using SUG.Essentials;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public sealed class ParcticeButton : UIButton
{
    // —— Component variable ——
    [SerializeField] private GameObject _lockMask;

    // —— Config variable ——
    [SerializeField] private Vector3 _lockScale;
    [SerializeField] private Vector3 _norScale;

    // Event
    public event Action onSelected;

    // ===================
    // Initialized
    // ===================
    public void Refresh(bool unlock)
    {
        //transform.localScale = unlock ? _norScale : _lockScale;
        _lockMask.gameObject.SetActive(!unlock);
    }

    #region Override

    public override void OnPointerClick(PointerEventData eventData)
    {
        // base.OnPointerClick(eventData);
        onSelected?.Invoke();
    }

    #endregion 
}
