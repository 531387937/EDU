using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class If_Card : MonoBehaviour {

    [SerializeField]
    GameObject Choose_Box;
    [SerializeField]
    GameObject statesmanager;


    // Use this for initialization
    void Start()
    {
        Choose_Box.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnIfClick()
    {
        Choose_Box.SetActive(true);
    }

    public void ChooseFlower()
    {
        int pickedUpFrom = StatsManager.instance.currentSlot;
        statesmanager.SendMessage("IfCardTypeChangeToFlower", pickedUpFrom-1);
    }

    public void ChooseHoney()
    {
        int pickedUpFrom = StatsManager.instance.currentSlot;
        statesmanager.SendMessage("IfCardTypeChangeToHoney", pickedUpFrom-1);
    }
}
