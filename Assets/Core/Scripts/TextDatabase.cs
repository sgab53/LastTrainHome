using System.Collections.Generic;
using UnityEngine;

public class TextData
{
    private readonly Dictionary<string, string> _text;

    public string GetText(string languageCode) => _text[languageCode];
}

public class TextDatabase
{
    private readonly Dictionary<string, TextData> _data;

    public TextData Get(string key) => _data[key];
    public string GetText(string key, string languageCode) => _data[key].GetText(languageCode);
}

[CreateAssetMenu(fileName = "TextDatabase", menuName = "TextDatabase", order = 0)]
public class TextDatabaseData : ScriptableObject
{
    private TextDatabase _database = new();
    public TextDatabase Database => _database;
}