using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    public Button back_reset;
    public Text back_reset_text;
    public Button menu;
    public Text description;

    void Start()
    {
        back_reset_text = back_reset.GetComponentInChildren<Text>();
        if (IsGameOver())
        {
            back_reset.onClick.AddListener(OnClickReset);
            back_reset_text.text = "Retry";
        }
        else
        {
            back_reset.onClick.AddListener(OnClickResume);
            back_reset_text.text = "Resume";
        }
        menu.onClick.AddListener(OnClickMenu);
        GetText();
    }

    void OnClickReset()
    {
        PlayerData.data.ResetData();
        EventData.data.ResetData();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScreen");
    }

    void OnClickResume()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScreen");
    }

    void OnClickMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    bool IsGameOver()
    {
        if (PlayerData.data.gods_love <= 0 ||
        PlayerData.data.production <= 0 ||
        PlayerData.data.morality <= 0 ||
        PlayerData.data.happiness <= 0 ||
        PlayerData.data.round_max - PlayerData.data.round <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void GetText()
    {
        if (!IsGameOver())
        {
            description.text = "Paused";
        }
        else if (PlayerData.data.gods_love <= 0)
        {
            description.text = "God does not love you";
        }
        else if (PlayerData.data.production <= 0)
        {
            description.text = "Production has ceased and your villagers have starved";
        }
        else if (PlayerData.data.morality <= 0)
        {
            description.text = "Your villagers turned on one another, and none were survived the night";
        }
        else if (PlayerData.data.happiness <= 0)
        {
            description.text = "Your village was unhappy, and everyone died";
        }
        else
        {
            description.text = "A winner is you!";
        }
    }
}
