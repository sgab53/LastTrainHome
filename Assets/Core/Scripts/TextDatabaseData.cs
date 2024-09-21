using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

[System.Serializable]
public class TextDatabase
{
    public Dictionary<string, Dictionary<string, string>> _data;

    public string GetText(string key, string languageCode)
    {
        if (!_data.ContainsKey(key))
            Debug.LogError($"Key \"{key}\" does not exist");
        
        if (!_data[key].ContainsKey(languageCode))
            Debug.LogError($"Key \"{languageCode}\" does not exist");
        
        return _data[key][languageCode];
    }
}

[CreateAssetMenu(fileName = "TextDatabase", menuName = "TextDatabase", order = 0)]
public class TextDatabaseData : ScriptableObject
{
    private TextDatabase _database;
    public TextDatabase Database => _database;

    public void InitFromJson(string file)
    {
        var textAsset = Resources.Load<TextAsset>(file);
        _database = JsonConvert.DeserializeObject<TextDatabase>(textAsset.text);
    }
}