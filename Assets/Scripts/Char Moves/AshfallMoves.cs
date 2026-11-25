using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AshfallMoves : Moves
{
    private bool chargeHit;
    private float chargeSpeed;
    private Rigidbody2D rb;
    public GameObject projectilePrefab;

    private void Start()
    {
        isP2 = gameObject.GetComponent<Moves>().isP2;
        rb = gameObject.GetComponent<Rigidbody2D>();
        chargeSpeed = gameObject.GetComponent<CharStats>().speed * 10f;
        chargeHit = false;
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
        //if character is attacking or defending animation, return
        if (gameObject.GetComponent<PlayerAnims>().lockInput) return;

        StartCoroutine(Charge());
    }

    protected override void SpcAttack()
    {
        //if character is attacking or defending animation, return
        if (gameObject.GetComponent<PlayerAnims>().lockInput) return;

        //shoot a fireball
        StartCoroutine(shootFireballs());
    }

    protected override void DefMove()
    {
        //if character is attacking or defending animation, return
        if (gameObject.GetComponent<PlayerAnims>().lockInput) return;
    }

    //std attack helper - check whether the enemy was hit and damage them
    private IEnumerator Charge()
    {
        yield return new WaitForSeconds(.05f);

        //only shoot if the standard attack animation is playing
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Std_Attack"))
        {
            //move player forward
            rb.velocity = new Vector2(isP2 ? -(chargeSpeed) : chargeSpeed, rb.velocity.y);
            rb.AddForce(new Vector2(isP2 ? 1 : -1, 0));  //decelerate
            chargeHit = false;

            //check for collision during move
            float timer = .1f;
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

    IEnumerator shootFireballs()
    {
        yield return new WaitForSeconds(.5f);

        //only shoot if the special attack animation is playing
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Spc_Attack"))
        {
            //fb1
            GameObject fireball1 = Instantiate(projectilePrefab,
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.y+1.2f),
                new Quaternion(0, 0, 0, 0));
            fireball1.transform.localScale = new Vector2(.3f, .3f);
            fireball1.GetComponentInChildren<Rigidbody2D>().gravityScale = 0;
            fireball1.GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(isP2 ? -40 : 40, 0);
            fireball1.GetComponentInChildren<Projectile>().dam = 10;
            fireball1.GetComponentInChildren<Projectile>().projectorIsP2 = isP2;

            //fb2
            GameObject fireball2 = Instantiate(projectilePrefab,
                new Vector2(gameObject.transform.position.x+.5f, gameObject.transform.position.y + 1.5f),
                new Quaternion(0, 0, 0, 0));
            fireball2.transform.localScale = new Vector2(.3f, .3f);
            fireball2.GetComponentInChildren<Rigidbody2D>().gravityScale = 0;
            fireball2.GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(isP2 ? -40 : 40, 0);
            fireball2.GetComponentInChildren<Projectile>().dam = 10;
            fireball2.GetComponentInChildren<Projectile>().projectorIsP2 = isP2;
        }
    }
}
