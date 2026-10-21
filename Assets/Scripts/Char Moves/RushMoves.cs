using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushMoves : Moves
{
    private GameObject[] chromes = new GameObject[12];
    public GameObject projectilePrefab;

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
        //if character is attacking or defending animation, return
        if (gameObject.GetComponent<PlayerAnims>().lockInput) return;

        //spawning in chromosomes
        StartCoroutine(throwChromosomes());
    }

    protected override void SpcAttack()
    {
        //ranged attack
    }

    protected override void DefMove()
    {
        //reduce damage
    }

    IEnumerator throwChromosomes()
    {
        yield return new WaitForSeconds(.15f);

        //only shoot if the standard attack animation is playing
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Std_Attack"))
        {
            for (int i = -2; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    chromes[i+2 + j] = Instantiate(projectilePrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Quaternion(0, 0, 0, 0));
                    chromes[i+2 + j].transform.localScale = new Vector2(.1f, .1f);
                    chromes[i+2 + j].GetComponentInChildren<Projectile>().projectorIsP2 = isP2;
                    //pos i chromes move right, neg i chromes move left
                    if (i < 0) { chromes[i+2 + j].GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(j + 3, i + 25); }
                    else { chromes[i+2 + j].GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(-(j + 3), i + 25); }
                }
                //only 1 in 4 do damage
                chromes[i+2].GetComponentInChildren<Projectile>().dam = 1;
                yield return new WaitForSeconds(.05f);
            }
        }
    }
}
