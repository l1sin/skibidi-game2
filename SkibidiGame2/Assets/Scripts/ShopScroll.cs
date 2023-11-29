using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScroll : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gameObjects;
    [SerializeField] private Text _nameText;
    [SerializeField] private List<string> _names;
    [SerializeField] private int _currentItemIndex;

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
        _nameText.text = _names[_currentItemIndex];
        _gameObjects[_currentItemIndex].SetActive(true);
    }
}
