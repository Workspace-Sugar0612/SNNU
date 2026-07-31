using SUG.Essentials;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FailOptionItem : UIButton
{
    [Header("UI控件")]
    [SerializeField] private TextMeshProUGUI _serNumber;
    [SerializeField] private Image _icon;

    public event Action<int> openThisQuestion;
    private int _index = 0;

    #region 声明周期

    private void Start()
    {
        onClickEnter += () => { openThisQuestion?.Invoke(_index); };
    }

    #endregion

    #region 初始化

    public void Setup(bool isWrong, int index, Sprite correctSprite, Sprite wrongSprite)
    {
        _icon.sprite = isWrong ? wrongSprite : correctSprite;
        _serNumber.text = (index + 1).ToString();
        _index = index;
    }

    #endregion
}
