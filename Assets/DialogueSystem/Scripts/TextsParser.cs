using UnityEngine;

public class TextsParser : MonoBehaviour
{
    [SerializeField] private TextDatabaseData _textDatabaseData;
    [SerializeField] private DialoguesDatabaseData _dialoguesDatabaseData;

    private void Awake()
    {
        _textDatabaseData.InitFromJson("texts");
        _dialoguesDatabaseData.InitFromJson("dialogues");
    }
}
