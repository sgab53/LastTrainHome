using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Image _gameOverPanel;
    [SerializeField] private GameStateData _gameState;
    [SerializeField] private ScreenFadeController _screenFade;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;

    private void Awake()
    {
        _restartButton.onClick.AddListener(Restart);
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void Restart()
    {
        
    }

    public void GoToMainMenu()
    {
        ReloadGame().Forget();
    }

    public async UniTaskVoid ReloadGame()
    {
        await _screenFade.FadeOut();
        SceneManager.LoadScene("MainScene");
        _gameState.UpdateGame(InputContext.UI);   
        _gameState.UpdateGame(PlayState.Playing);
        await _screenFade.FadeIn();
    }
}
