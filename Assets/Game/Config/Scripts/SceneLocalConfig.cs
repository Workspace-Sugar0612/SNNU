using System.Collections.Generic;
using SUG_UnityCore;
using UnityEngine;

public class SceneLocalConfig : ScriptableObject
{
    public List<UIBase> AutoOpenUITypes = new List<UIBase>();
}

[CreateAssetMenu(fileName = "Select_Scene_Config", menuName = "Game/Scenes/Select_Scene_Config")]
public class SelectSceneConfig : SceneLocalConfig
{

}