using UnityEngine;

[CreateAssetMenu(fileName = "LanguageData", menuName = "LanguageData", order = 0)]
public class LanguageData : ScriptableObject
{
    [SerializeField] private Texture2D _languageSprite;
    [SerializeField] private string _languageLabel;
    [SerializeField] private string _languageCode;
    [SerializeField] private int _languageIndex;
}