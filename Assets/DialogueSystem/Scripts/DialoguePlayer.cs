using System;
using Cysharp.Threading.Tasks;
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
        _dialoguePanel.Cancel();
        _gameState.UpdateGame(_previousState);
        _gameState.UpdateGame(_previousContext);
    }

    public void StartDialogue(string newDialogue)
    {
        _gameState.UpdateGame(InputContext.Dialogue);
        _gameState.UpdateGame(PlayState.Cutscene);
        _currentDialogue = newDialogue;
        _index = -1;
        Next();
    }


private bool _busy = false;
    public void Next()
    {
        if (_busy)
            return;
        WaitForNextLine().Forget();
    }

    private async UniTaskVoid WaitForNextLine()
    {
        _busy = true;
        var completed = await _dialoguePanel.Complete();
        if (completed)
        {
            _busy = false;
            return;
        }

        ++_index;
        var languageCode = _gameState.CurrentLanguage.Code;
        var lineKey = _dialoguesDatabaseData.Database.GetLineKey(_currentDialogue, _index);
        
        if (lineKey == null)
        {
            OnDialogueEnded?.Invoke();
            _busy = false;
            return;
        }

        var line = _textDatabaseData.Database.GetText(lineKey, languageCode);

        _dialoguePanel.SetText(line);
        _dialoguePanel.Show();
        _busy = false;
    }
}
