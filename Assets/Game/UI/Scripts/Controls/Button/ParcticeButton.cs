using SUG_UnityCore;
using Unity.VisualScripting;
using UnityEngine;

public sealed class ParcticeButton : UIButton
{
    // —— Component variable ——
    [SerializeField] private GameObject _lockMask;

    // —— Config variable ——
    [SerializeField] private Vector3 _lockScale;
    [SerializeField] private Vector3 _norScale;

    // ===================
    // Initialized
    // ===================
    public void Refresh(bool unlock)
    {
        transform.localScale = unlock ? _norScale : _lockScale;
        _lockMask.gameObject.SetActive(!unlock);
    }
}
