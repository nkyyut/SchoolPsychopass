using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
