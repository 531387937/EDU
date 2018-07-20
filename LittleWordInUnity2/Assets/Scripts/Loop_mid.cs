using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop_mid : MonoBehaviour {

    List<GameObject> loopmid;
    // Use this for initialization
    void Awake () {
        loopmid = new List<GameObject>();
        foreach (Transform child in transform)
        {
            loopmid.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

    }
	
	// Update is called once per frame
	void Update () {
        
        for (int i=0;i<StatsManager.instance.list.Count;i++)
        {
            
             loopmid[i].SetActive(StatsManager.instance.list[i].isLoop);

            
        }
		
	}
}
