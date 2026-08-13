using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBarUpdate : MonoBehaviour
{
    public static CharStats p1Stats;
    public static CharStats p2Stats;
    public Slider p1HealthBar;
    public Slider p2HealthBar;
    public TextMeshProUGUI winsText;

    // Start is called before the first frame update
    void Start()
    {
        winsText.enabled = false;
        p1HealthBar.maxValue = p1Stats.maxHealth;
        p2HealthBar.maxValue = p2Stats.maxHealth;
        UpdateBars();
    }

    //Hurt function lowers player's health. Is called by std or spc attack script
    public void Hurt(int player, int dam)
    {
        if (player == 1)
        {
            p1Stats.health -= dam;
            if (p1Stats.health <= 0) { PresentWin(2, p2Stats); }
        }
        else if (player == 2)
        {
            p2Stats.health -= dam;
            if (p2Stats.health <= 0) { PresentWin(1, p1Stats); }
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

    void PresentWin(int winner, CharStats winnerStats)
    {
        //Halt char controls and play win/loss animations
        PlayerAnims.winner = winner;

        //Halt cooldown bars
        gameObject.GetComponent<CooldownBarUpdate>().StopCoroutine(gameObject.GetComponent<CooldownBarUpdate>().p1Fill);
        gameObject.GetComponent<CooldownBarUpdate>().StopCoroutine(gameObject.GetComponent<CooldownBarUpdate>().p2Fill);

        //Set text to display winner's name
        winsText.text = winnerStats.charName + " Wins!";

        //Set text colors to be winner's colors
        winsText.colorGradient = new VertexGradient(
            winnerStats.primColor,  //top left
            winnerStats.secColor,   //top right
            winnerStats.primColor,  //bottom left
            winnerStats.secColor    //bottom right
            );

        winsText.enabled = true;

        StartCoroutine(Countdown());
        IEnumerator Countdown() {
            float countdown = 8f;
            while (countdown > 0) { 
                countdown -= Time.deltaTime;
                yield return new WaitForEndOfFrame(); }
            SceneManager.LoadScene("CharSelectMenu");
        }
    }
}
