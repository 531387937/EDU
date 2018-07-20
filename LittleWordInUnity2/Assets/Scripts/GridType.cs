using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridType : MonoBehaviour {

    public enum Type
    {
        Flat_Ground,
        High_Ground,
        Obstacle,
        Box,
        Home,
        Home_Push,
        Higher_Ground,
        Hightest_Ground,
        Collection,
        Flower,
        Case,
        Uncertain
    };

    public Type Grid_Type;

    public int type;

    public int num;

    private void Start()
    {
        type = (int)Grid_Type;
    }
}
