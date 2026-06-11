using SUG_UnityCore;
using UnityEngine;

public class NormalButton : ButtonBase
{
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
        if (ConfigManager.Get().HasConfig<UIInteractionSound>())
        {
            AudioClip hoverClip = ConfigManager.Get().GetConfig<UIInteractionSound>().norBtnHover;
            AudioManager.Get().Play(hoverClip);
        }
    }

    private void OnHoverExit()
    {
        //transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 1.0f);
    }

    private void OnClickEvent()
    {
        if (ConfigManager.Get().HasConfig<UIInteractionSound>())
        {
            AudioClip selectClip = ConfigManager.Get().GetConfig<UIInteractionSound>().norBtnSelect;
            AudioManager.Get().Play(selectClip);
        }
    }
}
