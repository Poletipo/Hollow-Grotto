using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    [Header("Player")]
    public Player player;

    [Header("Digging UI")]
    public Gradient SliderColor;
    public CanvasGroup DigPivot;
    public Image DigSliderFill;
    public TextMeshProUGUI DigPercentTxt;
    public Image DigOverheatIcon;

    private void Start()
    {
        player.OnDigPercentChange += OnDigPercentChange;
        player.OnDigOverheating += OnDigOverheating;
        player.OnDigStopOverheating += OnDigStopOverheating;
    }

    private void OnDigStopOverheating(Player player)
    {
        DigOverheatIcon.enabled = false;
    }

    private void OnDigOverheating(Player player)
    {
        DigOverheatIcon.enabled = true;
    }

    private void OnDigPercentChange(Player player)
    {
        ShowDigging(player.DigPercent);
    }

    public void ShowDigging(float percent)
    {
        DigSliderFill.fillAmount = percent / 100.0f;
        Color digSliderColor = SliderColor.Evaluate(percent / 100.0f);
        DigSliderFill.color = digSliderColor;
        DigPercentTxt.text = percent.ToString("F1") + "%";
        DigPercentTxt.color = digSliderColor;
        DigPivot.alpha = 1;
    }

    public void HideDigging()
    {
        DigPivot.alpha = 0;
    }


}
