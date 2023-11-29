using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Text _moneyText;

    [SerializeField] private Image _progressBarGunsImage;
    [SerializeField] private Image _progressBarUpgradesImage;
    [SerializeField] private Image _progressBarShopImage;

    [SerializeField] private Text _progressBarGunsText;
    [SerializeField] private Text _progressBarUpgradesText;
    [SerializeField] private Text _progessBarUpgradesText;

    [SerializeField] private Text _levelText;

    [SerializeField] public int[,] Prices;

    [SerializeField] private Texture _yanTexure;
    [SerializeField] private RawImage _yanIcon;

    private void Start()
    {
        UpdateMoney();
        UpdateProgressBars();
        SetLevel();
        Prices = Utility.Utility.ReadCSVInt("Prices");
        SetYanTexture("https://yastatic.net/s3/games-static/static-data/images/payments/sdk/currency-icon-m.png");
    }

    public void UpdateMoney()
    {
        _moneyText.text = SaveManager.Instance.CurrentProgress.Money.ToString();
    }

    public void UpdateProgressBars()
    {
        UpdateGunProgressBar();
        UpdateGunUpgradeBar();
    }

    public void UpdateGunProgressBar()
    {
        float upgradeCount = 0;
        for (int i = 0; i < 8; i++)
        {
            upgradeCount += SaveManager.Instance.CurrentProgress.UpgradeLevels[i];
        }
        _progressBarGunsImage.fillAmount = upgradeCount / 40;
        _progressBarGunsText.text = $"{upgradeCount}/40";
    }

    public void UpdateGunUpgradeBar()
    {
        float upgradeCount = 0;
        for (int i = 8; i < 10; i++)
        {
            upgradeCount += SaveManager.Instance.CurrentProgress.UpgradeLevels[i];
        }
        _progressBarUpgradesImage.fillAmount = upgradeCount / 10;
        _progressBarUpgradesText.text = $"{upgradeCount}/10";
    }

    public void SetGunsProgessBarToShop()
    {
        _progressBarShopImage.fillAmount = _progressBarGunsImage.fillAmount;
        _progessBarUpgradesText.text = _progressBarGunsText.text;
    }

    public void SetUpgradeProgessBarToShop()
    {
        _progressBarShopImage.fillAmount = _progressBarUpgradesImage.fillAmount;
        _progessBarUpgradesText.text = _progressBarUpgradesText.text;
    }

    public void SetLevel()
    {
        _levelText.text = $"Level {SaveManager.Instance.CurrentProgress.Level + 1}";
    }

    public void SpendMoney(int moneyAmount)
    {
        SaveManager.Instance.CurrentProgress.Money -= moneyAmount;
        UpdateMoney();
    }

    public void SetYanTexture(string url)
    {
        StartCoroutine(DownloadYanImage(url));
        Debug.Log("Icons set");
    }

    public IEnumerator DownloadYanImage(string mediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            _yanTexure = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
        _yanIcon.texture = _yanTexure;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }
}
