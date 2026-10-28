using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanMoves : Moves
{
    private GameObject[][] bolts = new GameObject[2][];
    public GameObject projectilePrefab;

    private void Start()
    {
        isP2 = gameObject.GetComponent<Moves>().isP2;
        bolts[0] = new GameObject[3];
        bolts[1] = new GameObject[3];
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

        //spawning in bolts
        StartCoroutine(throwBolts());
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

    IEnumerator throwBolts()
    {
        yield return new WaitForSeconds(.15f);

        //only shoot if the standard attack animation is playing
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Std_Attack"))
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    bolts[i][j] = Instantiate(projectilePrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Quaternion(0, 0, 0, 0));
                    bolts[i][j].transform.localScale = new Vector2(.15f, .15f);
                    bolts[i][j].GetComponentInChildren<Rigidbody2D>().gravityScale = 0;
                    bolts[i][j].GetComponentInChildren<Projectile>().projectorIsP2 = isP2;
                    bolts[i][j].GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(isP2? -(j+15) : j+15, (j-1)*2);
                }
                //only 1 in 3 do damage
                bolts[i][1].GetComponentInChildren<Projectile>().dam = 2;
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}
