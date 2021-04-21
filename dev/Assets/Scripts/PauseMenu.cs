using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    [Header("Main Menu Warning")]
    public GameObject MainMenuWarning;

    [Header("Pause Menu UI")]
    public Button ResumeBtn;
    public Button MainMenuBtn;

    [Header("HUD")]
    public GameObject HUD;

    CanvasGroup canvasGroup;

    bool isOpen = true;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        ClosePauseMenu();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            if (isOpen) {
                ClosePauseMenu();
            }
            else {
                OpenPauseMenu();
            }
        }
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameManager.Instance.Player.GetComponent<Player>().PlayerEnabled = false;
        HUD.SetActive(false);
        canvasGroup.alpha = 1;
        EnableButtons();
        isOpen = true;
    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.Player.GetComponent<Player>().PlayerEnabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canvasGroup.alpha = 0;
        HUD.SetActive(true);
        DisableButtons();
        MainMenuWarning.SetActive(false);
        isOpen = false;
    }

    public void MainMenu()
    {
        LevelManager.LoadLevel(LevelManager.Level.MainMenu);
    }


    public void ShowMainMenuMenu()
    {
        DisableButtons();
        MainMenuWarning.SetActive(true);
    }
    public void HideMainMenuMenu()
    {
        EnableButtons();
        MainMenuWarning.SetActive(false);
    }


    void EnableButtons()
    {
        ResumeBtn.enabled = true;
        MainMenuBtn.enabled = true;
    }

    void DisableButtons()
    {
        ResumeBtn.enabled = false;
        MainMenuBtn.enabled = false;
    }

}
