using UnityEngine;

[CreateAssetMenu(fileName = "GameGlobalSetting", menuName = "Game/GameGlobalSetting")]
public class GameGlobalSetting : ScriptableObject
{
    [Header("是否解锁了实训模式")]
    public bool isPracticeMode = false;
}