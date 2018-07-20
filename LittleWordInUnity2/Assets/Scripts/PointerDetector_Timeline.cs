using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerDetector_Timeline : MonoBehaviour {

	public void PointerEnter()
    {
        StatsManager.instance.currentArea = StatsManager.TIMELINE;
    }

    public void PointerExit()
    {
        StatsManager.instance.currentArea = StatsManager.GAME_AREA;
    }
}
