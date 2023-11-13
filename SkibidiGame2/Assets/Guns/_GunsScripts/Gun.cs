using UnityEngine;

public class Gun : MonoBehaviour
{
    public WeaponController WeaponController;
    public CharacterMovement _CharacterMovement;
    public Animator Animator;
    public Transform GunObject;
    public Camera Camera;
    public LayerMask Targets;
    public bool CanSwitch = true;
    public float Damage;
    public bool IsShooting;
    public float StartAmmo;
    public float AmmoLevel;
    public float Ammo;
    public float GunLevel;
    public float AnimationSpeedModifyer;

    public float AmmoBonus = 0.2f;
    public float DamageBonus = 0.125f;
    public float AnimationSpeedBonus = 0.125f;

    public void Start()
    {
        Animator.keepAnimatorStateOnDisable = true;
        Ammo = StartAmmo * (1 + AmmoLevel * AmmoBonus);
        Damage *= 1 + (GunLevel - 1) * DamageBonus;
        AnimationSpeedModifyer *= 1 + (GunLevel - 1) * AnimationSpeedBonus;
        Animator.SetFloat("AnimationSpeedModifyer", AnimationSpeedModifyer);
        UpdateAmmo(0);
    }

    public void Walk()
    {
        if (CharacterInput.IsMoving && _CharacterMovement.IsGrounded)
        {
            Animator.SetBool("Walking", true);
        }
        else
        {
            Animator.SetBool("Walking", false);
        }
    }

    public virtual void EndShooting()
    {
        IsShooting = false;
        CanSwitch = true;
    }

    public virtual void UpdateAmmo(float ammoConsumption)
    {
        Ammo -= ammoConsumption;
        WeaponController.UpdateAmmoText(Ammo);
    }
}
