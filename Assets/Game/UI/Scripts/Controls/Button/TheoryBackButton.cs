using SUG.Essentials;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TheoryBackButton : UIButton
{
    private TheoryBackMode _currBackMode = TheoryBackMode.Normal;

    // 点击事件
    public event Action<TheoryBackMode> onSelected;

    // Inject
    [EInject] private IGameService _gameMgr;

    // Life cycle
    private void Start()
    {
        onClickEnter += () => onSelected?.Invoke(_gameMgr.currTheoryBackMode);
    }
}
