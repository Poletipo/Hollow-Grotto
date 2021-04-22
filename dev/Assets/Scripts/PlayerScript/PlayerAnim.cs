using UnityEngine;

public class PlayerAnim : MonoBehaviour {

    public enum AnimationState {
        Digging,
        Idle,
        Walking,
        Jumping,
        Falling,
        Landing
    }

    public AnimationState AnimState { get; set; }

    [Header("Animation")]
    public Animator animator;

    Player player;
    MovementController mc;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        player.OnDigging += OnDiging;

        mc = GetComponent<MovementController>();
        mc.OnIdle += OnIdle;
        mc.OnMoving += OnMoving;
        mc.OnJumping += OnJumping;
        mc.OnFalling += OnFalling;
        mc.OnLanding += OnLanding;
        mc.OnSliping += OnSlipping;
    }

    private void OnSlipping(MovementController movementController)
    {
        animator.SetBool("Grounded", true);
    }

    private void OnLanding(MovementController movementController)
    {
        animator.SetBool("Grounded", true);
    }

    private void OnFalling(MovementController movementController)
    {
        animator.SetBool("Grounded", false);
    }

    private void OnJumping(MovementController movementController)
    {
        animator.SetTrigger("Jump");
        animator.SetBool("Grounded", false);
    }

    private void OnMoving(MovementController movementController)
    {
        animator.SetBool("Grounded", true);
        animator.SetBool("Moving", true);
    }

    private void OnIdle(MovementController movementController)
    {
        animator.SetBool("Grounded", true);
        animator.SetBool("Moving", false);
    }

    private void OnDiging(Player player)
    {
        animator.Play("Armature|Dig");
        //animator.SetTrigger("Digging");
    }
}
