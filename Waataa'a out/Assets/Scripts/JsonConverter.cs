using System.IO;
using UnityEngine;

public class JsonConverter : MonoBehaviour
{
    public static JsonConverter Instance;
    public PlayerInfo PlayerInfo;

    private string _path;
    private string _jsonInfo;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Instance == null) { Instance = this; }
        else if (Instance != this) { Destroy(this.gameObject); }

        _path = Path.Combine(Application.persistentDataPath, "DatosGuardados.json");

        if(File.Exists(_path)) { ReadJson(); }
        else { CreateJson(); }
    }

    private void CreateJson()
    {
        PlayerInfo = new PlayerInfo();
        _jsonInfo = JsonUtility.ToJson(PlayerInfo, true);
        File.WriteAllText(_path, _jsonInfo);
    }

    private void ReadJson()
    {
        _jsonInfo = File.ReadAllText(_path);
        PlayerInfo = JsonUtility.FromJson<PlayerInfo>(_jsonInfo);
    }

    public void SaveJson()
    {
        _jsonInfo = JsonUtility.ToJson(PlayerInfo, true);
        File.WriteAllText(_path, _jsonInfo);
    }
}
