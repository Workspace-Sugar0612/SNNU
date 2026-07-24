using DG.Tweening;
using SUG.Essentials;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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

    // 事件
    public event Action<int> onSwitchTopics;

    // 成员变量

    private int _idx; // 这个引导负责的题号
    private NavigationState _currState; // 当前状态
    [Inject] private IAssService _assMgr; // 考试管理器

    #region 初始化

    public void Setup(int index)
    {
        _idx = index;
        _idxTx.text = (index + 1).ToString();
        SetNaviStateAndRefresh(NavigationState.NotAnswered);

        // 上层业务事件绑定
        onClickEnter += OnClickedItem;
    }

    #endregion

    /// <summary>
    /// 根据不同的状态，设置UI控件不同的颜色和图片
    /// </summary>
    /// <param name="state"></param>
    private void SetNavigationState(NavigationState state)
    {
        _currState = state;
    }

    /// <summary>
    /// 更新UI
    /// </summary>
    public void RefreshUI()
    {
        var pkg = _navigationPkgs.Find(_ => _.state == _currState);
        if (pkg != null)
        {
            // 改变 UI 样式
            _bgImg.sprite = pkg.uiPkg.sprite;
            //_idxTx.color  = pkg.uiPkg.fontColor;
            _idxTx.DOColor(pkg.uiPkg.fontColor, 1.5f);
        }
    }

    /// <summary>
    /// 更新状态 + 更新UI
    /// </summary>
    /// <param name="state"></param>
    public void SetNaviStateAndRefresh(NavigationState state)
    {
        SetNavigationState(state);
        RefreshUI();
    }

    #region 事件
        
    /// <summary>
    /// 当点击了这个Item时触发
    /// </summary>
    private void OnClickedItem()
    {
        onSwitchTopics?.Invoke(_idx);

        // 不管之前什么状态，都要变成正在作答状态

        // SetNavigationState(NavigationState.Answering);
    }

    /// <summary>
    /// 当被放弃点击
    ///【通常是点击后，点击其他了其他item，所以当前item应该变成之前的状态】
    /// </summary>
    public void OnDeClickedItem()
    {
        bool isAnswer = false;
        if (_idx >= 0 && _idx < _assMgr.recordArr.Count())
        {
            var re = _assMgr.recordArr[_idx];
            if (re != null && re.selectContents != null)
            {
                // 表示已经作答
                // 如果该题目的selectContents为0，说明没有被记录过
                isAnswer = re.selectContents.Count > 0;
            }
        }

        // 更新状态
        NavigationState state = isAnswer ? NavigationState.AlreadyAnswered : NavigationState.NotAnswered;
        SetNavigationState(state);
        RefreshUI();
    }

    #endregion
}