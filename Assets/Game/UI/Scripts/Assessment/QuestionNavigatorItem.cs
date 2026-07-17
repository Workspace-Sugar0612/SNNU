using DG.Tweening;
using SUG.Essentials;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum NavigationState
{
    AlreadyAnswered, // 已经作答
    NotAnswered,  // 未作答
    Answering // 正在作答
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

    // 初始状态/未被点击的状态
    public NavigationState initialState { get; private set; } = NavigationState.NotAnswered;

    // 事件
    public event Action<int> onSwitchTopics;

    #region 初始化

    public void Setup(string index)
    {
        _idxTx.text = index;
        SetNavigationState(NavigationState.NotAnswered);

        // 上层业务事件绑定
        onClickEnter += OnClickedItem;
    }

    #endregion

    /// <summary>
    /// 根据不同的状态，设置UI控件不同的颜色和图片
    /// </summary>
    /// <param name="state"></param>
    public void SetNavigationState(NavigationState state)
    {
        var pkg = _navigationPkgs.Find(_ => _.state == state);
        if (pkg != null)
        {
            // 如果不是已经作答状态，那么可以修改他的状态
            if (state == NavigationState.AlreadyAnswered 
                || state == NavigationState.NotAnswered)
                initialState = state;

            // 改变 UI 样式
            _bgImg.sprite = pkg.uiPkg.sprite;
            //_idxTx.color  = pkg.uiPkg.fontColor;
            _idxTx.DOColor(pkg.uiPkg.fontColor, 1.5f);
        }
    }

    #region 事件
        
    /// <summary>
    /// 当点击了这个Item时触发
    /// </summary>
    private void OnClickedItem()
    {
        int idx = 0;
        if (int.TryParse(_idxTx.text, out idx))
        {
            onSwitchTopics?.Invoke(idx);

            // 不管之前什么状态，都要变成正在作答状态

            SetNavigationState(NavigationState.Answering);
        }
    }

    /// <summary>
    /// 当被放弃点击
    ///【通常是点击后，点击其他了其他item，所以当前item应该变成之前的状态】
    /// </summary>
    public void OnDeClickedItem()
    {
        SetNavigationState(initialState);
    }

    #endregion
}