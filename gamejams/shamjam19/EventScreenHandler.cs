using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class EventScreenHandler : MonoBehaviour
{
    public Button event_A;
    public Button event_B;
    public Text event_A_text;
    public Text event_B_text;
    public Text event_name;
    public Text description;
    public Image img;
    public EventData.EventDatum datum;
    public VideoPlayer vidplayer;

    void Start()
    {
        datum = EventData.data.GetEvent();
        img.sprite = Resources.Load<Sprite>(datum.img_path);
        event_A.onClick.AddListener(OnClickEventA);
        event_A_text = event_A.GetComponentInChildren<Text>();
        event_A_text.text = datum.option_1;
        event_B.onClick.AddListener(OnClickEventB);
        event_B_text = event_B.GetComponentInChildren<Text>();
        event_B_text.text = datum.option_2;
        UpdateName();
        UpdateDescription();
    }

    void OnClickEventA()
    {
        EventChoice(1);
    }

    void OnClickEventB()
    {
        EventChoice(2);
    }

    void EventChoice(int i)
    {
        description.text = (i == 1) ? datum.result_1 : datum.result_2;
        event_A.onClick.RemoveListener(OnClickEventA);
        event_B.onClick.RemoveListener(OnClickEventB);
        ChangeEventButtons(i);
        event_B.onClick.AddListener(BackButton);
    }

    void ChangeEventButtons(int i)
    {
        var stat_inc = (i == 1) ? datum.stat_1_inc : datum.stat_2_inc;
        var stat_dec = (i == 1) ? datum.stat_1_dec : datum.stat_2_dec;
        var stat_value_inc = (i == 1) ? datum.stat_value_1_inc : datum.stat_value_2_inc;
        var stat_value_dec = (i == 1) ? datum.stat_value_1_dec : datum.stat_value_2_dec;
        AdjustModifiers(stat_inc, stat_value_inc);
        AdjustModifiers(stat_dec, stat_value_dec);
        event_A_text.text = stat_inc + " " + stat_value_inc.ToString() +
            ", " + stat_dec + " " + stat_value_dec.ToString();
        event_B_text.text = "Click to Continue";
    }

    void AdjustModifiers(string stat, int value)
    {
        if (stat == "Production")
        {
            PlayerData.data.production_mod += value;
        }
        if (stat == "Morality")
        {
            PlayerData.data.morality_mod += value;
        }
        if (stat == "Happiness")
        {
            PlayerData.data.happiness_mod += value;
        }
    }

    void UpdateName()
    {
        event_name.text = datum.name;
    }

    void UpdateDescription()
    {
        description.text = datum.description;
    }

    void BackButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScreen");
    }
}
