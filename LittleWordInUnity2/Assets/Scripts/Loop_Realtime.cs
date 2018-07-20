using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop_Realtime : MonoBehaviour {

    [SerializeField]
    GameObject Realtime;
	// Use this for initialization
	void Start () {
        Realtime.SetActive(false);
	}
	
    public void MouseDrop()
    {
        Realtime.SetActive(true);
    }

}
