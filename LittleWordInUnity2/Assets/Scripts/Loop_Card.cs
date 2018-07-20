using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop_Card : MonoBehaviour {

    // Use this for initialization
  
    [SerializeField]
    GameObject Choose_Box;
    [SerializeField] Thistime thisTime;


  
   

    void Start () {
        Choose_Box.SetActive(false);
        
    }
	
 
	// Update is called once per frame


    public void OnIfClick()
    {
        Choose_Box.SetActive(true);
    }

    public void Choose1()
    {
        thisTime.times = 0;
        Choose_Box.SetActive(false);
    }

    public void Choose2()
    {
        thisTime.times = 1;
        Choose_Box.SetActive(false);
    }

    public void Choose3()
    {
       thisTime.times = 2;
        Choose_Box.SetActive(false);
    }

    public void Choose4()
    {
        thisTime.times = 3;
        Choose_Box.SetActive(false);
    }

    public void Choose5()
    {
        thisTime.times = 4;
        Choose_Box.SetActive(false);
    }
}
