using UnityEngine;
using UnityEngine.UI;

public class Summary : MonoBehaviour
{
    [SerializeField] private Text _timerText;
    [SerializeField] private Text _enemiesText;
    [SerializeField] private Text _hitText;
    [SerializeField] private Text _rewardText;

    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private CharacterHealth _characterHealth;

    [SerializeField] private int _rewardPerEnemy;
    [SerializeField] private int _noHitRewardMulty;

    private void Start()
    {
        int enemies = _enemySpawner.DeadEnemies;
        int hits = _characterHealth.HitsTaken;
        int reward = enemies * _rewardPerEnemy;
        if (hits == 0) reward *= _noHitRewardMulty;

        _timerText.text = GetTimerText(_enemySpawner.Timer);
        _enemiesText.text = enemies.ToString();

        if (hits > 0) _hitText.text = hits.ToString();
        else
        {
            _hitText.text = "NO HIT!";
            _hitText.color = Color.red;
        }

        _rewardText.text = reward.ToString();

        SaveManager.Instance.CurrentProgress.Level++;
        SaveManager.Instance.CurrentProgress.Money += reward;
        SaveManager.Instance.CurrentProgress.Kills += enemies;
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
}
