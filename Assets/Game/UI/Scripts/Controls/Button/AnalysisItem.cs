using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SUG.Essentials;

public class AnalysisItem : UIButton
{
    [Header("UI控件")]
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _contentTx;
    public RectTransform analysisTxRect; // 分析题目Item内容的Rect

    #region 初始化

    public void Setup(string content, Sprite sprite, Color color)
    {
        _icon.sprite = sprite;
        _contentTx.text = content;
        _contentTx.color = color;
    }

    #endregion
}
