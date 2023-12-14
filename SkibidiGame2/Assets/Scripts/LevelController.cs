using Sounds;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _winScreen;

    [SerializeField] private GameObject _musicPlayer;

    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _winSound;

    [SerializeField] private AudioMixerGroup audioMixerGroup;

    [SerializeField] private GameObject _pcLayout;
    [SerializeField] private GameObject _mobileLayout;

    public void Start()
    {
        Instance = this;
        if (SaveManager.Instance.IsMobile) _mobileLayout.SetActive(true);
        else _pcLayout.SetActive(true);
    }

    public void ShowDeathScreen()
    {
        PauseManager.TogglePause(true);
        CursorHelper.ShowCursor();
        _deathScreen.SetActive(true);
        SoundManager.Instance.PlaySound(_deathSound, audioMixerGroup);
        Destroy(_musicPlayer);
    }

    public void ShowWinScreen()
    {
        PauseManager.TogglePause(true);
        CursorHelper.ShowCursor();
        _winScreen.SetActive(true);
        SoundManager.Instance.PlaySound(_winSound, audioMixerGroup);
        Destroy(_musicPlayer);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(1);
        PauseManager.TogglePause(false);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        PauseManager.TogglePause(false);
    }
}
