using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardType : MonoBehaviour {

    public enum Type
    {
        Forward,
        Turn_Left,
        Turn_Right,
        Jump,
        Push,
        Loop,
        LoopStart,
        LoopEnd,
        If,
        Else,
        Empty,
        Fetch,
        Brew,
        Collect
    };

    public Type Card_Type;

    public int type;

    private void Start()
    {
        type = (int)Card_Type;
    }
}
