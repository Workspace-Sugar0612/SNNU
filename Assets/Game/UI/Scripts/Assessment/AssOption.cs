using System;
using System.Collections.Generic;
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
    // bool: 选中还是取消
    // bool: 这个选项是否正确
    // string：选项内容
    public event Action<bool, bool, string> onTrigger = null;

    // 生命周期函数
    private void Start()
    {
        _selected.onValueChanged.AddListener((_) => { onTrigger?.Invoke(_, _isAnswer, _contentTx.text); });
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
    
    /// <summary>
    /// 核对内容
    /// 如果这个题在本轮考试中回答了
    /// 那么当再次切换到该题的时候，应该显示之前回答过的选项
    /// </summary>
    /// <param name="content"></param>
    public void Verify(List<string> contents)
    {
        if (contents == null)
            return;

        _selected.isOn = contents.Contains(_contentTx.text);
    }

    // 设置激活
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}