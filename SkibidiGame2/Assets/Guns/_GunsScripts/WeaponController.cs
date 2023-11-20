using Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject[] _gunObjects;
    [SerializeField] private Gun[] _gunScripts;
    [SerializeField] private Image[] _gunBackgroundIcons;
    [SerializeField] private GameObject[] _lockIcons;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Gun _currentGun;
    [SerializeField] private CharacterMovement _characterMovement;
    private int _currentGunIndex;

    private void SelectWeapon(InputAction.CallbackContext obj)
    {
        if (!_currentGun.CanSwitch) return;
        int ind = int.Parse(obj.action.name) - 1;
        if (_currentGun != _gunScripts[ind] && _gunScripts[ind].GunLevel > 0)
        {
            SelectGun(ind);
        }
    }

    private void ScrollWeapon(InputAction.CallbackContext obj)
    {
        if (!_currentGun.CanSwitch) return;
        if (obj.ReadValue<Vector2>().y < 0)
        {
            if (_currentGunIndex + 1 >= _gunScripts.Length) SelectGun(0);
            else SelectGun(_currentGunIndex + 1);
        }
        else
        {
            if (_currentGunIndex - 1 < 0) SelectGun(_gunScripts.Length - 1);
            else SelectGun(_currentGunIndex - 1);
        }
    }

    private void Update()
    {
        Walk();
    }
    public void Walk()
    {
        if (_characterMovement.MoveInput != default && _characterMovement.IsGrounded) _currentGun.WalkStart();
        else _currentGun.WalkEnd();
    }

    public void Awake()
    {
        int[] ints = new int[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
        SetGunProperties(ints);
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
        _currentGun = _gunObjects[index].GetComponent<Gun>();
        _currentGunIndex = index;
        _currentGun.PlayTakeAnimation();
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
        for (int i = 0; i < _gunScripts.Length; i++)
        {
            _gunScripts[i].GunLevel = gunLevel[i];
            if (_gunScripts[i].GunLevel > 0) _lockIcons[i].SetActive(false);
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
