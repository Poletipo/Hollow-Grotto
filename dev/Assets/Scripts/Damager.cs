using UnityEngine;

public class Damager : MonoBehaviour {

    public int damage = 30;

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null) {
            player.health.Hurt(damage);
        }
    }
}
