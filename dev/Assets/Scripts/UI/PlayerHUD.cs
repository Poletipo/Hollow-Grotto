using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    [Header("Player")]
    public Player player;
    private GameObject worm;

    [Header("Reticule")]
    public Image Reticule;
    public Color NormalReticuleColor;
    public Color DigReticuleColor;
    public Color InteractReticuleColor;
    public Color GrappleReticuleColor;

    [Header("Digging UI")]
    public Gradient SliderColor;
    public CanvasGroup DigPivot;
    public Image DigSliderFill;
    public TextMeshProUGUI DigPercentTxt;
    public Image DigOverheatIcon;

    [Header("Health UI")]
    public Gradient HealthTxtColor;
    public CanvasGroup HealthPivot;
    public Image HealthSliderFill;
    public TextMeshProUGUI HealthTxt;
    public GameObject HealthRate;
    public Sprite FlatLine;
    public Vector2 HeartRateSpeedMinMax;
    private float HeartRateSpeed;
    public CanvasGroup HurtOverlayCG;

    [Header("DigSize UI")]
    public CanvasGroup DigSizePivot;
    public TextMeshProUGUI DigSizeTxt;

    [Header("Interact UI")]
    public CanvasGroup InteractPivot;
    public TextMeshProUGUI InteractTxt;

    [Header("Warning UI")]
    public float WarningDistance = 50;
    public GameObject WarningRadar;

    [Header("Grapple UI")]
    public CanvasGroup GrapplePivot;
    public Image GrappleSliderFill;

    private float healtRateOffset = 0;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        player.OnDigPercentChange += OnDigPercentChange;
        player.OnDigOverheating += OnDigOverheating;
        player.OnInRangeChange += OnInRangeChange;
        player.OnDigSizeChanged += OnDigSizeChanged;

        player.OnGrappleTimerChange += OnGrappleTimerChange;

        player.health.OnChanged += OnHealthChanged;
        player.health.OnHit += OnHit;
        player.health.OnDeath += OnDeath;

        worm = GameManager.Instance.Worm;
        HeartRateSpeed = HeartRateSpeedMinMax.y;
    }

    private void OnHit(Health health)
    {
        HurtOverlayCG.alpha = 0.25f;
    }

    private void OnGrappleTimerChange(Player player)
    {
        if (player.GrappleTimer >= player.grappleInterval) {
            GrapplePivot.alpha = 0;
        }
        else {
            GrapplePivot.alpha = 1;

            float grapplePercent = player.GrappleTimer / player.grappleInterval;

            GrappleSliderFill.fillAmount = grapplePercent;
        }
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
        healtRateOffset += HeartRateSpeed * Time.deltaTime;

        if (healtRateOffset > 1) {
            healtRateOffset = 1 - healtRateOffset;
        }
        HealthRate.GetComponent<Image>().material.mainTextureOffset = Vector2.right * healtRateOffset;


        if (HurtOverlayCG.alpha > 0f) {
            HurtOverlayCG.alpha -= Time.deltaTime * 0.5f;
        }

        float dist = Vector3.Distance(worm.transform.position, player.transform.position);


        if (dist <= WarningDistance) {
            float fillAmount = ((WarningDistance - dist) / WarningDistance) / 2.0f;
            WarningRadar.GetComponent<Image>().fillAmount = fillAmount;
            Vector2 WormPos2D = new Vector2(worm.transform.position.x, worm.transform.position.z);
            Vector2 playerPos2D = new Vector2(player.transform.position.x, player.transform.position.z);
            Vector2 playerDir2D = new Vector2(player.transform.forward.x, player.transform.forward.z);

            Vector2 direction = WormPos2D - playerPos2D;
            float warningAngle = Vector2.SignedAngle(playerDir2D, direction);

            float offset = 180 * fillAmount;

            Vector3 warningRotation = Vector3.forward * (warningAngle + offset);

            WarningRadar.transform.rotation = Quaternion.Euler(warningRotation);
        }
        else {
            WarningRadar.GetComponent<Image>().fillAmount = 0;
        }
    }

    private void OnDeath(Health health)
    {
        canvasGroup.alpha = 0;
    }

    private void OnDigSizeChanged(Player player)
    {
        DigSizeTxt.text = player.DigSize.ToString() + "m";
    }

    private void OnHealthChanged(Health health)
    {
        float healthPercent = health.hp / (health.maxHp * 1.0f);
        HeartRateSpeed = Mathf.Lerp(HeartRateSpeedMinMax.x, HeartRateSpeedMinMax.y, healthPercent);

        HealthTxt.text = (healthPercent * 100).ToString();
        HealthTxt.color = HealthTxtColor.Evaluate(healthPercent);

        if (healthPercent <= 0) {
            HealthRate.GetComponent<Image>().sprite = FlatLine;
        }
    }

    private void OnInRangeChange(Player player)
    {
        if (player.InRangeState == Player.InRange.Nothing) {
            Reticule.color = NormalReticuleColor;
            InteractPivot.alpha = 0;
        }
        else if (player.InRangeState == Player.InRange.Destructible) {
            Reticule.color = DigReticuleColor;
        }
        else if (player.InRangeState == Player.InRange.Interactible) {
            Reticule.color = InteractReticuleColor;

            InteractPivot.alpha = 1;

            if (player.hit.collider.GetComponent<Interactible>() != null) {
                InteractTxt.text = player.hit.collider.GetComponent<Interactible>().Action.ToString();
            }
        }
        else if (player.InRangeState == Player.InRange.Grapple) {
            Reticule.color = GrappleReticuleColor;
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

}
