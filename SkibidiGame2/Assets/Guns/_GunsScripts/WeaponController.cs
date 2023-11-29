using Input;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject[] _gunObjects;
    [SerializeField] private GameObject[] _secondGuns;
    [SerializeField] private List<int> _avaliableGuns = new List<int>();
    [SerializeField] private Image[] _gunBackgroundIcons;
    [SerializeField] private GameObject[] _lockIcons;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Gun[] _currentGuns;
    [SerializeField] private CharacterMovement _characterMovement;
    private int _currentGunIndex;

    [SerializeField] private int[] _gunLevels;

    public void Awake()
    {
        _gunLevels = SaveManager.Instance.CurrentProgress.UpgradeLevels;
        SetGunProperties(_gunLevels);
        SelectGun(0);
    }

    private void Update()
    {
        Walk();
    }

    private void SelectWeapon(InputAction.CallbackContext obj)
    {
        if (!_currentGuns[0].CanSwitch) return;
        int ind = int.Parse(obj.action.name) - 1;
        if (_currentGunIndex != ind && _avaliableGuns.Contains(ind))
        {
            SelectGun(ind);
        }
    }

    private void ScrollWeapon(InputAction.CallbackContext obj)
    {
        if (!_currentGuns[0].CanSwitch || _avaliableGuns.Count <= 1) return;
        if (obj.ReadValue<Vector2>().y < 0)
        {
            if (_currentGunIndex >= _avaliableGuns[_avaliableGuns.Count - 1]) SelectGun(0);
            else SelectGun(_avaliableGuns[_avaliableGuns.IndexOf(_currentGunIndex) + 1]);
        }
        else
        {
            if (_currentGunIndex - 1 < 0) SelectGun(_avaliableGuns[_avaliableGuns.Count - 1]);
            else SelectGun(_avaliableGuns[_avaliableGuns.IndexOf(_currentGunIndex) - 1]);
        }
    }

    public void Walk()
    {
        if (_characterMovement.MoveInput != default && _characterMovement.IsGrounded)
        {
            foreach (Gun gun in _currentGuns)
            {
                gun.WalkStart();
            }
        }
        else
        {
            foreach (Gun gun in _currentGuns)
            {
                gun.WalkEnd();
            }
        } 
    } 

    private void SelectGun(int index)
    {
        ChangeGun(index);
        ChangeIcon(index);
    }

    public void ChangeGun(int index)
    {
        foreach (GameObject w in _gunObjects)
        {
            w.SetActive(false);
        }
        _gunObjects[index].SetActive(true);
        _currentGunIndex = index;

        _currentGuns = _gunObjects[index].GetComponentsInChildren<Gun>();

        foreach (Gun gun in _currentGuns)
        {
            gun.PlayTakeAnimation();
        }
    }

    public void ChangeIcon(int index)
    {
        foreach (Image i in _gunBackgroundIcons)
        {
            i.color = _defaultColor;
        }
        _gunBackgroundIcons[index].color = _selectedColor;
    }

    public void SetGunProperties(int[] gunLevel)
    {
        for (int i = 0; i < _gunObjects.Length; i++)
        {
            if (gunLevel[i] > 0)
            {
                _lockIcons[i].SetActive(false);
                _avaliableGuns.Add(i);
                var guns = _gunObjects[i].GetComponentsInChildren<Gun>(true);
                foreach (Gun g in guns)
                {
                    g.BuffGun(SaveManager.Instance.CurrentProgress.UpgradeLevels[i]);
                }
            } 
            if (gunLevel[i] >= 5) _secondGuns[i].SetActive(true);
        }
    }
    private void OnEnable()
    {
        foreach (InputAction ia in InputManager.ChangeWeaponInputAction)
        {
            ia.performed += SelectWeapon;
        }
        InputManager.ScrollWeaponInputAction.performed += ScrollWeapon;
    }

    private void OnDisable()
    {
        foreach (InputAction ia in InputManager.ChangeWeaponInputAction)
        {
            ia.performed -= SelectWeapon;
        }
        InputManager.ScrollWeaponInputAction.performed -= ScrollWeapon;
    }
}
