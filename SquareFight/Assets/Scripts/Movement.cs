using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [System.Serializable]
    public class MovementStats
    {
        public float maxJoystickToMove = .1f;
        public float moveSpeed, counterMovementSpeed;
        public float jumpForce;
        public float maxToJump = 0.85f;
        public float jumpChargeMax, jumpChargeSpeed;
        public float groundCastDist = .75f;
    }

    public MovementStats stats;
    public Joystick joystick;
    public Slider jumpSlider;
    public LayerMask whatIsGround;
    [Header("Grounded")]
    public Vector2 groundCheckPos;
    public float groundCheckRadius;

    [Header("Teams")]
    public Team team;
    public Shoot gun;

    // Movement
    float x;
    float jumpCharge;
    Rigidbody2D rb ;

    // Testing
    public float boxCastAngle = 90f;

    Vector2 checkPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checkPos = rb.position + groundCheckPos;
        jumpSlider.maxValue = stats.jumpChargeMax;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        //Debug.Log(IsGrounded());
    }

    private void FixedUpdate()
    {
        DoMovement();
        if (jumpCharge >= stats.jumpChargeMax && IsGrounded())
        {
            Jump();
            jumpCharge = 0f;
        }
    }

    void DoMovement()
    {
        rb.AddForce(new Vector2(x, 0) * stats.moveSpeed, ForceMode2D.Force);
        rb.AddForce(-stats.counterMovementSpeed * rb.velocity * Vector2.right, ForceMode2D.Force);
    }

    void GetInput()
    {
        
        if(joystick.Horizontal >= stats.maxJoystickToMove)
        {
            x = 1f;
        }
        else if(joystick.Horizontal <= -stats.maxJoystickToMove)
        {
            x = -1f;
        }
        else
        {
            x = 0f;
        }

        // PC testing
        if (x == 0f) 
        {
            if(team == Team.Blue)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    x = -1f;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    x = 1f;
                }
                if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
                {
                    Jump();
                }
            }

            if (team == Team.Red)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    x = -1f;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    x = 1f;
                }
                if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
                {
                    Jump();
                }
            }
        }
        

        // Jump
        if(joystick.Vertical >= stats.maxToJump && IsGrounded())
        {
            jumpCharge += stats.jumpChargeSpeed * Time.deltaTime;
            jumpSlider.value = jumpCharge;
        }
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
