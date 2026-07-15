using SUG.Essentials;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum NavigationState
{
    High, Moderate, Low
}

[Serializable] public class NavigationUIPkg
{
    [SerializeField] public Sprite sprite;
    [SerializeField] public Color fontColor;
}

[Serializable] public class NavigationPkg
{
    [SerializeField] public NavigationState state;
    [SerializeField] public NavigationUIPkg uiPkg;
}

/// <summary>
/// 题目导航脚本
/// </summary>
public class QuestionNavigatorItem : UIButton
{
    [Header("设置容器")]
    [SerializeField] private List<NavigationPkg> _navigationPkgs = new List<NavigationPkg>();

    [Header("UI控件")]
    [SerializeField] private Image _bgImg;
    [SerializeField] private TextMeshProUGUI _idxTx;

    // 服务组件
    [Inject] private IAssService _assMgr;

    public void Setup(string index)
    {
        _idxTx.text = index;
        CheckStatus();
    }

    /// <summary>
    /// 根据不同的状态，设置UI控件不同的颜色和图片
    /// </summary>
    /// <param name="state"></param>
    public void SetNavigationState(NavigationState state)
    {
        var pkg = _navigationPkgs.Find(_ => _.state == state);
        if (pkg != null)
        {
            _bgImg.sprite = pkg.uiPkg.sprite;
            _idxTx.color  = pkg.uiPkg.fontColor;
        }
    }

    /// <summary>
    /// 检查当前item是否需要切换状态
    /// </summary>
    public void CheckStatus()
    {
        int i = 0;
        if (int.TryParse(_idxTx.text, out i))
        {
            if (_assMgr.currIdx == i) SetNavigationState(NavigationState.High);
            else if (_assMgr.currIdx + 1 == i || _assMgr.currIdx - 1 == i) SetNavigationState(NavigationState.Moderate);
            else SetNavigationState(NavigationState.Low);
        }
    }
}