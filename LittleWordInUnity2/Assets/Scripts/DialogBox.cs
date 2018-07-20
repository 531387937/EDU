using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogBox : MonoBehaviour {

//	public GameObject Db;
	public GameObject Db;


	// Use this for initialization
	void Start () {
			Db.transform.localScale = new Vector3 (1, 1, 0);
			Db.transform.localPosition = new Vector3 (-6.0f, -7.0f, 0.0f);

//		Db.transform.Translate (739.0f, 15.0f, 0.0f);

	}
	
	// Update is called once per frame
	void Update () {
		for (float i = 1; i <= 2; i += 0.01f) {
			Db.transform.localScale = new Vector3 (i, i, 0);
		}
	}

	public void DeleteDB(){
		Debug.Log ("delete");
		Db.SetActive (false);
	}

	IEnumerator Wait() {
		Debug.Log("Before Waiting 2 seconds");
		yield return new WaitForSeconds(2);
		Debug.Log("After Waiting 2 Seconds");
		int SceneIndex = SceneManager.GetActiveScene().buildIndex;
		Debug.Log(SceneIndex);
		if (SceneIndex < 12) {
			SceneManager.LoadScene (SceneIndex + 1);
		}
	}
}
