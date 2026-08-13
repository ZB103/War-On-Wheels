using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBarUpdate : MonoBehaviour
{
    public static CharStats p1Stats;
    public static CharStats p2Stats;
    public Slider p1CooldownBar;
    public Slider p2CooldownBar;
    public Coroutine p1Fill;
    public Coroutine p2Fill;

    // Start is called before the first frame update
    void Start()
    {
        p1CooldownBar.maxValue = p1Stats.maxCooldown;
        p2CooldownBar.maxValue = p2Stats.maxCooldown;
        p1CooldownBar.value = 0;
        p2CooldownBar.value = 0;
        UpdateBars();
        p1Fill = StartCoroutine(FillChargeP1());
        p2Fill = StartCoroutine(FillChargeP2());
    }

    //Hurt function lowers player's health. Is called by std or spc attack script
    public void UseCharge(int player)
    {
        if (player == 1)
        {
            StopCoroutine(p1Fill);
            p1Stats.cooldown = 0;
            p1Fill = StartCoroutine(FillChargeP1());
        }
        else if (player == 2)
        {
            StopCoroutine(p2Fill);
            p2Stats.cooldown = 0;
            p2Fill = StartCoroutine(FillChargeP2());
        }
    }

    //Refill cooldown until reaching max
    IEnumerator FillChargeP1()
    {
        while (p1Stats.cooldown < p1Stats.maxCooldown)
        {
            UpdateBars();
            p1Stats.cooldown += (2 * p1Stats.speed * Time.deltaTime);
            yield return new WaitForSeconds(.1f);
        }
        p1Stats.cooldown = p1Stats.maxCooldown;
        UpdateBars();
        yield return null;
    }

    //Refill cooldown until reaching max
    IEnumerator FillChargeP2()
    {
        while (p2Stats.cooldown < p2Stats.maxCooldown)
        {
            UpdateBars();
            p2Stats.cooldown += (2 * p2Stats.speed * Time.deltaTime);
            yield return new WaitForSeconds(.1f);
        }
        p2Stats.cooldown = p2Stats.maxCooldown;
        UpdateBars();
        yield return null;
    }

    //Update health bars on UI
    void UpdateBars()
    {
        
        p1CooldownBar.value = p1Stats.cooldown;
        p2CooldownBar.value = p2Stats.cooldown;
    }
}
