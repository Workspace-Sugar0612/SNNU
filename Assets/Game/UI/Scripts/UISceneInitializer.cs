using System;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine;
using VContainer;
using SUG.Essentials;

public class UISceneInitializer : MonoBehaviour
{
    private MethodInfo _openUIMethod;

    [Inject]
    private IUIService _uiMgr;

    // =======================
    // Life cycle
    // =======================
    private void Awake()
    {
        _openUIMethod = typeof(IUIService).GetMethod("OpenUI", Type.EmptyTypes);
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene sc, LoadSceneMode mode)
    {
        if (ConfigManager.Get().HasConfig<SceneLocalConfig>())
        {
            SceneLocalConfig c = ConfigManager.Get().GetConfig<SceneLocalConfig>();
            foreach (var p in c.AutoOpenUITypes)
            {
                Type t = p.GetType();
                var genericOpen = _openUIMethod.MakeGenericMethod(t);
                genericOpen?.Invoke(_uiMgr, null);
            }
        }
    }
}