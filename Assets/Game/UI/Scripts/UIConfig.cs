using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// GUI 交互音效配置文件
[CreateAssetMenu(fileName = "UI_AudioConfig", menuName = "Game/UI/AudioConfig")]
public class UIInteractionSound : ScriptableObject
{
    [Header("普通按钮交互的Sound")]
    public AudioClip norBtnHover;
    public AudioClip norBtnSelect;
}
