using SUG.Essentials;
using UnityEngine;

public enum GameType 
{
     None,
     Start, 
     Parctice, 
     Theory
}

[Service(ServiceLifetime.Global)]
public sealed class GameMode : MonoBehaviour, IGameService
{
    // ——  Config variable ——
    //[Header("Is it unlock parctic.")]
    public bool isPar { set; get; }

    //[Header("Current game mode.")]
    public GameType currGameMode { set; get; }
    public TheoryBackMode currTheoryBackMode { set; get; }

    // Game setting config.
    [SerializeField] private GameGlobalSettingSO _gameSetting;
    public GameGlobalSettingSO gameSetting { set => _gameSetting = value; get => _gameSetting; }

    // =================
    // Life cycle
    // =================

    private void Start()
    {
        Initialztion();
    }

    // =================
    // Initialized
    // =================
    private void Initialztion()
    {
        currGameMode = GameType.None;
        currTheoryBackMode = TheoryBackMode.Normal;
        isPar = gameSetting.isPracticeMode;
    } 
}