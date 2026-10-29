using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int dam;
    public bool projectorIsP2;

    private void Awake()
    {
        dam = 0;    //default - to be changed upon projectile's initialization
        projectorIsP2 = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) { return; }
        //if shooter is P1 and hit is P2
        if (!projectorIsP2 && collision.gameObject.GetComponent<Moves>().isP2)  { 
            Moves.healthScript.Hurt(2, dam);
            Destroy(gameObject.transform.parent.gameObject);
        }
        //if shooter is P2 and hit is P1
        else if (projectorIsP2 && !collision.gameObject.GetComponent<Moves>().isP2) { 
            Moves.healthScript.Hurt(1, dam);
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
