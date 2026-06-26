using UnityEngine;
using SUG_UnityCore;
using System;
using DG.Tweening;

public sealed class TheoryElementButton : UIButton
{
    // —— Event variable ——
    public event Action<TheoryMode> onSelect;

    [Header("理论模式类型")]
    public TheoryMode currMode;

    [Header("对应需要展示的面板"), SerializeField]
    private GameObject targetPanel;

    // 面板伸缩动画参数
    [Header("面板伸缩参数")]
    [SerializeField] private Vector3 _showScale = new Vector3(0.75f, 0.75f, 0.75f);
    [SerializeField] private Vector3 _hideScale = Vector3.zero;
    [SerializeField] private float _scaleDuration = 0.5f;

    // 面板激活参数
    private bool panelActive = false;

    // ================
    // Life cycle
    // ================
    private void Awake()
    {
        onClickEnter += () => onSelect?.Invoke(currMode);
        //onHoverEnter += () => RaiseTrigger(InteractionTrigger.HoverEnter);
        //onHoverExit  += () => RaiseTrigger(InteractionTrigger.HoverExit);

        //initializaction();
    }

    // initializaction
    private void initializaction()
    {
        targetPanel?.SetActive(false);
        SetPanelActive(false); 
    }

    // Target panel action.

    public void TogglePanel()
    {
        SetPanelActive(panelActive);
    }

    public void SetPanelActive(bool active)
    {
        Vector3 tarScale = active ? _showScale : _hideScale;
        targetPanel?.transform.DOScale(tarScale, _scaleDuration);
        targetPanel?.SetActive(active);
        panelActive = !panelActive;
    }

    // Interface
    public void OnSelected() => OnPointClick();
}