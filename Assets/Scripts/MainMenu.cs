using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPressExit()
    {
        Application.Quit();
    }

    public void OnPressStart() {
        SceneManager.LoadScene("Game");
    }
}
