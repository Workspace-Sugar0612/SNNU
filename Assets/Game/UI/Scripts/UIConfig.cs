using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// GUI 交互音效配置文件
[CreateAssetMenu(fileName = "UI_AudioConfig", menuName = "Game/UI/AudioConfig")]
public class UIInteractionSound : ScriptableObject
{
    [Header("Normal button inter music")]
    public AudioClip norBtnHover;
    public AudioClip norBtnSelect;

    [Header("Start button inter music")]
    public AudioClip startBtnHover;
    public AudioClip startBtnSelect;
}
