using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputManager))]
public class Movement : MonoBehaviour
{
    [System.Serializable]
    public class MovementStats
    {
        public float moveSpeed, counterMovementSpeed;
        public float jumpForce;
        public float groundCastDist = .75f;
        public int jumpCount = 1;
        public float coyoteTime = 0.1f;
        public float jumpDelay = 0.15f;
        public float bufferDuration = 0.15f;
    }

    public MovementStats stats;
    public LayerMask whatIsGround;
    [Header("Grounded")]
    public Vector2 groundCheckPos;
    public float groundCheckRadius;

    [Header("Teams")]
    public Team team;
    public Shoot gun;

    // Movement
    InputManager input_manager;
    float x;
    int _jump_count;
    float _time_off_ground = 0f;
    float _time_not_jumped = 0f;
    float _time_since_last_jump_press = 0f;
    Rigidbody2D rb ;

    // Testing
    public float boxCastAngle = 90f;

    Vector2 checkPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checkPos = rb.position + groundCheckPos;
        input_manager = GetComponent<InputManager>();

        // Subscribe jumping events to key presses
        input_manager.onJumpPressed += RegisterJumpPress;
        input_manager.onJumpReleased += JumpCutoff;

        _time_off_ground = 0f;
        _time_not_jumped = 0f;
        _time_since_last_jump_press = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        _time_not_jumped += Time.deltaTime;
        _time_since_last_jump_press += Time.deltaTime;
        x = input_manager.horizontalMovement; // Get horizontal movement

        // Reset count if able to jump and grounded
        if(IsGrounded() && _time_not_jumped >= stats.jumpDelay){
            _jump_count = stats.jumpCount;
            _time_off_ground = 0f;
        }
        else _time_off_ground += Time.deltaTime;
        
        // If leave platform without jumping, jump count is lowered by one
        if(_jump_count == stats.jumpCount && !(_time_off_ground <= stats.coyoteTime)) _jump_count--;
        if(_time_since_last_jump_press <= stats.bufferDuration) TryToJump();
    }

    private void FixedUpdate()
    {
        DoMovement();
    }

    void DoMovement()
    {
        rb.AddForce(new Vector2(x, 0) * stats.moveSpeed, ForceMode2D.Force);
        rb.AddForce(-stats.counterMovementSpeed * rb.velocity * Vector2.right, ForceMode2D.Force);
    }

    void RegisterJumpPress(){
        _time_since_last_jump_press = 0f;
    }

    void TryToJump(){
        // Positive jump count and jump cooldown is over
        if(_jump_count <= 0 || _time_not_jumped < stats.jumpDelay) return;
        Jump();
    }

    void JumpCutoff(){
        if(rb.velocity.y <= 0f) return;
        float cutoffRate = 0.4f;
        rb.AddForce(cutoffRate * rb.velocity.y * Vector2.down, ForceMode2D.Impulse);
    }

    void Jump()
    {
        _time_since_last_jump_press = 150f;
        // Reset vertical movement
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(stats.jumpForce * Vector2.up, ForceMode2D.Impulse);
        _jump_count--;
        _time_not_jumped = 0f;
    }

    bool IsGrounded()
    {
        return Physics2D.CircleCast(rb.position + groundCheckPos, groundCheckRadius, Vector2.down, 0f, whatIsGround);
    }

    private void OnDrawGizmosSelected()
    {
        checkPos = transform.position + new Vector3(groundCheckPos.x, groundCheckPos.y, 0f);
        Gizmos.DrawSphere(checkPos, .1f);
        //rb = GetComponent<Rigidbody2D>();
        Gizmos.DrawLine(checkPos, checkPos - new Vector2(0, stats.groundCastDist));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Gun g) && gun.stats == null)
        {
            gun.stats = g.stats;
            gun.currentMag = gun.stats.clipSize;
            gun.cooldown = 0f;
            gun.isShooting = false;
            g.GunDestroy();
        }
    }
}
