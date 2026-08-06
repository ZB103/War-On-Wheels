using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyvernMoves : Moves
{
    private void Start()
    {
        isP2 = gameObject.GetComponent<Moves>().isP2;
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
        //char moves toward target before calling std
        
    }

    protected override void DefMove()
    {
        //std heal
        base.DefMove();
    }
}
