using SUG_UnityCore.UI;
using UnityEngine;

public class SelectPanel : UIBase
{
    // —— UI Component ——
    [Header("UI组件")]
    [SerializeField] private ButtonInteractive _startBtn;
    [SerializeField] private ButtonInteractive _theoryBtn, _parcticeBtn;

    // ===================
    // Life cycle
    // ===================
    private void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
            
    }
}