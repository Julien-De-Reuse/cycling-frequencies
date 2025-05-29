using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ToDead : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(2); // Load the main scene
    }

}
