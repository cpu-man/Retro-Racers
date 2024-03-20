using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadMenuScene : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadSceneAsync(1);
    }
    
    
}