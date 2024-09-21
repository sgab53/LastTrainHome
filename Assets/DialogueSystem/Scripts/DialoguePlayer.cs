using System;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private GameStateData _gameState;
    [SerializeField] private TextDatabaseData _textDatabaseData;
    [SerializeField] private DialoguesDatabaseData _dialoguesDatabaseData;

    public event Action OnDialogueEnded;

    private string _currentDialogue;
    private int _index = -1;

    public void StartDialogue(string newDialogue)
    {
        _currentDialogue = newDialogue;
        Next();
    }

    public void Next()
    {
        // play next dialogue line
        ++_index;

        var languageCode = _gameState.CurrentLanguage.Code;
        var lineKey = _dialoguesDatabaseData.Database.GetLineKey(_currentDialogue, _index);
        
        if (lineKey == null)
        {
            OnDialogueEnded?.Invoke();
            return;
        }

        var line = _textDatabaseData.Database.GetText(lineKey, languageCode);
    }
}
