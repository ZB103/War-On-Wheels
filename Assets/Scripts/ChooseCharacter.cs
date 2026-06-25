using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseCharacter : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject[] characters; //fix later: can't display two of same character
    private int charIndex;
    CharStats cs;
    string str;
    private int playerChoosing; //which player is currently selecting
    //character the player has chosen
    private int p1Selection = -1;
    private int p2Selection = -1;
    //on finalize screen, whether player has locked in choice
    private bool p1Ready = false;
    private bool p2Ready = false;
    //positions of objects on screen
    private Vector2 p1CharPos = new Vector2(-4.5f, -.4f);
    private Vector2 p2CharPos = new Vector2(5f, -.4f);
    private Vector2 p1TextPos = new Vector2(415f, 8f);
    private Vector2 p2TextPos = new Vector2(-545f, 8f);
    //text boxes
    public TextMeshProUGUI flexTextbox;
    public TextMeshProUGUI leftTextbox;
    public TextMeshProUGUI rightTextbox;
    public TextMeshProUGUI leftCtrlTextbox;
    public TextMeshProUGUI rightCtrlTextbox;

    // Start is called before the first frame update
    void OnEnable()
    {
        StaticData.prefabs = prefabs;
        charIndex = 0;
        P1Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerChoosing == 1)    //P1 selecting
        {
            //choose current character
            if (Input.GetKeyDown(KeyCode.E)) {
                p1Selection = charIndex;
                if (p2Selection == -1) { P2Select(); }
                else { FinalizeSelections(); }
            }
            //browse characters
            else if (Input.GetKeyDown(KeyCode.A))
            {
                charIndex++;
                UpdateSelectedChar(charIndex - 1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                charIndex--;
                UpdateSelectedChar(charIndex + 1);
            }
        }
        else if (playerChoosing == 2)   //P2 selecting
        {
            //choose current character
            if (Input.GetKeyDown(KeyCode.Return)) {
                p2Selection = charIndex;
                if (p1Selection == -1) { P1Select(); }
                else { FinalizeSelections(); }
            }
            //browse characters
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                charIndex++;
                UpdateSelectedChar(charIndex - 1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                charIndex--;
                UpdateSelectedChar(charIndex + 1);
            }
        }
        else if (playerChoosing == 3)   //finalize screen
        {
            //Lock in selection
            if (Input.GetKeyDown(KeyCode.E)) { p1Ready = true; }
            if (Input.GetKeyDown(KeyCode.Return)) { p2Ready = true; }
            if (p1Ready) { leftCtrlTextbox.text = "READY"; }
            if (p2Ready) { rightCtrlTextbox.text = "READY"; }
            if (p1Ready && p2Ready)
            {
                //transition to arena scene
                StaticData.P1Selection = (CharPrefab)p1Selection;
                StaticData.P2Selection = (CharPrefab)p2Selection;
                SceneManager.LoadScene("Arena");
            }

            //P1 changes mind
            if (p1Ready == false && Input.GetKeyDown(KeyCode.Q))
            {
                P1Select();
            }
            //P2 changes mind
            else if (p2Ready == false && Input.GetKeyDown(KeyCode.RightShift))
            {
                P2Select();
            }
        }
    }

    void UpdateSelectedChar(int prevIndex)
    {
        //remove old character
        if (prevIndex < 0) { prevIndex = characters.Length-1; }
        else if (prevIndex == characters.Length) { prevIndex = 0; }
        characters[prevIndex].SetActive(false);

        //set new character
        if (charIndex < 0) { charIndex = characters.Length - 1; }
        else if (charIndex == characters.Length) { charIndex = 0; }
        characters[charIndex].SetActive(true);
        
        cs = characters[charIndex].GetComponent<CharStats>();
        str = "{ " + cs.charName + " }\n"
            + "maxHP - " + cs.maxHP + "\n"
            + "dam - " + cs.dam + "\n"
            + "speed - " + cs.speed + "\n"
            + "primary - " + cs.primary + ": " + cs.primaryDesc + "\n"
            + "special - " + cs.special + ": " + cs.specialDesc + "\n"
            + "defensive - " + cs.defensive + ": " + cs.defensiveDesc + "\n"
            + cs.blurb;

        //update textboxes
        flexTextbox.text = str;
        if (playerChoosing == 1) { leftTextbox.text = "P1: " + cs.charName; }
        else if (playerChoosing == 2) { rightTextbox.text = "P2: " + cs.charName; }
    }

    void P1Select()
    {
        p1Selection = -1;
        playerChoosing = 1;

        //update textboxes
        flexTextbox.transform.localPosition = p1TextPos;
        rightTextbox.text = "";
        rightCtrlTextbox.text = "";
        leftCtrlTextbox.text = "[A] and [D] browse\n[E] select";

        //hide all chars but the first one selected and place in the P1 position
        foreach (GameObject g in characters)
        {
            g.SetActive(false);
            g.transform.parent.gameObject.transform.position = p1CharPos;
        }
        charIndex = 8;
        UpdateSelectedChar(charIndex);
    }

    void P2Select()
    {
        p2Selection = -1;
        playerChoosing = 2;

        //update textboxes
        flexTextbox.transform.localPosition = p2TextPos;
        leftTextbox.text = "";
        leftCtrlTextbox.text = "";
        rightCtrlTextbox.text = "[<-] and [->] browse\n[RETURN] select";

        //hide all chars but the first one selected and place in the P2 position
        foreach (GameObject g in characters)
        {
            g.SetActive(false);
            g.transform.parent.gameObject.transform.position = p2CharPos;
        }
        charIndex = 8;
        UpdateSelectedChar(charIndex);

    }

    void FinalizeSelections()
    {
        playerChoosing = 3;
        p1Ready = false;
        p2Ready = false;

        //update textboxes
        flexTextbox.text = "";
        leftTextbox.text = "P1: " + characters[p1Selection].GetComponent<CharStats>().charName;
        rightTextbox.text = "P2: " + characters[p2Selection].GetComponent<CharStats>().charName;
        leftCtrlTextbox.text = "[Q] change\n[E] finalize";
        rightCtrlTextbox.text = "[SHIFT] change\n[RETURN] ready";

        //show both characters
        characters[p1Selection].transform.parent.gameObject.transform.position = p1CharPos;
        characters[p1Selection].SetActive(true);
        characters[p2Selection].transform.parent.gameObject.transform.position = p2CharPos;
        characters[p2Selection].SetActive(true);
    }
}
