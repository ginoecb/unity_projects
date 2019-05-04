using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenHandler : MonoBehaviour
{
    public Button greyling;
    public Button morrow;
    public Button westerling;
    public Button pickman;
    public Text doomsday_clock;
    public Text inspect_count;
    public Slider production;
    public Slider morality;
    public Slider happiness;
    public Slider gods_love;
    public RawImage doomsday_counter;

    void Start()
    {
        // Check for Game Over state
        if (PlayerData.data.gods_love <= 0 ||
            PlayerData.data.production <= 0 ||
            PlayerData.data.morality <= 0 ||
            PlayerData.data.happiness <= 0 ||
            PlayerData.data.round_max - PlayerData.data.round <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }
        // Otherwise, handle buttons, text, and sliders
        greyling.onClick.AddListener(OnClickGreyling);
        morrow.onClick.AddListener(OnClickMorrow);
        westerling.onClick.AddListener(OnClickWesterling);
        pickman.onClick.AddListener(OnClickPickman);
        UpdateDoomsdayClock();
        UpdateInspectCount();
        UpdateProduction();
        UpdateMorality();
        UpdateHappiness();
        UpdateGodsLove();
        UpdateDoomsdayCounter();
    }

    void OnClickGreyling()
    {
        HouseButton("Greyling");
    }

    void OnClickMorrow()
    {
        HouseButton("Morrow");
    }

    void OnClickWesterling()
    {
        HouseButton("Westerling");
    }

    void OnClickPickman()
    {
        HouseButton("Pickman");
    }

    // Handles clicking on a house button
    void HouseButton(string housename)
    {
        SceneManager.data.input = housename;
        UnityEngine.SceneManagement.SceneManager.LoadScene("House");
    }

    void UpdateDoomsdayClock()
    {
        var num_days = PlayerData.data.round_max - PlayerData.data.round;
        doomsday_clock.text = (num_days > 1) ? (num_days).ToString() + " Days Remain" : (num_days).ToString() + " Day Remains";
    }

    void UpdateInspectCount()
    {
        inspect_count.text = "Searches left: " + PlayerData.data.num_inspect.ToString() + "/" + PlayerData.data.num_inspect_max.ToString();
    }

    void UpdateProduction()
    {
        PlayerData.data.GetStat("Production");
        production.value = PlayerData.data.production;
    }
    void UpdateMorality()
    {
        PlayerData.data.GetStat("Morality");
        morality.value = PlayerData.data.morality;
    }
    void UpdateHappiness()
    {
        PlayerData.data.GetStat("Happiness");
        happiness.value = PlayerData.data.happiness;
    }

    void UpdateGodsLove()
    {
        gods_love.value = PlayerData.data.gods_love;
    }

    void UpdateDoomsdayCounter()
    {
        doomsday_counter.texture = (Texture)Resources.Load("UIElements/Number_" + (PlayerData.data.round_max - PlayerData.data.round).ToString());
    }
}
