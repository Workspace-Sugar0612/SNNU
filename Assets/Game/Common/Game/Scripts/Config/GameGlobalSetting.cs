using SUG.Essentials;
using UnityEditor.Search.Providers;
using UnityEngine;

[CreateAssetMenu(fileName = "GameGlobalSetting", menuName = "Game/GameGlobalSetting")]
public class GameGlobalSetting : ScriptableObject
{
    [Header("是否解锁了实训模式")]
    public bool isPracticeMode = false;

    [Header("场景实例")]
    [Scene] public string parcitcScene;
    [Scene] public string theoryScene;
    [Scene] public string startScene;
}