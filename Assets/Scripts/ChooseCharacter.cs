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
    private Vector2 p1CharPos = new Vector2(-6.15f, -.7f);
    private Vector2 p2CharPos = new Vector2(6.15f, -.7f);
    private Vector2 p1TextPos = new Vector2(480f, 8f);
    private Vector2 p2TextPos = new Vector2(-655f, 8f);
    //text boxes
    public GameObject flexTextboxHolder;
    public TextMeshProUGUI blurbTextbox;
    public TextMeshProUGUI movesTextbox;
    public TextMeshProUGUI statsTextbox;
    public TextMeshProUGUI leftTextbox;
    public TextMeshProUGUI rightTextbox;
    public TextMeshProUGUI leftCtrlTextbox;
    public TextMeshProUGUI rightCtrlTextbox;
    //stat image dots
    public GameObject[] healthDots;
    public GameObject[] damDots;
    public GameObject[] speedDots;
    private Color disabledColor = new Color(.4f,.4f,.4f,1);
    private GameObject clone;
    //background image
    public GameObject bg;

    // Start is called before the first frame update
    void OnEnable()
    {
        StaticData.prefabs = prefabs;
        charIndex = 0;
        P1Select();
        clone = null;
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
        str = cs.primary + ": " + cs.primaryDesc + "\n\n"
        + cs.special + ": " + cs.specialDesc + "\n\n"
        + cs.defensive + ": " + cs.defensiveDesc + "\n\n";

        //update textboxes
        movesTextbox.text = str;
        blurbTextbox.text = cs.blurb;
        if (playerChoosing == 1) { leftTextbox.text = "P1: " + cs.charName; }
        else if (playerChoosing == 2) { rightTextbox.text = "P2: " + cs.charName; }

        //update statistics ranking dots
        int hRank, dRank, sRank;
        foreach (GameObject h in healthDots) { h.GetComponent<Image>().color = new Color(1, 1, 1, 1); }
        foreach (GameObject d in damDots) { d.GetComponent<Image>().color = new Color(1, 1, 1, 1); }
        foreach (GameObject s in speedDots) { s.GetComponent<Image>().color = new Color(1, 1, 1, 1); }
        //calculate health rank 50-100
        if (cs.maxHealth > 85) { hRank = 3; }
        else if (cs.maxHealth > 70) { hRank = 2; }
        else { hRank = 1; }
        if (hRank < 3) { healthDots[2].GetComponent<Image>().color = disabledColor; }
        if (hRank < 2) { healthDots[1].GetComponent<Image>().color = disabledColor; }

        //calculate damage rank 3-10
        if (cs.dam > 7) { dRank = 3; }
        else if (cs.dam > 5) { dRank = 2; }
        else { dRank = 1; }
        if (dRank < 3) { damDots[2].GetComponent<Image>().color = disabledColor; }
        if (dRank < 2) { damDots[1].GetComponent<Image>().color = disabledColor; }

        //calculate speed rank 2-10
        if (cs.speed > 6) { sRank = 3; }
        else if (cs.speed > 4) { sRank = 2; }
        else { sRank = 1; }
        if (sRank < 3) { speedDots[2].GetComponent<Image>().color = disabledColor; }
        if (sRank < 2) { speedDots[1].GetComponent<Image>().color = disabledColor; }

    }

    void P1Select()
    {
        p1Selection = -1;
        playerChoosing = 1;

        //delete clone
        try { Destroy(clone); }
        catch { }

        //update textboxes
        flexTextboxHolder.transform.localPosition = p1TextPos;
        rightTextbox.text = "";
        rightCtrlTextbox.text = "";
        leftCtrlTextbox.text = "[A] and [D] browse\n[E] select";
        statsTextbox.text = "Health:\n\nDamage:\n\nSpeed:";

        //update bg
        bg.SetActive(true);
        bg.GetComponent<SpriteRenderer>().flipX = true;

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

        //delete clone
        try { Destroy(clone); }
        catch { }

        //update textboxes
        flexTextboxHolder.transform.localPosition = p2TextPos;
        leftTextbox.text = "";
        leftCtrlTextbox.text = "";
        rightCtrlTextbox.text = "[<-] and [->] browse\n[RETURN] select";
        statsTextbox.text = "Health:\n\nDamage:\n\nSpeed:";

        //update bg
        bg.SetActive(true);
        bg.GetComponent<SpriteRenderer>().flipX = false;

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
        blurbTextbox.text = "";
        movesTextbox.text = "";
        statsTextbox.text = "";
        leftTextbox.text = "P1: " + characters[p1Selection].GetComponent<CharStats>().charName;
        rightTextbox.text = "P2: " + characters[p2Selection].GetComponent<CharStats>().charName;
        leftCtrlTextbox.text = "[Q] change\n[E] finalize";
        rightCtrlTextbox.text = "[SHIFT] change\n[RETURN] ready";
        foreach (GameObject h in healthDots) { h.GetComponent<Image>().color = new Color(1, 1, 1, 0); }
        foreach (GameObject d in damDots) { d.GetComponent<Image>().color = new Color(1, 1, 1, 0); }
        foreach (GameObject s in speedDots) { s.GetComponent<Image>().color = new Color(1, 1, 1, 0); }

        //IF CHARS ARE THE SAME, DUPLICATE
        if (p1Selection == p2Selection) {
            clone = Instantiate(characters[p2Selection].transform.parent.gameObject,
                    p1CharPos, new Quaternion(0, 0, 0, 0)); }

        //show both characters
        characters[p1Selection].transform.parent.gameObject.transform.position = p1CharPos;
        characters[p1Selection].SetActive(true);
        characters[p2Selection].transform.parent.gameObject.transform.position = p2CharPos;
        characters[p2Selection].SetActive(true);

        //update bg
        bg.SetActive(false);
    }
}
