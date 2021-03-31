using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void LoadMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1, LoadSceneMode.Single);
    }   
}
