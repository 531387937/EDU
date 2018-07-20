using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenceManage : MonoBehaviour {

	public static int StageSave;

    public int scence;
	// Use this for initialization
	public void Start () {
        PlayerPrefs.SetInt("Stage", 31);
        StageSave = PlayerPrefs.GetInt("Stage",0);

		Debug.Log (StageSave);
        //Application.LoadLevel(scence);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
