using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClear: MonoBehaviour {

	//	public GameObject Db;
	public GameObject Db;
	public GameObject role;
    public AudioSource ClearA;
    private bool once = false;

    // Use this for initialization
    void Start () {
//		if(role.GetComponent<Role_MoveForward>().StageClear == true)
//		{
//			Db.transform.localScale = new Vector3 (1, 1, 0);
//
//		}
		//		Db.transform.Translate (739.0f, 15.0f, 0.0f);

	}

	// Update is called once per frame
	void Update () {
        if (role.GetComponent<Character>() != null)
        {
            if (role.GetComponent<Character>().clear == true && once == false)
            {
                StartCoroutine("Delay");
                once = true;
                //Db.transform.localPosition = new Vector3(6.0f, 7.0f, 0.0f);
                //for (float i = 1; i <= 2; i += 0.01f) {
                //	Db.transform.localScale = new Vector3 (i, i, 0);
                //	int SceneIndex = SceneManager.GetActiveScene().buildIndex;
                //	if (SceneIndex >= 5 + ScenceManage.StageSave)

                //PlayerPrefs.SetInt ("Stage", ScenceManage.StageSave);
                //Debug.Log (ScenceManage.StageSave);
                //Role_MoveForward.CantClick = true;

                //}
            }
        }
        if (role.GetComponent<Ant_Ctr>() != null)
        {
            if (role.GetComponent<Ant_Ctr>().clear == true && once == false)
            {
                print("FEFSDW");
                StartCoroutine("Delay");
                once = true;
                //for (float i = 1; i <= 2; i += 0.01f) {
                //	Db.transform.localScale = new Vector3 (i, i, 0);
                //	int SceneIndex = SceneManager.GetActiveScene().buildIndex;
                //	if (SceneIndex >= 5 + ScenceManage.StageSave)

                //PlayerPrefs.SetInt ("Stage", ScenceManage.StageSave);
                //Debug.Log (ScenceManage.StageSave);
                //Role_MoveForward.CantClick = true;

                //}
            }
        }
    }

//	void OnMouseDown(){
//		Debug.Log ("delete");
////		StartCoroutine(Wait());
//		Db.SetActive (false);
//		int SceneIndex = SceneManager.GetActiveScene().buildIndex;
//		Db.SetActive (false);
//		if (SceneIndex < 12) {
//			SceneManager.LoadScene (SceneIndex + 1);
//		}
//	}

	IEnumerator Wait() {
		
		Debug.Log("Before Waiting 2 seconds");
		yield return new WaitForSeconds(2);
		Debug.Log("After Waiting 2 Seconds");

		int SceneIndex = SceneManager.GetActiveScene().buildIndex;
//		Db.SetActive (false);
		Debug.Log(SceneIndex);
		if (SceneIndex < 12) {
			SceneManager.LoadScene (SceneIndex + 1);
		}
	}

    IEnumerator Delay()
    {
        Debug.Log("Before Waiting 2 seconds");
        yield return new WaitForSeconds(2);
        Debug.Log("After Waiting 2 Seconds");
        Db.transform.localPosition = new Vector3(6.0f, 7.0f, 0.0f);
        if (!ClearA.isPlaying)
        {
            ClearA.Play();
        }
    }
}
