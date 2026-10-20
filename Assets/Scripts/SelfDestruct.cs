using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//destroys object upon collision with floor/wall
public class SelfDestruct : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
            Destroy(gameObject.transform.parent.gameObject);
    }
}
