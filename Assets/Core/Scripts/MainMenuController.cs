using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameStateData _gameState;
    [SerializeField] private Image _mainMenuPanel;

    [SerializeField] private Image _instructionsPanel;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _instructionsButton;
    [SerializeField] private Button _instructionsBackButton;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _playButton.onClick.AddListener(PlayGame);
        _instructionsButton.onClick.AddListener(ShowInstructionsPanel);
        _instructionsBackButton.onClick.AddListener(HideInstructionsPanel);
        _quitButton.onClick.AddListener(QuitGame);
    }

    private void PlayGame()
    {
        _gameState.UpdateGame(InputContext.Game);
        _gameState.UpdateGame(PlayState.Playing);

        _mainMenuPanel.gameObject.SetActive(false);
    }

    private void ShowInstructionsPanel()
    {
        _instructionsPanel.gameObject.SetActive(true);
    }

    private void HideInstructionsPanel()
    {
        _instructionsPanel.gameObject.SetActive(false);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
