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
        Shiver = gameObject.transform.parent.gameObject;
        if (Shiver.GetComponent<ShiverMoves>().isP2) 
        { 
            sr.flipX = true;
            gameObject.transform.localPosition = new Vector2(-gameObject.transform.localPosition.x, gameObject.transform.localPosition.y);
        }
        else
        {
            sr.flipX = false;
        }
        hasContact = false;
    }

    public bool Attack()
    {
        //play animation
        Play(new(Animations.Claw, true, new()));

        //determine hit
        return hasContact;
    }

    public override void DefaultAnimation(int layer) { Play(new(Animations.EMPTY, false, new())); }

    private void OnTriggerEnter2D(Collider2D collision)
    {   //shiver compared to is the specific shiver gameobject declared above
        if (collision.tag == "Player")
        {
            //check for edge case: both players are Shiver
            if(collision.gameObject.name == Shiver.name)
            {
                if ((Shiver.GetComponent<ShiverMoves>().isP2 && !collision.GetComponent<ShiverMoves>().isP2) ||
                        (!Shiver.GetComponent<ShiverMoves>().isP2 && collision.GetComponent<ShiverMoves>().isP2))
                    { hasContact = true; }
            }
            else { hasContact = true; }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {   //shiver compared to is the specific shiver gameobject declared above
        if (collision.tag == "Player")
        {
            //check for edge case: both players are Shiver
            if (collision.gameObject.name == Shiver.name)
            {
                if ((Shiver.GetComponent<ShiverMoves>().isP2 && !collision.GetComponent<ShiverMoves>().isP2) ||
                        (!Shiver.GetComponent<ShiverMoves>().isP2 && collision.GetComponent<ShiverMoves>().isP2))
                { hasContact = false; }
            }
            else { hasContact = false; }
        }
    }
}
