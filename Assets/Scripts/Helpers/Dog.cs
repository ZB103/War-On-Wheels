using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SHG.AnimatorCoder;

public class Dog : AnimatorCoder
{
    public GameObject Jun;
    private bool isP2;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private AnimatorCoder junAnim;
    private bool hasContact;    //in the moment
    private bool biteConnected; //in an attack
    private bool trackContact;  //enabled during period of time dog is jumping

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        sr = gameObject.GetComponent<SpriteRenderer>();
        isP2 = Jun.GetComponent<JunMoves>().isP2;
        rb = gameObject.GetComponent<Rigidbody2D>();
        Jun = gameObject.transform.parent.gameObject;
        junAnim = Jun.GetComponent<AnimatorCoder>();
        if (isP2)
        {
            sr.flipX = true;
            gameObject.transform.localPosition = new Vector2(-gameObject.transform.localPosition.x, gameObject.transform.localPosition.y);
        }
        else
        {
            sr.flipX = false;
        }
        hasContact = false;
        biteConnected = false;
        trackContact = false;
    }

    public bool Attack()
    {
        StartCoroutine(DogJump());

        return biteConnected;
    }

    public override void DefaultAnimation(int layer)
    {
        biteConnected = false;  //reset
        //if Jun moves, the dog moves too
        //fix later
        //if (junAnim.GetCurrentAnimation(0) == Animations.Forward || junAnim.GetCurrentAnimation(0) == Animations.Backward)
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            { Play(new(Animations.Dog_Run)); }
        else { Play(new(Animations.Dog_Idle)); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   //Jun compared to is the specific Jun gameobject declared above
        if (trackContact && collision.tag == "Player")
        {
            //check for edge case: both players are Jun
            if (collision.gameObject.name == Jun.name)
            {
                if ((Jun.GetComponent<JunMoves>().isP2 && !collision.GetComponent<JunMoves>().isP2) ||
                        (!Jun.GetComponent<JunMoves>().isP2 && collision.GetComponent<JunMoves>().isP2))
                { hasContact = true; }
            }
            else { hasContact = true; }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {   //Jun compared to is the specific Jun gameobject declared above
        if (trackContact && collision.tag == "Player")
        {
            //check for edge case: both players are Jun
            if (collision.gameObject.name == Jun.name)
            {
                if ((Jun.GetComponent<JunMoves>().isP2 && !collision.GetComponent<JunMoves>().isP2) ||
                        (!Jun.GetComponent<JunMoves>().isP2 && collision.GetComponent<JunMoves>().isP2))
                { hasContact = false; }
            }
            else { hasContact = false; }
        }
    }

    IEnumerator DogJump()
    {
        //dog runs
        rb.velocity = new Vector2(isP2 ? -2 : 2, 0);
        yield return new WaitForSeconds(.5f);

        //dog jumps
        trackContact = true;
        biteConnected = false;

        //determine if hit
        //while (rb.velocity.y > 0) { if (hasContact) { biteConnected = true; } }
        //trackContact = false;

    }
}
