using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public static SceneManager data;
    public string input;
    public CharacterData character;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;

    // Ensures only one instance of SceneManager exists
    void Awake()
    {
        if (data == null)
        {
            DontDestroyOnLoad(gameObject);
            data = this;
        }
        else if (data != this)
        {
            Destroy(gameObject);
        }
        // Ensures value is initialized
        input = "Greyling";
    }

    void Start()
    {
        // Ensures value is initialized
        character = PlayerData.data.civilians[0];
    }

    void ResetStatics()
    {
        
    }
}
