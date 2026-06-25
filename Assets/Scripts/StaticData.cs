using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharPrefab
{
    ASHFALL,
    COMMAN,
    ICE_QUEEN,
    JUN,
    MAISEY,
    PUP,
    RUSH,
    SHIVER,
    WYVERN,
}

public class StaticData : MonoBehaviour
{
    public static Object[] prefabs;
    public static CharPrefab P1Selection;
    public static CharPrefab P2Selection;
}
