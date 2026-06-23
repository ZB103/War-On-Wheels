using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStats : MonoBehaviour
{
    public GameObject textbox;
    CharStats cs;
    string str;
    // Start is called before the first frame update
    void Start()
    {
        //textbox = GameObject.Find("Textbox");
        cs = gameObject.GetComponent<CharStats>();

        str = "{ " + cs.charName + " }\n"
            + "maxHP - " + cs.maxHP + "\n"
            + "dam - " + cs.dam + "\n"
            + "speed - " + cs.speed + "\n"
            + "primary - " + cs.primary + ": " + cs.primaryDesc + "\n"
            + "special - " + cs.special + ": " + cs.specialDesc + "\n"
            + "defensive - " + cs.defensive + ": " + cs.defensiveDesc + "\n"
            + cs.blurb;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            textbox.GetComponent<TextMeshProUGUI>().text = str;
        }
    }
}
