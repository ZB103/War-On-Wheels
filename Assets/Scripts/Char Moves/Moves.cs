using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Moves : MonoBehaviour
{
    //script that controls health bars
    public static HealthBarUpdate healthScript;
    //script that controls cooldown bars
    public static CooldownBarUpdate cooldownScript;
    //whether p1 or p2
    public bool isP2;

    private void Awake()
    {
        //scripts are static in Moves
        healthScript = GameObject.Find("CanvasController").GetComponent<HealthBarUpdate>();
        cooldownScript = GameObject.Find("CanvasController").GetComponent<CooldownBarUpdate>();
    }

    protected virtual void StdAttack()
    {
        print("standard attack");
    }

    protected virtual void SpcAttack()
    {
        print("special attack");
    }

    protected virtual void DefMove()
    {
        print("def move");
    }
}