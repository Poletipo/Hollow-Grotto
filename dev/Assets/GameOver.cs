using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    [Header("Game Over UI")]
    public Button MainMenuBtn;
    public TextMeshProUGUI FixedRobotCount;

    [Header("HUD")]
    public GameObject HUD;
    public GameObject PauseMenu;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        GameManager.Instance.Player.GetComponent<Health>().OnDeath += OnDeath;
    }

    private void ShowGameOver()
    {
        SaveManager.DeleteWorld();
        Player player = GameManager.Instance.Player.GetComponent<Player>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        player.PlayerEnabled = false;
        HUD.SetActive(false);
        PauseMenu.SetActive(false);

        FixedRobotCount.text = player.FixedRobotCount.ToString();

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        MainMenuBtn.enabled = true;

        Time.timeScale = 0;
    }

    public void MainMenu()
    {
        LevelManager.LoadLevel(LevelManager.Level.MainMenu);
    }

    private void OnDeath(Health health)
    {
        StartCoroutine(OnDeathCoroutine());
    }

    IEnumerator OnDeathCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        ShowGameOver();
    }

}
