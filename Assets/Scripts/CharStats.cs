using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    //basics
    public string charName;
    public string disability;
    public string blurb;
    //stats
    public int maxHP;
    public int currHP;
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
    void Start()
    {
        currHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
