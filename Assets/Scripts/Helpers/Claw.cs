using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SHG.AnimatorCoder;

//the claaaaawwwe ^^^
public class Claw : AnimatorCoder
{
    public GameObject Shiver;
    private SpriteRenderer sr;
    private bool hasContact;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        sr = gameObject.GetComponent<SpriteRenderer>();
        Shiver = GameObject.Find("Sprite-Shiver");
        if (Shiver.GetComponent<ShiverMoves>().isP2) 
        { 
            sr.flipX = true;
            gameObject.transform.localPosition = new Vector2(-3.5f, -.5f);
        }
        else
        {
            sr.flipX = false;
            gameObject.transform.localPosition = new Vector2(3.5f, -.5f);
        }
        sr.color = new Color(1,1,1,0);  //hide claw to start
        hasContact = false;
    }

    public bool Attack()
    {
        //play animation
        sr.color = new Color(1, 1, 1, 1);
        Play(new(Animations.Claw, true, new()));

            //determine hit
            return hasContact;
    }

    public override void DefaultAnimation(int layer) { sr.color = new Color(1, 1, 1, 0); }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") { hasContact = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") { hasContact = false; }
    }
}
