using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadSceneAfter()
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;
        //		Debug.Log(SceneIndex);

        SceneManager.LoadScene(SceneIndex + 1);

    }

    public void ReLoad()
    {
        //		Application.LoadLevel(Application.loadedLevelName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void MainScene()
    {
        SceneManager.LoadScene(0);

    }

    public void ToChoose()
    {
        SceneManager.LoadScene("ChooseStage");
        //back to the scene which can choose stages.
    }

    public void ToStages(string StageName)
    {
        SceneManager.LoadScene(StageName);
    }

}
