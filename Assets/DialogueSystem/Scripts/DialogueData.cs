using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "DialogueData", order = 0)]
public class DialogueData : ScriptableObject
{
    [SerializeField] private string[] _keys;
    public string[] Keys => _keys;
}