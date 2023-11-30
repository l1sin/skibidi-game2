using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public Progress CurrentProgress;
    public string[,] Dictionary;
    public string[] Localization;
    public bool IsMobile;
    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void LoadDataLocal()
    {
        Progress progress;
        if (PlayerPrefs.HasKey("save"))
        {
            string json = PlayerPrefs.GetString("save");
            progress = JsonUtility.FromJson<Progress>(json);
            CurrentProgress = progress;
            Debug.Log($"Loaded from PlayerPrefs:\n{json}");
        }
        else
        {
            progress = new Progress();
            SaveData(progress);
            Debug.Log("File do not exists. Creating save file");
        }
        SceneManager.LoadScene(12);
    }

    public void LoadDataCloud(string json)
    {
        Progress progress;
        if (json != null)
        {
            progress = JsonUtility.FromJson<Progress>(json);
            CurrentProgress = progress;
            Debug.Log($"Loaded from Cloud\n{json}");
        }
        else
        {
            progress = new Progress();
            SaveData(progress);
            Debug.Log("File do not exists. Creating save file");
        }
        SceneManager.LoadScene(12);
    }

    public void SaveData(Progress progress)
    {
        SaveDataLocal(progress);
#if UNITY_EDITOR
        Debug.Log("CloudSave");
#elif UNITY_WEBGL
        SaveDataCloud(progress);
#endif
    }

    public void SaveDataLocal(Progress progress)
    {
        CurrentProgress = progress;
        string json = JsonUtility.ToJson(progress);
        PlayerPrefs.SetString("save", json);
        PlayerPrefs.Save();
        Debug.Log($"Local save to PlayerPrefs");
    }

    public void SaveDataCloud(Progress progress)
    {
        CurrentProgress = progress;
        string json = JsonUtility.ToJson(progress);
        Yandex.SaveExtern(json);
        Debug.Log($"Cloud save");
    }

    public void LoadLanguage(string language)
    {
        Dictionary = Utility.Utility.ReadCSVString("Localization");

        int id = GetLanguageId(language);
        Localization = new string[Dictionary.GetLength(0) - 1];

        for (int i = 1; i < Dictionary.GetLength(0); i++)
        {
            Localization[i - 1] = Dictionary[i, id];
        }
    }

    public int GetLanguageId(string language)
    {
        for (int j = 0; j < Dictionary.GetLength(1); j++)
        {
            if (Dictionary[0, j] == language)
            {
                return j;
            }
        }
        Debug.Log("Unknown language - switch to en");
        return GetLanguageId("en");
    }
}
