using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacter : MonoBehaviour
{
    public GameObject[] characters;
    private int charIndex;
    CharStats cs;
    public TextMeshProUGUI textbox;
    string str;

    // Start is called before the first frame update
    void Start()
    {
        charIndex = 0;
        textbox = gameObject.GetComponent<TextMeshProUGUI>();
        UpdateSelectedChar(8);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            charIndex++;
            UpdateSelectedChar(charIndex-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            charIndex--;
            UpdateSelectedChar(charIndex+1);
        }
    }

    void UpdateSelectedChar(int prevIndex)
    {
        //remove old character
        if (prevIndex < 0) { prevIndex = characters.Length-1; }
        else if (prevIndex == characters.Length) { prevIndex = 0; }
        characters[prevIndex].GetComponent<Transform>().localScale = new Vector2(1, 1);

        //set new character
        if (charIndex < 0) { charIndex = characters.Length - 1; }
        else if (charIndex == characters.Length) { charIndex = 0; }
        characters[charIndex].GetComponent<Transform>().localScale = new Vector2(1.5f, 1.5f);
        
        cs = characters[charIndex].GetComponent<CharStats>();
        str = "{ " + cs.charName + " }\n"
            + "maxHP - " + cs.maxHP + "\n"
            + "dam - " + cs.dam + "\n"
            + "speed - " + cs.speed + "\n"
            + "primary - " + cs.primary + ": " + cs.primaryDesc + "\n"
            + "special - " + cs.special + ": " + cs.specialDesc + "\n"
            + "defensive - " + cs.defensive + ": " + cs.defensiveDesc + "\n"
            + cs.blurb;
        textbox.text = str;
    }
}
