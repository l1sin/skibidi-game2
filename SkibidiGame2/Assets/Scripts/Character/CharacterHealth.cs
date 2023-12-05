using Sounds;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float _healthCurrent;
    [SerializeField] private float _healthMax;
    [SerializeField] private float _healthBuff;
    [SerializeField] private bool _isDead;
    [SerializeField] private Image _hpBar;
    [SerializeField] private AudioClip _hurt;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    public int HitsTaken;

    public void Start()
    {
        _healthMax *= Mathf.Pow(_healthBuff, SaveManager.Instance.CurrentProgress.UpgradeLevels[8]);
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
            SoundManager.Instance.PlaySound(_hurt, _audioMixerGroup);
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
