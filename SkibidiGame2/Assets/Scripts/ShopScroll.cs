using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScroll : MonoBehaviour
{
    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private List<GameObject> _gameObjects;
    [SerializeField] private Text _nameText;
    [SerializeField] private int[] _nameLocaliztionIndices;
    [SerializeField] private List<string> _names;
    [SerializeField] private List<int> _indices;
    [SerializeField] private int _currentItemIndex;
    [SerializeField] private int _currentItemPrice;

    [SerializeField] private Button _buyOneButton;
    [SerializeField] private Button _buyFullButton;

    [SerializeField] private Text _buyOneButtonText;
    [SerializeField] private Text _buyFullButtonText;

    [SerializeField] private List<GameObject> _lockStars;

    [SerializeField] ShopType _shopType;

    private enum ShopType
    {
        Guns,
        Upgrades
    }

    private void Start()
    {
        for (int i = 0; i < _nameLocaliztionIndices.Length; i++)
        {
            _names[i] = SaveManager.Instance.Localization[_nameLocaliztionIndices[i]];
        }
        SetCurrentItem();
    }


    public void ScrollLeft()
    {
        _gameObjects[_currentItemIndex].SetActive(false);
        if (_currentItemIndex - 1 >= 0) _gameObjects[--_currentItemIndex].SetActive(true);
        else _currentItemIndex = _gameObjects.Count - 1;
        SetCurrentItem();
    }

    public void ScrollRight()
    {
        _gameObjects[_currentItemIndex].SetActive(false);
        if (_currentItemIndex + 1 < _gameObjects.Count) _gameObjects[++_currentItemIndex].SetActive(true);
        else _currentItemIndex = 0;
        SetCurrentItem();
    }

    public void SetCurrentItem()
    {
        SetItem();

        if (SaveManager.Instance.CurrentProgress.UpgradeLevels[_indices[_currentItemIndex]] <= 4)
        {
            EnableButton();
        }
        else
        {
            DisableButtons();
        }
    }

    public void BuyOneItem()
    {
        SaveManager.Instance.CurrentProgress.UpgradeLevels[_indices[_currentItemIndex]]++;
        _mainMenuController.SpendMoney(_currentItemPrice);
        _mainMenuController.UpdateProgressBars();
        if (_shopType == ShopType.Guns) _mainMenuController.SetGunsProgessBarToShop();
        else if (_shopType == ShopType.Upgrades) _mainMenuController.SetUpgradeProgessBarToShop();
        SetCurrentItem();
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
    }

    public void CallPurchaseMenu()
    {
        Yandex.CallPurchaseMenu(_mainMenuController.ProductIDs[_indices[_currentItemIndex]], gameObject.name);
    }

    public void BuyFullItem(string token)
    {
        SaveManager.Instance.CurrentProgress.UpgradeLevels[_indices[_currentItemIndex]] = 5;
        _mainMenuController.UpdateProgressBars();
        if (_shopType == ShopType.Guns) _mainMenuController.SetGunsProgessBarToShop();
        else if (_shopType == ShopType.Upgrades) _mainMenuController.SetUpgradeProgessBarToShop();
        SetCurrentItem();
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
#if UNITY_EDITOR

#elif UNITY_WEBGL
        //Debug.Log(token);
        Yandex.ConsumePurchase(token);
#endif
    }

    public void EnableButton()
    {
        _currentItemPrice = _mainMenuController.Prices[_indices[_currentItemIndex], SaveManager.Instance.CurrentProgress.UpgradeLevels[_indices[_currentItemIndex]]];

        _buyOneButtonText.text = _currentItemPrice.ToString();
        _buyOneButton.onClick.RemoveAllListeners();
        _buyOneButton.onClick.AddListener(BuyOneItem);
        if (_currentItemPrice > SaveManager.Instance.CurrentProgress.Money) _buyOneButton.interactable = false;
        else _buyOneButton.interactable = true;

        _buyFullButtonText.text = _mainMenuController.YanPrices[_indices[_currentItemIndex]];
        _buyFullButton.onClick.RemoveAllListeners();
        _buyFullButton.onClick.AddListener(CallPurchaseMenu);
        _buyFullButton.interactable = true;
    }

    public void DisableButtons()
    {
        _buyOneButtonText.text = SaveManager.Instance.Localization[8];
        _buyOneButton.onClick.RemoveAllListeners();
        _buyOneButton.interactable = false;

        _buyFullButtonText.text = SaveManager.Instance.Localization[8];
        _buyFullButton.onClick.RemoveAllListeners();
        _buyFullButton.interactable = false;
    }

    public void SetItem()
    {
        _nameText.text = _names[_currentItemIndex];
        _gameObjects[_currentItemIndex].SetActive(true);

        foreach (GameObject ls in _lockStars)
        {
            ls.SetActive(true);
        }

        int count = SaveManager.Instance.CurrentProgress.UpgradeLevels[_indices[_currentItemIndex]];
        for (int i = 0; i < count; i++)
        {
            _lockStars[i].SetActive(false);
        }
    }
}
