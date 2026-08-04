using SUG.Essentials;
using UnityEditor.Search.Providers;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "GameGlobalSetting", menuName = "Game/GameGlobalSetting")]
public class GameGlobalSettingSO : ScriptableObject
{
    [Header("是否解锁了实训模式")]
    public bool isPracticeMode = false;

    [Header("场景实例")]
    public AssetReference parcitcScene;
    public AssetReference theoryScene;
    public AssetReference startScene;
}