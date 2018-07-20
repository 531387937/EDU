using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerDetector_Slot : MonoBehaviour {
    [SerializeField]
    Thistime thistime;
	public void PointerEnter()
    {
      

        string[] pieces = gameObject.transform.name.Split('_');
        int slot_int = int.Parse(pieces[1]);
        StatsManager.instance.currentSlot = slot_int;
        Debug.Log("zhelihi" + slot_int);
    
    }

    public void PointerExit()
    {
        StatsManager.instance.currentSlot = 0;
     
    }
}
