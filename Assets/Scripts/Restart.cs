using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    // Start is called before the first frame update
    public void RestartLevel()
    {
        SceneManager.LoadScene("MainScene");
    }

    // Update is called once per frame
    public void ExitToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
