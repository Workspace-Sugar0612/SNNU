using System;
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

    // 选择事件
    public event Action<bool> onTrigger = null;

    // 生命周期函数
    private void Start()
    {
        _selected.onValueChanged.AddListener((_) => { onTrigger?.Invoke(_isAnswer); });
    }

    /// <summary>
    /// 初始化设置
    /// </summary>
    /// <param name="isAnswer">是否时正确选项</param>
    /// <param name="content">选项内容</param>
    /// <param name="isSingle">是否是单选题</param>
    /// <param name="group">Toggle Group父类组件</param>
    public void Setup(bool isAnswer, string content, bool isSingle, ToggleGroup group)
    {
        _isAnswer = isAnswer;
        _contentTx.text = content;
        _selected.isOn = false;
        _group = group;
        if (isSingle) _selected.group = _group;
    }

    // 设置激活
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}