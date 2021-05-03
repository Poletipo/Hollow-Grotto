﻿using TMPro;
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

    [Header("DigSize UI")]
    public CanvasGroup DigSizePivot;
    public TextMeshProUGUI DigSizeTxt;

    [Header("Interact UI")]
    public CanvasGroup InteractPivot;
    public TextMeshProUGUI InteractTxt;

    [Header("Interact UI")]
    public float WarningDistance = 50;
    public GameObject WarningRadar;

    private void Start()
    {
        player.OnDigPercentChange += OnDigPercentChange;
        player.OnDigOverheating += OnDigOverheating;
        player.OnInRangeChange += OnInRangeChange;
        player.OnDigSizeChanged += OnDigSizeChanged;

        player.health.OnChanged += OnHealthChanged;

        worm = GameManager.Instance.Worm;
        HeartRateSpeed = HeartRateSpeedMinMax.y;
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

    float offset = 0;
    private void Update()
    {

        offset += HeartRateSpeed * Time.deltaTime;

        if (offset > 1) {
            offset = 1 - offset;
        }

        HealthRate.GetComponent<Image>().material.mainTextureOffset = Vector2.right * offset;


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



}
