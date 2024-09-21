using UnityEngine;

public enum GameState
{
    Unset       = -1,
    Playing     = 0,
    Pause       = 1,
    Cutscene    = 2
}

[CreateAssetMenu(fileName = "GameStateData", menuName = "GameStateData", order = 0)]
public class GameStateData : ScriptableObject
{
    [SerializeField] private GameState _gameState;
    [SerializeField] private LanguageData _currentLanguage;

}
