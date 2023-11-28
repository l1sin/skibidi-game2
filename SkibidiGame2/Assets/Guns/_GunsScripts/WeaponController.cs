using Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject[] _gunObjects;
    [SerializeField] private Image[] _gunBackgroundIcons;
    [SerializeField] private GameObject[] _lockIcons;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Gun[] _currentGuns;
    [SerializeField] private CharacterMovement _characterMovement;
    private int _currentGunIndex;

    [SerializeField] private int[] _gunLevels;

    private void SelectWeapon(InputAction.CallbackContext obj)
    {
        if (!_currentGuns[0].CanSwitch) return;
        int ind = int.Parse(obj.action.name) - 1;
        if (_currentGunIndex != ind && _gunLevels[ind] > 0)
        {
            SelectGun(ind);
        }
    }

    private void ScrollWeapon(InputAction.CallbackContext obj)
    {
        if (!_currentGuns[0].CanSwitch) return;
        if (obj.ReadValue<Vector2>().y < 0)
        {
            if (_currentGunIndex + 1 >= _gunObjects.Length) SelectGun(0);
            else SelectGun(_currentGunIndex + 1);
        }
        else
        {
            if (_currentGunIndex - 1 < 0) SelectGun(_gunObjects.Length - 1);
            else SelectGun(_currentGunIndex - 1);
        }
    }

    private void Update()
    {
        Walk();
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

    public void Awake()
    {
        SetGunProperties(_gunLevels);
        SelectGun(0);
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
            if (_gunLevels[i] > 0) _lockIcons[i].SetActive(false);
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
