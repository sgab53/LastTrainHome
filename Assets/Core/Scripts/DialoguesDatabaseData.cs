using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Data.Common;
using System.Linq;

[System.Serializable]
public class DialoguesDatabase
{
    // Key: dialogue key
    // Value: line texts keys
    public Dictionary<string, string[]> _data;

    public string GetLineKey(string key, int index)
    {
        if (!_data.ContainsKey(key))
            Debug.LogError($"Key \"{key}\" does not exist.");

        var lines = _data[key];

        if (index >= lines.Length)
            return null;

        return lines[index];
    }
}

[CreateAssetMenu(fileName = "DialoguesDatabase", menuName = "DialoguesDatabase", order = 0)]
public class DialoguesDatabaseData : ScriptableObject
{
    private DialoguesDatabase _database;
    public DialoguesDatabase Database => _database;

    public void InitFromJson(string file)
    {
        var textAsset = Resources.Load<TextAsset>(file);
        _database = JsonConvert.DeserializeObject<DialoguesDatabase>(textAsset.text);
    }
}