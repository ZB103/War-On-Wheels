using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    //basics
    public string charName;
    public string disability;
    public string blurb;
    public bool canJump;
    //stats
    public int maxHealth;
    public int health;      //defeat when health reaches 0
    public int maxCooldown;
    public int cooldown;    //special attack can be used when cooldown reaches 0
    public int dam;
    public int speed;
    //primary attack
    public string primary;
    public string primaryDesc;
    //special attack
    public string special;
    public string specialDesc;
    //defensive move
    public string defensive;
    public string defensiveDesc;

    // Start is called before the first frame update
    void OnEnable()
    {
        health = maxHealth;
        cooldown = maxCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
