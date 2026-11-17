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

    public void Attack()
    {
        StartCoroutine(DogJump());
    }

    public override void DefaultAnimation(int layer)
    {
        biteConnected = false;  //reset
        //if Jun moves, the dog moves too
        //fix later
        //if (junAnim.GetCurrentAnimation(0) == Animations.Forward || junAnim.GetCurrentAnimation(0) == Animations.Backward)
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            { Play(new(Animations.Dog_Run, false, new())); }
        else { Play(new(Animations.Dog_Idle, false, new())); }
        if (rb.velocity.y > 0) { Play(new(Animations.Dog_Jump, true, new())); }
        else if (rb.velocity.x != 0) { Play(new(Animations.Dog_Run, false, new())); }
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

        //if dog hits ground, stop y motion
        if (collision.tag == "Ground" || gameObject.transform.localPosition.y < -.33) { rb.velocity = new Vector2(rb.velocity.x, 0); }
        //if dog hits wall, stop x motion
        if (collision.tag == "Bounds") { rb.velocity = new Vector2(0, rb.velocity.y); }
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
        Vector2 startCoords = gameObject.transform.localPosition;

        //dog runs
        rb.velocity = new Vector2(isP2 ? -5 : 5, 0);
        yield return new WaitForSeconds(.5f);

        //dog jumps
        biteConnected = false;
        rb.velocity = new Vector2(isP2 ? -5:5, 15);
        yield return new WaitForSeconds(.1f);
        do
        {
            if (hasContact) biteConnected = true;
            rb.velocity = new Vector2(isP2 ? -5 : 5, rb.velocity.y - 1);
            yield return new WaitForEndOfFrame();
        } while (gameObject.transform.localPosition.y > startCoords.y);
        gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x, startCoords.y);
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(.5f);

        //dog returns to Jun's side
        sr.flipX = !sr.flipX;
        rb.velocity = new Vector2(isP2 ? 5 : -5, 0);
        if (!isP2) {while (gameObject.transform.localPosition.x > startCoords.x) { yield return new WaitForEndOfFrame(); } }
        else { while (gameObject.transform.localPosition.x < startCoords.x) { yield return new WaitForEndOfFrame(); } }
        rb.velocity = new Vector2(0, 0);
        sr.flipX = !sr.flipX;

        //determine hit
        
    }
}
