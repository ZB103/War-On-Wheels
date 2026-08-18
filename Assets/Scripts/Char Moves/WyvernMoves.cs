using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyvernMoves : Moves
{
    private Vector2 chargeSpeed;
    private Rigidbody2D rb;

    private void Start()
    {
        isP2 = gameObject.GetComponent<Moves>().isP2;
        rb = gameObject.GetComponent<Rigidbody2D>();
        chargeSpeed = new Vector2(5f, 0f);
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
        //char moves right
    }

    protected override void DefMove()
    {
        //std heal
        base.DefMove();
    }
}
