using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadArena : MonoBehaviour
{
    //character the player has chosen
    private GameObject p1Selection; //parent
    private GameObject p2Selection; //parent
    private GameObject p1Char;      //child (sprite)
    private GameObject p2Char;      //child (sprite)
    //positions of objects on screen
    private Vector2 p1CharPos = new Vector2(-5f, -2.25f);
    private Vector2 p2CharPos = new Vector2(5f, -2.25f);
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
        //add selected characters to scene
        p1Selection = (GameObject)Instantiate(StaticData.prefabs[(int)StaticData.P1Selection], p1CharPos, new Quaternion(0, 0, 0, 0));
        p2Selection = (GameObject)Instantiate(StaticData.prefabs[(int)StaticData.P2Selection], p2CharPos, new Quaternion(0, 0, 0, 0));
        p1Selection.transform.localScale = new Vector2(0.4f, 0.4f);
        p2Selection.transform.localScale = new Vector2(0.4f, 0.4f);
        p1Char = p1Selection.transform.GetChild(0).gameObject;
        p2Char = p2Selection.transform.GetChild(0).gameObject;
        p1Char.AddComponent<P1Controls>();
        p2Char.AddComponent<P2Controls>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
