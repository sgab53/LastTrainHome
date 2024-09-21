using System.Collections.Generic;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private GameStateData _gameState;
    [SerializeField] private TextDatabaseData _textDatabaseData;
    private DialoguesDatabase _dialoguesDatabase;
    private DialogueData _currentDialogue;
    private int _index = -1;

    private void Awake()
    {
        var textAsset = Resources.Load<TextAsset>("dialogues");
        _dialoguesDatabase = JsonUtility.FromJson<DialoguesDatabase>(textAsset.text);
    }

    public void StartDialogue(DialogueData newDialogue)
    {
        _currentDialogue = newDialogue;
        Next();
    }

    public void Next()
    {
        // play next dialogue line
        var key = _currentDialogue.Keys[++_index];
        var languageCode = _gameState.CurrentLanguage.Code;
        _dialoguesDatabase.GetLine(_textDatabaseData.Database, key, languageCode);
    }

    private class DialoguesDatabase
    {
        // Key: dialogue key
        // Value: text key
        private readonly Dictionary<string, string> _dialoguesTexts;

        public string GetLine(TextDatabase database, string key, string languageCode)
        {
            return database.GetText(_dialoguesTexts[key], languageCode);
        }
    }
}
