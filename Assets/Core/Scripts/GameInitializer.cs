using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameStateData _gameStateData;
    [SerializeField] private GameState _initialGameState;

    private void Awake()
    {
        _gameStateData.UpdateGame(_initialGameState);
    }
}
