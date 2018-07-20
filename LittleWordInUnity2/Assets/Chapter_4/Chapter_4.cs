using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter_4 : MonoBehaviour {

    [SerializeField]
    GameObject beauty1;
    [SerializeField]
    int num1;

    [SerializeField]
    GameObject beauty2;
    [SerializeField]
    int num2;

    [SerializeField]
    GameObject beauty3;
    [SerializeField]
    int num3;

    [SerializeField]
    GameObject beauty4;
    [SerializeField]
    int num4;

    [SerializeField]
    GameObject beauty5;
    [SerializeField]
    int num5;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Collect(int num)
    {
        Debug.Log("collected");
        if(num==num1)
            beauty1.gameObject.SetActive(false);
        if (num == num2)
            beauty2.gameObject.SetActive(false);
        if (num == num3)
            beauty3.gameObject.SetActive(false);
        if (num == num4)
            beauty4.gameObject.SetActive(false);
        if (num == num5)
            beauty5.gameObject.SetActive(false);
    }
}
