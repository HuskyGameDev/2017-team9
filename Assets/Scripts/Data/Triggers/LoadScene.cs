using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
    public void Trigger()
    {
        Debug.Log("Loading Scene ");
        SceneManager.LoadScene("PlayTesting");
    }
}
