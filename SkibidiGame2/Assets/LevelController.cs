using Sounds;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _winScreen;

    [SerializeField] private GameObject _musicPlayer;

    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _winSound;

    public void Start() => Instance = this;

    public void ShowDeathScreen()
    {
        PauseManager.TogglePause(true);
        CursorHelper.ShowCursor();
        _deathScreen.SetActive(true);
        SoundManager.Instance.PlaySound(_deathSound);
        Destroy(_musicPlayer);
    }

    public void ShowWinScreen()
    {
        PauseManager.TogglePause(true);
        CursorHelper.ShowCursor();
        _winScreen.SetActive(true);
        SoundManager.Instance.PlaySound(_winSound);
        Destroy(_musicPlayer);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        PauseManager.TogglePause(false);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        PauseManager.TogglePause(false);
    }

    public void WatchAd()
    {
        Debug.Log("Ad");
    }
}
