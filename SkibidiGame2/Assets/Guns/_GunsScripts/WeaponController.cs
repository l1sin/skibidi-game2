using Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public GameObject[] Weapons;
    public Gun[] AllGuns;
    public Image[] GunIcons;
    public GameObject[] LockIcons;
    public Color DefaultColor;
    public Color SelectedColor;
    public Gun CurrentGun;
    public TextMeshProUGUI AmmoText;
    [SerializeField] private CharacterMovement _characterMovement;

    private void OnEnable()
    {
        InputManager.W1.performed += SelectWeapon;
        InputManager.W2.performed += SelectWeapon;
        InputManager.W3.performed += SelectWeapon;
        InputManager.W4.performed += SelectWeapon;
        InputManager.W5.performed += SelectWeapon;
        InputManager.W6.performed += SelectWeapon;
        InputManager.W7.performed += SelectWeapon;
        InputManager.W8.performed += SelectWeapon;
    }

    private void OnDisable()
    {
        InputManager.W1.performed -= SelectWeapon;
        InputManager.W2.performed -= SelectWeapon;
        InputManager.W3.performed -= SelectWeapon;
        InputManager.W4.performed -= SelectWeapon;
        InputManager.W5.performed -= SelectWeapon;
        InputManager.W6.performed -= SelectWeapon;
        InputManager.W7.performed -= SelectWeapon;
        InputManager.W8.performed -= SelectWeapon;
    }

    private void SelectWeapon(InputAction.CallbackContext obj)
    {
        if (!CurrentGun.CanSwitch) return;
        int ind = int.Parse(obj.action.name) - 1;
        if (CurrentGun != AllGuns[ind] && AllGuns[ind].GunLevel > 0)
        {
            ChangeGun(ind);
            ChangeIcon(ind);
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
            CurrentGun.WalkStart();
        }
        else
        {
            CurrentGun.WalkEnd();
        }
    }

    public void Awake()
    {
        //SetGunProperties(SaveManager.Instance.CurrentProgress.UpgradeLevel, SaveManager.Instance.CurrentProgress.GunLevel);
        ChangeGun(0);
        ChangeIcon(0);
    }
    //public void Update()
    //{
    //    if (CurrentGun.CanSwitch)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Alpha1) && CurrentGun != AllGuns[0] && AllGuns[0].GunLevel > 0)
    //        {
    //            ChangeGun(0);
    //            ChangeIcon(0);
    //        }

    //        if (Input.GetKeyDown(KeyCode.Alpha2) && CurrentGun != AllGuns[1] && AllGuns[1].GunLevel > 0)
    //        {
    //            ChangeGun(1);
    //            ChangeIcon(1);
    //        }

    //        if (Input.GetKeyDown(KeyCode.Alpha3) && CurrentGun != AllGuns[2] && AllGuns[2].GunLevel > 0)
    //        {
    //            ChangeGun(2);
    //            ChangeIcon(2);
    //        }

    //        if (Input.GetKeyDown(KeyCode.Alpha4) && CurrentGun != AllGuns[3] && AllGuns[3].GunLevel > 0)
    //        {
    //            ChangeGun(3);
    //            ChangeIcon(3);
    //        }

    //        if (Input.GetKeyDown(KeyCode.Alpha5) && CurrentGun != AllGuns[4] && AllGuns[4].GunLevel > 0)
    //        {
    //            ChangeGun(4);
    //            ChangeIcon(4);
    //        }

    //        if (Input.GetKeyDown(KeyCode.Alpha6) && CurrentGun != AllGuns[5] && AllGuns[5].GunLevel > 0)
    //        {
    //            ChangeGun(5);
    //            ChangeIcon(5);
    //        }
    //    }
    //}

    public void ChangeGun(int index)
    {
        foreach (GameObject w in Weapons)
        {
            w.SetActive(false);
        }
        Weapons[index].SetActive(true);
        CurrentGun = Weapons[index].GetComponent<Gun>();
        UpdateAmmoText(CurrentGun.Ammo);
    }

    public void ChangeIcon(int index)
    {
        foreach (Image i in GunIcons)
        {
            i.color = DefaultColor;
        }
        GunIcons[index].color = SelectedColor;
    }
    public void UpdateAmmoText(float ammoAmount)
    {
        AmmoText.text = string.Format("{0:f0}", ammoAmount);
    }

    public void SetGunProperties(int[] upgradeLevel, int[] gunLevel)
    {
        foreach (Gun gun in AllGuns)
        {
            gun.AmmoLevel = upgradeLevel[2];
        }
        for (int i = 0; i < AllGuns.Length; i++)
        {
            AllGuns[i].GunLevel = gunLevel[i];
            if (AllGuns[i].GunLevel > 0) LockIcons[i].SetActive(false);
        }
    }
}
