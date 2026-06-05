using DG.Tweening;
using SUG_UnityCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NormalButton : ButtonInteractive
{
    // —— Private variable ——
    [Header("交互变量")]
    [SerializeField] private Vector3 _normalScale = new Vector3(1.0f, 1.0f, 1.0f);
    [SerializeField] private Vector3 _hoverScale = new Vector3(1.2f, 1.2f, 1.2f);
    [SerializeField] private Vector3 _selectScale = new Vector3(0.6f, 0.6f, 0.6f);

    private const float HOVER_DUR = 1.0f;
    private const float CLICK_DUR = 0.3f;

    // ===================
    // Life cycle
    // ===================
    private void Start()
    {
        Initialized();
    }

    // ===================
    // Initialized
    // ===================
    private void Initialized()
    {
        onHoverEnter += () => OnHoverEnter();
        onHoverExit += () => OnHoverExit();
        onClickEnter += () => OnClickEvent();
    }

    // ===================
    // Event
    // ===================
    private void OnHoverEnter()
    {
        SetScale(_hoverScale, HOVER_DUR);
        if (ConfigManager.Get().HasConfig<UIInteractionSound>())
        {
            AudioClip hoverClip = ConfigManager.Get().GetConfig<UIInteractionSound>().norBtnHover;
            AudioManager.Get().Play(hoverClip);
        }
    }

    private void OnHoverExit()
    {
        SetScale(_normalScale, HOVER_DUR);

        transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 1.0f);
    }

    private void OnClickEvent()
    {
        SetScale(_selectScale, CLICK_DUR, () => SetScale(_normalScale, CLICK_DUR));

        if (ConfigManager.Get().HasConfig<UIInteractionSound>())
        {
            AudioClip selectClip = ConfigManager.Get().GetConfig<UIInteractionSound>().norBtnSelect;
            AudioManager.Get().Play(selectClip);
        }
    }

    // ===================
    // Toolkit Function
    // ===================
    private void SetScale(Vector3 target, float duration, Action callback = null)
    {
        transform.DOScale(target, duration).OnComplete(() => { callback?.Invoke(); });
    }
}
