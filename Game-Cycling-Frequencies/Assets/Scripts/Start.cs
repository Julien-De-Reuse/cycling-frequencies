using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Start : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1); // Load the main scene
    }
    public void QuitGame()
    {
        Application.Quit(); // Quit the application
        Debug.Log("Game is exiting..."); // Log to console for debugging
    }

}
