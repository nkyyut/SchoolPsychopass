using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string NextSceneName;    //次のシーンを予約する

    public void ChangeNextScene()
    {
        SceneManager.LoadScene(NextSceneName);
    }
    public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
