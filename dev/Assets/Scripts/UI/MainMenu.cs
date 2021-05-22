using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [Header("Main Menu UI")]
    public Button WorldBtn;
    public Button PlayBtn;
    public Button QuitBtn;
    public Button InfoBtn;

    [Header("New World Warning")]
    public GameObject NewWorldWarning;

    [Header("Quit Warning")]
    public GameObject QuitWarning;

    [Header("Info screen")]
    public GameObject InfoScreen;

    private bool worldExist = true;

    private void Awake()
    {
        if (!SaveManager.SaveExist()) {
            PlayBtn.gameObject.SetActive(false);
            worldExist = false;
        }
    }

    public void PlayGame()
    {
        LevelManager.LoadLevel(LevelManager.Level.MainScene);
    }

    public void ShowQuitMenu()
    {
        DisableFGButton();
        QuitWarning.SetActive(true);
    }
    public void HideQuitMenu()
    {
        EnableFGButton();
        QuitWarning.SetActive(false);
    }

    public void QuitMenu()
    {
        Application.Quit();
    }

    public void ShowNewWorld()
    {
        if (worldExist) {
            DisableFGButton();
            NewWorldWarning.SetActive(true);
        }
        else {
            CreateNewWorld();
        }
    }

    public void HideNewWorld()
    {
        EnableFGButton();
        NewWorldWarning.SetActive(false);
    }

    public void CreateNewWorld()
    {
        SaveManager.DeleteWorld();
        LevelManager.LoadLevel(LevelManager.Level.MainScene);
    }

    public void ShowInfoMenu()
    {
        DisableFGButton();
        InfoScreen.SetActive(true);
    }
    public void HideInfoMenu()
    {
        EnableFGButton();
        InfoScreen.SetActive(false);
    }

    void DisableFGButton()
    {
        PlayBtn.enabled = false;
        WorldBtn.enabled = false;
        QuitBtn.enabled = false;
        InfoBtn.enabled = false;
    }
    void EnableFGButton()
    {
        PlayBtn.enabled = true;
        WorldBtn.enabled = true;
        QuitBtn.enabled = true;
        InfoBtn.enabled = true;
    }

}
