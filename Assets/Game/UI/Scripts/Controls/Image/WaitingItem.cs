using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public class WaitingInfo
{
    [SerializeField] public Sprite icon;
    [SerializeField] public float height;
}

/// <summary>
/// 加载界面的动态Image
/// </summary>
public class WaitingItem : MonoBehaviour
{
    [SerializeField] private List<WaitingInfo> _infos = new ();

    [Range(0, 9), SerializeField] 
    private int _startIdx = 0;

    private Image _img;

    private bool _isLooper = true;

    private void Start()
    {
        _img = GetComponent<Image>();
        _img.sprite = _infos[_startIdx].icon;
        var rect = _img.transform as RectTransform;
        rect.DOSizeDelta(new Vector2(rect.sizeDelta.x, _infos[_startIdx].height), 0.0f);
        //if (_img != null)
        //{
        //    StartCoroutine(Looper());
        //}
    }

    private IEnumerator Looper()
    {
        int symbol = 1;
        while (_isLooper)
        {
            var rect = _img.transform as RectTransform;
            rect.DOSizeDelta(new Vector2(rect.sizeDelta.x, _infos[_startIdx].height), 0.05f);
            _img.sprite = _infos[_startIdx].icon;

            yield return new WaitForSeconds(0.05f);

            int buffer = _startIdx + 1 * symbol;
            symbol = buffer < 0 || buffer >= _infos.Count ? symbol * -1 : symbol;
            _startIdx = _startIdx + 1 * symbol;
        }
    }

    private void OnDestroy()
    {
        _isLooper = false;
    }
}
