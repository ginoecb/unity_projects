using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public Button start;

    void Start()
    {
        start.onClick.AddListener(Begin);
    }
    
    void Begin()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScreen");
    }
}
