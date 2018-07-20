using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeRealtime : MonoBehaviour {

   
    [SerializeField] Thistime thisTime;

    Image image;
    int realtime;

    [SerializeField]
    private Sprite[] times;

    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        switch (thisTime.times)
        {
            case 0:
                image.sprite = times[0];
                break;
            case 1:
                image.sprite = times[1];
                break;
            case 2:
                image.sprite = times[2];
                break;
            case 3:
                image.sprite = times[3];
                break;
            case 4:
                image.sprite = times[4];
                break;

        }
        
	}
}
