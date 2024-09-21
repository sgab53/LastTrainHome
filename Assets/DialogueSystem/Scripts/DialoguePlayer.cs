using System;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private GameStateData _gameState;
    [SerializeField] private TextDatabaseData _textDatabaseData;
    [SerializeField] private DialoguesDatabaseData _dialoguesDatabaseData;

    [Header("UI")]
    [SerializeField] private DialoguePanelController _dialoguePanel;

    public event Action OnDialogueEnded;

    private PlayState _previousState;
    private InputContext _previousContext;
    private string _currentDialogue;
    private int _index = -1;

    private void Awake()
    {
        OnDialogueEnded += ResetState;
    }

    private void ResetState()
    {
        _dialoguePanel.Hide();
        _gameState.UpdateGame(_previousState);
        _gameState.UpdateGame(_previousContext);
    }

    public void StartDialogue(string newDialogue)
    {
        Debug.Log("start dialog");
        _gameState.UpdateGame(InputContext.Dialogue);
        _gameState.UpdateGame(PlayState.Cutscene);
        _currentDialogue = newDialogue;
        _index = -1;
        Next();
    }

    public void Next()
    {
        Debug.Log("next");
        ++_index;
        var languageCode = _gameState.CurrentLanguage.Code;
        var lineKey = _dialoguesDatabaseData.Database.GetLineKey(_currentDialogue, _index);
        
        if (lineKey == null)
        {
            Debug.Log("null finito");
            OnDialogueEnded?.Invoke();
            return;
        }

        var line = _textDatabaseData.Database.GetText(lineKey, languageCode);

        _dialoguePanel.SetText(line);
        _dialoguePanel.Show();
    }
}
