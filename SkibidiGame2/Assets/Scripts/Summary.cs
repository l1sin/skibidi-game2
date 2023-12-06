using Sounds;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class Summary : MonoBehaviour
{
    [SerializeField] private Text _timerText;
    [SerializeField] private Text _enemiesText;
    [SerializeField] private Text _hitText;
    [SerializeField] private Text _rewardText;

    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private CharacterHealth _characterHealth;

    [SerializeField] private int _rewardPerEnemy;
    [SerializeField] private float _noHitRewardMulty;

    private int _reward;

    private void Start()
    {
        int enemies = _enemySpawner.DeadEnemies;
        int hits = _characterHealth.HitsTaken;
        _reward = enemies * _rewardPerEnemy;
        if (hits == 0) _reward = Mathf.CeilToInt((float)_reward * _noHitRewardMulty);

        _timerText.text = GetTimerText(_enemySpawner.Timer);
        _enemiesText.text = enemies.ToString();

        if (hits > 0) _hitText.text = hits.ToString();
        else
        {
            _hitText.text = SaveManager.Instance.Localization[26];
            _hitText.color = Color.red;
        }

        _rewardText.text = _reward.ToString();

        SaveManager.Instance.CurrentProgress.Level++;
        SaveManager.Instance.CurrentProgress.Money += _reward;
        SaveManager.Instance.CurrentProgress.Kills += enemies;

        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);

        if (SaveManager.Instance.CurrentProgress.Level <= 25)
        {
            Yandex.ReachGoal(SaveManager.Instance.CurrentProgress.Level.ToString());
        }
    }

    public string GetTimerText(float time)
    {
        int seconds = Mathf.FloorToInt(time);
        int minutes = seconds / 60;
        seconds %= 60;
        string text;
        if (seconds >= 10)
        {
            text = $"{minutes}:{seconds}";
        }
        else
        {
            text = $"{minutes}:0{seconds}";
        }
        return text;
    }

    public void WatchAd()
    {
        SoundManager.Instance.OffSound();
        Yandex.WatchAdDouble();
    }

    public void RewardAd()
    {
        _rewardText.text = (_reward * 2).ToString();
        SaveManager.Instance.CurrentProgress.Money += _reward;
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
        SoundManager.Instance.OnSound();
    }
}
