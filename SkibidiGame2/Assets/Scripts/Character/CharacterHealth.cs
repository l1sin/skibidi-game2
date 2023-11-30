using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float _healthCurrent;
    [SerializeField] private float _healthMax;
    [SerializeField] private bool _isDead;
    [SerializeField] private Image _hpBar;
    public int HitsTaken;

    public void Start()
    {
        _healthMax *= Mathf.Pow(2, SaveManager.Instance.CurrentProgress.UpgradeLevels[8]);
        _healthCurrent = _healthMax;
        UpdateHealthBar();
    }
    
    public void Die()
    {
        LevelController.Instance.ShowDeathScreen();
    }

    public void GetDamage(float damage)
    {
        if (!_isDead)
        {
            HitsTaken++;
            _healthCurrent -= damage;
            UpdateHealthBar();
            if (_healthCurrent <= 0)
            {
                Die();
            }
        }
    }

    private void UpdateHealthBar()
    {
        _hpBar.fillAmount = _healthCurrent / _healthMax;
    }
}
