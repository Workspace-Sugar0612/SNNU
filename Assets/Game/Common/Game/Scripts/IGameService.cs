using SUG.Essentials;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Injectable] public interface IGameService
{
    public bool isPar { set; get; }
    public GameType currGameMode { set; get; }
    public TheoryBackMode currTheoryBackMode { set; get; }
    public GameGlobalSettingSO gameSetting { set; get; }
}
