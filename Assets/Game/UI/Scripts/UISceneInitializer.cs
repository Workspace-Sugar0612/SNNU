using System;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine;
using SUG.Essentials;

public class UISceneInitializer : MonoBehaviour
{
    private MethodInfo _openUIMethod;

    // Inject
    [Inject] private ICfgService _cfgMgr;
    [Inject] private IUIService _uiMgr;
    [Inject] private ISceneService _sceneMgr;

    // =======================
    // Life cycle
    // =======================
    private void Awake()
    {
        _openUIMethod = typeof(IUIService).GetMethod("OpenUI", Type.EmptyTypes);
        _sceneMgr.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene sc, LoadSceneMode mode)
    {
        if (_cfgMgr.HasConfig<SceneLocalConfig>())
        {
            SceneLocalConfig c = _cfgMgr.GetConfig<SceneLocalConfig>();

            foreach (var p in c.AutoOpenUITypes)
            {
                Type t = p.GetType();
                var genericOpen = _openUIMethod.MakeGenericMethod(t);
                genericOpen?.Invoke(_uiMgr, null);
            }
        }
    }

    private void OnDestroy()
    {
        _sceneMgr.sceneLoaded -= OnSceneLoad;
    }
}