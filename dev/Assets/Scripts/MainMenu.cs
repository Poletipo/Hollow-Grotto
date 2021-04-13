using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    bool worldExist = true;

    [Header("Main Menu UI")]
    public Button WorldBtn;
    public Button PlayBtn;
    public Button QuitBtn;

    [Header("New World Warning")]
    public GameObject NewWorldWarning;

    [Header("Quit Warning")]
    public GameObject QuitWarning;


    private void Awake()
    {
        if (!SaveManager.SaveExist()) {
            PlayBtn.gameObject.SetActive(false);
            worldExist = false;
        }
    }

    public void PlayGame()
    {
        //load level
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
        //Leave game
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

    }


    void DisableFGButton()
    {
        PlayBtn.enabled = false;
        WorldBtn.enabled = false;
        QuitBtn.enabled = false;
    }
    void EnableFGButton()
    {
        PlayBtn.enabled = true;
        WorldBtn.enabled = true;
        QuitBtn.enabled = true;
    }




}
