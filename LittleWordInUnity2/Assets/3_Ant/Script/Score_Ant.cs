using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_Ant : MonoBehaviour {
    private int reality = 0;
    public int best_step;
    public GameObject END;
    static public bool run = true;
    // Use this for initialization
    public void Start()
    {
        reality = 0;

    }

    // Update is called once per frame
    void Update()
    {
        reality = Ant_Ctr.step;
        Debug.Log(Ant_Ctr.step);

        if (reality <= best_step)
            ;

        if (reality > best_step && reality <= best_step + 3)
        {
            END.transform.GetChild(2).gameObject.SetActive(false);
        }
        if (reality > best_step + 3)
        {

            END.transform.GetChild(1).gameObject.SetActive(false);
            END.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}
