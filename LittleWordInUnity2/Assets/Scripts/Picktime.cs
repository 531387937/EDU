using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picktime : MonoBehaviour {

    Thistime[] thistimeinchild;
    [SerializeField]Character character;
	// Use this for initialization
	
	
	// Update is called once per frame
	void Update () {

        thistimeinchild = GetComponentsInChildren<Thistime>();
        for (int i = 0; i <thistimeinchild.Length; i++)
        {
            character.times[i]=thistimeinchild[i].times;
        }

    }

}
