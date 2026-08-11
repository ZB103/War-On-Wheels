using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Moves : MonoBehaviour
{
    //script that controls health bars
    public static HealthBarUpdate healthScript;
    //script that controls cooldown bars
    public static CooldownBarUpdate cooldownScript;
    //stats script
    public static CharStats pStats;
    //whether p1 or p2
    public bool isP2;
    //whether is colliding with other player
    protected bool hasContact;

    private void Awake()
    {
        //scripts are static in Moves
        healthScript = GameObject.Find("CanvasController").GetComponent<HealthBarUpdate>();
        cooldownScript = GameObject.Find("CanvasController").GetComponent<CooldownBarUpdate>();
        pStats = gameObject.GetComponent<CharStats>();
        hasContact = false;
    }

    //the default attack funcs presume a close-range melee attack.
    //Chars w/ ranged attacks have override functions in NameMoves scripts
    protected virtual void StdAttack()
    {
        if (hasContact) { healthScript.Hurt(isP2 ? 1 : 2, (int)pStats.dam/2); }
    }

    protected virtual void SpcAttack()
    {
        //if cooldown is not max, skip
        if (pStats.cooldown == pStats.maxCooldown)
        {
            cooldownScript.UseCharge(isP2 ? 2 : 1);
            if (hasContact) { healthScript.Hurt(isP2 ? 1 : 2, pStats.dam); }
        }
    }

    //the default defense func presumes a healing def move.
    //Chars w/ other def moves have override functions in NameMoves scripts
    protected virtual void DefMove()
    {
        //if cooldown is not max, skip
        if (pStats.cooldown == pStats.maxCooldown)
        {
            cooldownScript.UseCharge(isP2 ? 2 : 1);
            if (hasContact) { healthScript.Heal(isP2 ? 2 : 1, (int)pStats.dam / 2); }
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") { hasContact = true; }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") { hasContact = false; }
    }
}