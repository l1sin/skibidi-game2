using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float _healthCurrent;
    [SerializeField] private float _healthMax;
    [SerializeField] private bool _isDead;
    [SerializeField] private Image _hpBar;

    public void Start()
    {
        _healthCurrent = _healthMax;
        UpdateHealthBar();
    }
    
    public void Die()
    {
        Debug.Log("Player is Dead!");
    }

    public void GetDamage(float damage)
    {
        if (!_isDead)
        {
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
