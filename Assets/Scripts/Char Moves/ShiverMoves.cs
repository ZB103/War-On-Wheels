using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiverMoves : Moves
{
    public GameObject claw;
    private void Start()
    {
        isP2 = gameObject.GetComponent<Moves>().isP2;
        //claw.SetActive(false);  //hide claw to start
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

        //show grabber animation
        if (claw.GetComponent<Claw>().Attack()) { healthScript.Hurt(isP2 ? 1 : 2, pStats.dam); }
    }

    protected override void SpcAttack()
    {
        //if character is attacking or defending animation, return
        if (gameObject.GetComponent<PlayerAnims>().lockInput) return;
    }

    protected override void DefMove()
    {
        //if character is attacking or defending animation, return
        if (gameObject.GetComponent<PlayerAnims>().lockInput) return;
    }
}
