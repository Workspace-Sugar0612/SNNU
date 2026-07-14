using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 选项类
/// </summary>
public class AssOption : MonoBehaviour
{
    // 这个选项是否是答案
    private bool _isAnswer = false;
    
    // 选项内容
    [SerializeField] private TextMeshProUGUI _contentTx;
    [SerializeField] private Toggle _selected;
    private ToggleGroup _group;

    // 初始化设置
    public void Setup(bool isAnswer, string content, ToggleGroup group)
    {
        _isAnswer = isAnswer;
        _contentTx.text = content;
        _selected.isOn = false;
        _group = group;
        _selected.group = _group;
    }

    // 设置激活
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}