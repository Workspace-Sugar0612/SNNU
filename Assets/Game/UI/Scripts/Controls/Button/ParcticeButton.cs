using SUG_UnityCore;
using Unity.VisualScripting;
using UnityEngine;

public class ParcticeButton : ControlBase
{
    // —— Component variable ——
    [SerializeField] private GameObject _lockMask;

    // —— Config variable ——
    [SerializeField] private Vector3 _lockScale;
    [SerializeField] private Vector3 _norScale;

    // ===================
    // Life cycle
    // ===================
    private void Start()
    {
        Initialzaction();
    }

    // ===================
    // Initialized
    // ===================
    private void Initialzaction()
    {
        bool par = GameMode.Get().isPar;
        transform.localScale = par ? _norScale : _lockScale;
        _lockMask.gameObject.SetActive(!par);
    }

    // ===================
    // Event
    // ===================
    protected override void OnHoverEnter()
    {
        RaiseTrigger(InteractionTrigger.HoverEnter, ControlType.Normal);
    }

    protected override void OnHoverExit()
    {
        RaiseTrigger(InteractionTrigger.HoverExit, ControlType.Normal);
    }

    protected override void OnClickEnter()
    {
        if (GameMode.Get().isPar == false)
        {
            _lockMask.gameObject.SetActive(true);
            transform.localScale = _lockScale;
            RaiseTrigger(InteractionTrigger.UnSelctable, ControlType.Normal);
            return;
        }
        
        _lockMask.gameObject.SetActive(false);
        transform.localScale = _norScale;
        RaiseTrigger(InteractionTrigger.Selected, ControlType.Normal);
    }
}
