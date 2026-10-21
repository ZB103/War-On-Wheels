using System.Collections;
using System.Collections.Generic;
using SHG.AnimatorCoder;
using Unity.VisualScripting;
using UnityEngine;

public class IceQueenMoves : Moves
{
    private GameObject[] salts = new GameObject[16];
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

        //spawning in salts
        StartCoroutine(throwSalt());
    }

    protected override void SpcAttack()
    {
        //shoot a fireball
        StartCoroutine(shootFireball());
    }

    protected override void DefMove()
    {
        //dodge
    }

    IEnumerator throwSalt()
    {
        yield return new WaitForSeconds(.15f);

        //only shoot if the standard attack animation is playing
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Std_Attack"))
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    salts[i+j] = Instantiate(projectilePrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Quaternion(0, 0, 0, 0));
                    salts[i+j].transform.localScale = new Vector2(.05f, .05f);
                    salts[i+j].GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(j + 10, i + 15);
                    salts[i+j].GetComponentInChildren<Projectile>().projectorIsP2 = isP2;
                }
                //only 1 in 3 do damage
                salts[i].GetComponentInChildren<Projectile>().dam = 1;
                yield return new WaitForSeconds(.05f);
            }
        }
    }

    IEnumerator shootFireball()
    {
        yield return new WaitForSeconds(.15f);

        //only shoot if the special attack animation is playing
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Spc_Attack"))
        {
            GameObject fireball = Instantiate(projectilePrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Quaternion(0, 0, 0, 0));
            fireball.transform.localScale = new Vector2(.5f, .5f);
            fireball.GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(45, 0);
            fireball.GetComponentInChildren<Projectile>().dam = 10;
            fireball.GetComponentInChildren<Projectile>().projectorIsP2 = isP2;
        }
    }
}
