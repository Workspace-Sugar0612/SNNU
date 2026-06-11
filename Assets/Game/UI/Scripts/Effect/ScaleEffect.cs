using UnityEngine;
using SUG_UnityCore;
using DG.Tweening;
using System;

public class ScaleEffect : MonoBehaviour
{
    // —— Private variable ——
    [Header("交互变量")]
    public Vector3 _normalScale = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 _hoverScale = new Vector3(1.2f, 1.2f, 1.2f);
    public Vector3 _selectScale = new Vector3(0.6f, 0.6f, 0.6f);

    // —— Public variable ——
    [Header("动画时长")]
    public float hoverDur = 1.0f;
    public float clickDur = 0.3f;

    // ===================
    // Initialized
    // ===================
    public void Setup(Vector3 normal, Vector3 hover, Vector3 select, float hoverDuration = 0.8f, float clickDuration = 0.2f)
    {
        _normalScale = normal;
        _hoverScale  = hover;
        _selectScale = select;
        hoverDur = hoverDuration;
        clickDur = clickDuration;
    }

    // ===================
    // Core
    // ===================
    public void OnZoomIn(Action callback = null)
    {
        SetScale(_hoverScale, hoverDur, callback);
    }

    public void OnNormal(Action callback = null)
    {
        SetScale(_normalScale, hoverDur, callback);
    }

    public void OnZoomOut(Action callback = null)
    {
        SetScale(_selectScale, clickDur, callback);
    }

    // ===================
    // Toolkit Function
    // ===================
    private void SetScale(Vector3 target, float duration, Action callback = null)
    {
        transform.DOScale(target, duration).OnComplete(() => { callback?.Invoke(); });
    }
}