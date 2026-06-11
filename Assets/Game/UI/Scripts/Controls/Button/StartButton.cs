using UnityEngine;
using SUG_UnityCore;

public class StartButton : ButtonBase
{

    // —— Runtime variable ——
    private ScaleEffect _scaleEffect;

    // —— Config variable ——
    [SerializeField] private Vector3 _normalScale = new Vector3(1.0f, 1.0f, 1.0f);
    [SerializeField] private Vector3 _hoverScale  = new Vector3(1.1f, 1.1f, 1.1f);
    [SerializeField] private Vector3 _selectScale = new Vector3(0.9f, 0.9f, 0.9f);

    // ===================
    // Life cycle
    // ===================
    private void Start()
    {
        ComponentInitialized();
        EventInitialized();
    }

    // ===================
    // Initialized
    // ===================

    private void ComponentInitialized()
    {
        _scaleEffect = GetComponent<ScaleEffect>();
        if (_scaleEffect == null) _scaleEffect = gameObject.AddComponent<ScaleEffect>();      
        _scaleEffect.Setup(_normalScale, _hoverScale, _selectScale);
    }

    private void EventInitialized()
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
            AudioClip hoverClip = ConfigManager.Get().GetConfig<UIInteractionSound>().startBtnHover;
            AudioManager.Get().Play(hoverClip);
        }
        _scaleEffect.OnZoomIn();
    }

    private void OnHoverExit()
    {
        _scaleEffect.OnNormal();
    }

    private void OnClickEvent()
    {
        if (ConfigManager.Get().HasConfig<UIInteractionSound>())
        {
            AudioClip selectClip = ConfigManager.Get().GetConfig<UIInteractionSound>().startBtnSelect;
            AudioManager.Get().Play(selectClip);
        }
        _scaleEffect.OnZoomOut(() => { _scaleEffect.OnNormal(); });
    }
}