using Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConsoleController : MonoBehaviour
{
    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private GameObject _console;
    [SerializeField] private InputField _inputField;
    private void ToggleConsole(InputAction.CallbackContext obj)
    {
        _console.SetActive(!_console.activeInHierarchy);
    }

    private void EnterInput(InputAction.CallbackContext obj)
    {
        if (_console == null) Debug.Log("null");
        if (!_console.activeInHierarchy) return;
        string code = _inputField.text;
        switch (code)
        {
            case "+progress":
                MaxProgess();
                break;
            case "-progress":
                MinProgess();
                break;
            case "money":
                AddMoney();
                break;
        }
        _console.SetActive(false);
    }

    private void MaxProgess()
    {
        Progress maxProgress = new Progress();
        maxProgress.Level = 25;
        maxProgress.UpgradeLevels = new int[] {5,5,5,5,5,5,5,5,5,5 };
        SaveManager.Instance.CurrentProgress = maxProgress;
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void MinProgess()
    {
        SaveManager.Instance.CurrentProgress = new Progress();
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void AddMoney()
    {
        SaveManager.Instance.CurrentProgress.Money += 10000;
        _mainMenuController.UpdateMoney();
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
    }

    private void OnEnable()
    {
        InputManager.ConsoleInputAction.performed += ToggleConsole;
        InputManager.EnterInputAction.performed += EnterInput;
    }

    private void OnDisable()
    {
        InputManager.ConsoleInputAction.performed -= ToggleConsole;
        InputManager.EnterInputAction.performed -= EnterInput;
    }
}
