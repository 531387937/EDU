using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerDetector_Pick : MonoBehaviour {

    public void PointerEnter()
    {
        StatsManager.instance.currentArea = StatsManager.PICK_AREA;
    }

    public void PointerExit()
    {
        StatsManager.instance.currentArea = StatsManager.GAME_AREA;
    }
}
