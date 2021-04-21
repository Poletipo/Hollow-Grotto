using UnityEngine;

public class Health : MonoBehaviour {
    // delegate signature de fonction
    public delegate void HealthEvent(Health health);

    //Listeners
    public HealthEvent OnChanged;
    public HealthEvent OnHit;
    public HealthEvent OnHeal;
    public HealthEvent OnDeath;

    public int maxHp = 100;
    public int hp = 100;
    public float InvincibleTime = 1;
    public bool IsInvincible = false;

    public float InvincibleTimer { get; set; }


    public int SetHealth(int hp)
    {
        this.hp = hp;
        OnChanged?.Invoke(this);
        return this.hp;
    }


    public int Heal(int hp)
    {
        this.hp = Mathf.Clamp((this.hp + hp), 0, maxHp);
        OnChanged?.Invoke(this);
        OnHeal?.Invoke(this);

        return this.hp;
    }

    public int Hurt(int hp)
    {
        if (!IsInvincible) {
            if (InvincibleTimer < 0) {
                this.hp = Mathf.Clamp((this.hp - hp), 0, maxHp);
                OnHit?.Invoke(this);
                OnChanged?.Invoke(this);
                InvincibleTimer = InvincibleTime;
                if (this.hp <= 0) {
                    OnDeath?.Invoke(this);
                }
            }
        }
        return this.hp;
    }
    private void Update()
    {
        InvincibleTimer -= Time.deltaTime;
    }

}
