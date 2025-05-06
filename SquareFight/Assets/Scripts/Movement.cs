using System.Collections;
using System.Collections.Generic;
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
    Rigidbody2D rb ;

    // Testing
    public float boxCastAngle = 90f;

    Vector2 checkPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checkPos = rb.position + groundCheckPos;
        input_manager = GetComponent<InputManager>();
        input_manager.onJumpPressed += TryToJump;
    }

    // Update is called once per frame
    void Update()
    {
        x = input_manager.horizontalMovement; // Get horizontal movement
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

    void TryToJump(){
        if(!IsGrounded()) return;
        Jump();
    }

    void Jump()
    {
        rb.AddForce(stats.jumpForce * Vector2.up, ForceMode2D.Impulse);
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
