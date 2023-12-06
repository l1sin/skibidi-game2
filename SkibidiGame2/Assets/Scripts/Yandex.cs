using System.Runtime.InteropServices;
using UnityEngine;

public class Yandex : MonoBehaviour
{
    public static Yandex Instance;
    public const string Path = "idbfs/MonsterShooterSaveDirectory";
    public Localization DefaultLanguage;
    public enum Localization
    {
        en,
        ru,
        tr,
        es,
        pt,
        id,
        fr,
        it,
        de
    }

    [DllImport("__Internal")]
    public static extern string GetLanguage();

    [DllImport("__Internal")]
    public static extern void Rate();

    [DllImport("__Internal")]
    public static extern void WatchAdAdd();

    [DllImport("__Internal")]
    public static extern void WatchAdDouble();

    [DllImport("__Internal")]
    public static extern void SaveExtern(string data);

    [DllImport("__Internal")]
    public static extern void LoadExtern();

    [DllImport("__Internal")]
    public static extern void FullScreenAd();

    [DllImport("__Internal")]
    public static extern void CallRate();

    [DllImport("__Internal")]
    public static extern void CallPurchaseMenu(string id, string name);

    [DllImport("__Internal")]
    public static extern string GetPrice(int index);

    [DllImport("__Internal")]
    public static extern void GetYanIcon();

    [DllImport("__Internal")]
    public static extern void GameReady();

    [DllImport("__Internal")]
    public static extern void ConsumePurchase(string purchaseToken);

    [DllImport("__Internal")]
    public static extern void CheckPurchases();

    [DllImport("__Internal")]
    public static extern void ReachGoal(string goal);

    [DllImport("__Internal")]
    public static extern bool CheckDevice();

    private void Awake()
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
#if UNITY_EDITOR
        EditorInit();
#endif
    }

    public void StartInit()
    {
        string lang = GetLanguage();
        SaveManager.Instance.LoadLanguage(lang);
        SaveManager.Instance.IsMobile = CheckDevice();
        LoadExtern();
    }

    public void EditorInit()
    {
        SaveManager.Instance.LoadLanguage(DefaultLanguage.ToString());
        SaveManager.Instance.LoadDataLocal();
    }
}
