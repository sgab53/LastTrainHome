using UnityEngine;

public enum PlayState
{
    Unset       = -1,
    Playing     = 0,
    Pause       = 1,
    Cutscene    = 2
}

public enum InputContext
{
    UI,
    Game,
    Dialogue,
}

[CreateAssetMenu(fileName = "GameStateData", menuName = "GameStateData", order = 0)]
public class GameStateData : ScriptableObject
{
    private GameState _gameState = new();

    public PlayState PlayState => _gameState.State;
    public LanguageData CurrentLanguage => _gameState.CurrentLanguage;
    public InputContext InputContext => _gameState.InputContext;

    public void UpdateGame(PlayState newState) => _gameState.State = newState;
    public void UpdateGame(LanguageData newLanguage) => _gameState.CurrentLanguage = newLanguage;
    public void UpdateGame(InputContext newContext) => _gameState.InputContext = newContext;

    public void UpdateGame(GameState gameState)
    {
        _gameState = gameState;
    }
}

[System.Serializable]
public struct GameState
{
    public PlayState State;
    public LanguageData CurrentLanguage;
    public InputContext InputContext;
}