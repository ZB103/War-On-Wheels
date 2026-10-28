using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyvernMoves : Moves
{
    private bool chargeHit;
    private float chargeSpeed;
    private Rigidbody2D rb;

    private void Start()
    {
        isP2 = gameObject.GetComponent<Moves>().isP2;
        rb = gameObject.GetComponent<Rigidbody2D>();
        chargeHit = false;
        chargeSpeed = gameObject.GetComponent<CharStats>().speed * 10f;
    }

    void Update()
    {
        if (!isP2)  //player 1 ctrls
        {
            if (Input.GetKeyDown(KeyCode.Q)) { StdAttack(); }
            else if (Input.GetKeyDown(KeyCode.E)) { SpcAttack(); }
            else if (Input.GetKeyDown(KeyCode.Tab)) { DefMove(); }
        }
        else        //player 2 ctrls
        {
            if (Input.GetKeyDown(KeyCode.RightShift)) { StdAttack(); }
            else if (Input.GetKeyDown(KeyCode.Return)) { SpcAttack(); }
            else if (Input.GetKeyDown(KeyCode.Slash)) { DefMove(); }
        }
    }

    protected override void StdAttack()
    {
        //std melee
        base.StdAttack();
    }

    protected override void SpcAttack()
    {
        //if character is attacking or defending animation, return
        if (gameObject.GetComponent<PlayerAnims>().lockInput) return;

        StartCoroutine(checkChargeHit());
    }

    protected override void DefMove()
    {
        //std heal
        base.DefMove();
    }

    //spc attack helper - check whether the enemy was hit and damage them
    private IEnumerator checkChargeHit()
    {
        yield return new WaitForSeconds(.05f);

        //only shoot if the standard attack animation is playing
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Spc_Attack"))
        {
            //move player forward
            rb.velocity = new Vector2(isP2 ? -(chargeSpeed) : chargeSpeed, rb.velocity.y);
            rb.AddForce(new Vector2(isP2 ? 1 : -1, 0));  //decelerate
            chargeHit = false;

            //check for collision during move
            float timer = .2f;
            while (timer > 0)
            {
                if (hasContact) chargeHit = true;
                timer = timer - Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            //do damage if applicable
            if (chargeHit) healthScript.Hurt(isP2 ? 1 : 2, pStats.dam);
        }
    }
}
