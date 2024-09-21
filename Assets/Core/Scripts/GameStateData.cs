using UnityEngine;

public enum GameState
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
    [SerializeField] private GameState _gameState;
    [SerializeField] private LanguageData _currentLanguage;
    [SerializeField] private InputContext _inputContext;

    public GameState GameState => _gameState;
    public LanguageData CurrentLanguage => _currentLanguage;
    public InputContext InputContext => _inputContext;

    public void ChangeState(GameState newState) => _gameState = newState;
    public void ChangeLanguage(LanguageData newLanguage) => _currentLanguage = newLanguage;
    public void ChangeInputContext(InputContext newContext) => _inputContext = newContext;
}
