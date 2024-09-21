using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    private DialogueData _currentDialogue;
    private int _index = -1;

    public void StartDialogue(DialogueData newDialogue)
    {
        _currentDialogue = newDialogue;
        Next();
    }

    public void Next()
    {
        // play next dialogue line
        var key = _currentDialogue.Keys[++_index];
    }
}
