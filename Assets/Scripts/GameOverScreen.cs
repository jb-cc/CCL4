using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void Setup(){
        gameObject.SetActive(true);
    }

    public void RestartGame(){
        Debug.Log("Restarting game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu(){
        Debug.Log("Returning to main menu...");
        SceneManager.LoadScene("MainMenu");
    }
}
