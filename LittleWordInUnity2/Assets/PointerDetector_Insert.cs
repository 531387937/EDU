using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerDetector_Insert : MonoBehaviour {

    public void PointerEnter()
    {
        

        string[] pieces = gameObject.transform.name.Split('_');
        int insert_int = int.Parse(pieces[0]);
        StatsManager.instance.currentInsert = insert_int;
     

    }

    public void PointerExit()
    {
        
        StatsManager.instance.currentInsert = 0;
    }
}
