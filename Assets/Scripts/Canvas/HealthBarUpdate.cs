using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUpdate : MonoBehaviour
{
    public static CharStats p1Stats;
    public static CharStats p2Stats;
    public Slider p1HealthBar;
    public Slider p2HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        p1HealthBar.maxValue = p1Stats.maxHealth;
        p2HealthBar.maxValue = p2Stats.maxHealth;
        UpdateBars();
    }

    //TEMPORARY for testing
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) { Hurt(1, 5); }
        if (Input.GetKeyDown(KeyCode.I)) { Heal(1, 5); }
        if (Input.GetKeyDown(KeyCode.J)) { Hurt(2, 5); }
        if (Input.GetKeyDown(KeyCode.K)) { Heal(2, 5); }
    }

    //Hurt function lowers player's health. Is called by std or spc attack script
    public void Hurt(int player, int dam)
    {
        if (player == 1)
        {
            p1Stats.health -= dam;
            if (p1Stats.health <= 0) { }    //P1Win();
        }
        else if (player == 2)
        {
            p2Stats.health -= dam;
            if (p2Stats.health <= 0) { }    //P2Win();
        }

        UpdateBars();
    }

    //Heal function lowers player's health. Is called by def move script
    public void Heal(int player, int rec)
    {
        if (player == 1)
        {
            p1Stats.health += rec;
            if (p1Stats.health > p1Stats.maxHealth) { p1Stats.health = p1Stats.maxHealth; }
        }
        else if (player == 2)
        {
            p2Stats.health += rec;
            if (p2Stats.health > p2Stats.maxHealth) { p2Stats.health = p2Stats.maxHealth; }
        }

        UpdateBars();
    }

    //Update health bars on UI
    void UpdateBars()
    {
        p1HealthBar.value = p1Stats.health;
        p2HealthBar.value = p2Stats.health;
    }
}
