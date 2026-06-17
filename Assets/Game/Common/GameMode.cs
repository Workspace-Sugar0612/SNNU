using SUG_UnityCore;
using UnityEngine;

public sealed class GameMode : Singleton<GameMode, SingletonGlobal>
{
    // ——  Config variable ——
    [Header("Is it unlock parctic.")]
    public bool isPar = false;

    private GameGlobalSetting _gameSetting;

    // =================
    // Life cycle
    // =================
    private void Start()
    {
        ConfigInit();
    }

    // =================
    // Initialized
    // =================
    private void ConfigInit()
    {
        // Get game global setting config instance.
        _gameSetting = ConfigManager.Get().GetConfig<GameGlobalSetting>();

        // Config element initialization.
        isPar = _gameSetting.isPracticeMode;
    } 
}