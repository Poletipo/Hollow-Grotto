using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    [Header("Player")]
    public Player player;

    [Header("Reticule")]
    public Image Reticule;
    public Color NormalReticuleColor;
    public Color DigReticuleColor;
    public Color InteractReticuleColor;

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
        player.OnInRangeChange += OnInRangeChange;
    }

    private void OnInRangeChange(Player player)
    {
        if (player.InRangeState == Player.InRange.Nothing) {
            Reticule.color = NormalReticuleColor;
        }
        else if (player.InRangeState == Player.InRange.Destructible) {
            Reticule.color = DigReticuleColor;
        }
        else if (player.InRangeState == Player.InRange.Interactible) {
            Reticule.color = InteractReticuleColor;
        }
    }

    private void OnDigOverheating(Player player)
    {
        if (player.IsOverHeating) {
            DigOverheatIcon.enabled = true;
        }
        else {
            DigOverheatIcon.enabled = false;
        }
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


    private void Update()
    {

    }



}
