using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// GUI 交互音效配置文件
[CreateAssetMenu(fileName = "UI_AudioConfig", menuName = "Game/UI/AudioConfig")]
public class UIInteractionSound : ScriptableObject
{
    [Header("normal Button inter music")]
    public AudioClip norBtnHover;
    public AudioClip norBtnSelect;

    [Header("scale Button inter music")]
    public AudioClip scaleBtnHover;
    public AudioClip scaleBtnSelect;
}
