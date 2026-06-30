using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class P2Controls : MonoBehaviour
{
    private Rigidbody2D rb;
    public BoxCollider2D pCollider;
    private CharStats cs;
    public float moveSpeed; //horizontal movement
    private float acc;  //horizontal acceleration
    private float dec;  //horizontal deceleration
    private float jumpForce; //vertical movement
    public bool movementOn; //player can move?
    private float maxJumpBufferTime;    //jump is allowed before hitting the ground
    public float jumpBufferTimer;      //timer used to determine whether jump allowed at that moment
    public bool jumpStart;      //used to trigger jump animation
    private float descentMultiplier;  //amount of velocity adjustment when falling
    private bool right;
    private bool left;
    private bool jumpQueued;
    public GameObject ground;

    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        pCollider = GetComponent<BoxCollider2D>();
        cs = GetComponent<CharStats>();
        moveSpeed = cs.speed;  //get speed multiplier from stats
        acc = 2.5f;
        dec = 3.5f;
        jumpForce = 2 * cs.speed;
        Physics2D.gravity = new Vector2(0, -70f);
        movementOn = true;
        maxJumpBufferTime = .5f;
        jumpBufferTimer = 0f;
        jumpStart = false;
        descentMultiplier = 0.3f;
        right = false;
        left = false;
        jumpQueued = false;
    }

    private void Update()
    {
        //key presses checked for in update for consistency
        if (movementOn)
        {
            //right movement
            if (Input.GetKey(KeyCode.RightArrow) && rb.velocity.x < moveSpeed) { right = true; }
            else { right = false; }
            //left movement
            if (Input.GetKey(KeyCode.LeftArrow) && rb.velocity.x > -moveSpeed) { left = true; }
            else { left = false; }

            //jump buffer countdown
            if (cs.canJump)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && jumpBufferTimer <= 0f)
                { jumpBufferTimer = maxJumpBufferTime; jumpQueued = true; jumpStart = true; }
                jumpBufferTimer -= Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        //right movement
        if (right)
        {
            //same direction
            if (rb.velocity.x >= 0)
            {
                rb.velocity = new Vector2(rb.velocity.x + acc, rb.velocity.y);
            }
            //turn around
            else
            {
                rb.velocity = new Vector2(rb.velocity.x * -0.2f, rb.velocity.y);
            }
        }
        else if (rb.velocity.x > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x - dec, rb.velocity.y);
            if (rb.velocity.x < dec)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        //left movement
        if (left)
        {
            //same direction
            if (rb.velocity.x <= 0)
            {
                rb.velocity = new Vector2(rb.velocity.x - acc, rb.velocity.y);
            }
            //turn around
            else
            {
                rb.velocity = new Vector2(rb.velocity.x * -0.2f, rb.velocity.y);
            }
        }
        else if (rb.velocity.x < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x + dec, rb.velocity.y);
            if (rb.velocity.x > -dec)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        //execute jump if within jump buffer
        if (jumpQueued)
        {
            jumpQueued = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //reduced gravity at top of jump (apex float)
        if (!IsGrounded() && Mathf.Abs(rb.velocity.y) <= 0.5f)
        {
            rb.gravityScale = 0.3f;
        }
        else if (!IsGrounded())
        {
            rb.gravityScale = 1f;
        }

        //fall quickly
        if (rb.velocity.y < -1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - descentMultiplier);
        }
    }

    //determines whether the player is touching the ground
    public bool IsGrounded()
    {
        return Physics2D.BoxCast(pCollider.bounds.center, pCollider.bounds.size, 0f, Vector2.down, 0.5f);
    }

    //determines whether the player is just above the ground, for purposes of queueing a jump
    public bool IsAlmostGrounded()
    {
        return Physics2D.BoxCast(pCollider.bounds.center, pCollider.bounds.size, 0f, Vector2.down, 0.85f);
    }

    //change move/jump stats based on stress levels
    public void UpdateStats()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
