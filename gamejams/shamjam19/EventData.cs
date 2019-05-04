using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EventData : MonoBehaviour
{
    public static EventData data;
    public List<EventDatum> event_list;

    // Ensures only one instance of EventData exists
    private void Awake()
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
    }

    // Load events from input text file
    void Start()
    {
        event_list = new List<EventDatum>();
        GetEventData("Assets/Resources/InputTextFiles/EventData.txt");
    }

    // Load events from input text file
    public void GetEventData(string path)
    {
        // Read from file
        StreamReader reader = new StreamReader(path);
        string text = "";
        while (true)
        {
            text = reader.ReadLine();
            // While there is stuff to read
            if (text != null)
            {
                // name, stat_name, stat_value, desc, opt_1, opt_2, img.name
                var datum = new EventDatum(text.Split('*'));
                event_list.Add(datum);
                ///civilians[i] = new CharacterData(values[0], values[1], values[2]);
            }
            // End of the file
            else
            {
                break;
            }
        }
    }

    // Return info for when a new event occurs
    public EventDatum GetEvent()
    {
        int idx = Random.Range(0, event_list.Count);
        EventDatum my_event = event_list[idx];
        // Events may only occur once per game
        event_list.RemoveAt(idx);
        return my_event;
    }

    // Destroys self
    public void ResetData()
    {
        Destroy(gameObject);
    }

    // Info for a single event
    public class EventDatum
    {
        public string name;
        public string description;
        public string option_1;
        public string option_2;
        public string stat_1_inc;
        public string stat_1_dec;
        public string stat_2_inc;
        public string stat_2_dec;
        public int stat_value_1_inc;
        public int stat_value_1_dec;
        public int stat_value_2_inc;
        public int stat_value_2_dec;
        public string result_1;
        public string result_2;
        public string img_path;

        // Constructor
        public EventDatum(string[] values)
        {
            name = values[0];
            description = values[1];
            option_1 = values[2];
            stat_1_inc = values[3];
            stat_value_1_inc = int.Parse(values[4]);
            stat_1_dec = values[5];
            stat_value_1_dec = int.Parse(values[6]);
            option_2 = values[7];
            stat_2_inc = values[8];
            stat_value_2_inc = int.Parse(values[9]);
            stat_2_dec = values[10];
            stat_value_2_dec = int.Parse(values[11]);
            result_1 = values[12];
            result_2 = values[13];
            img_path = values[14];
        }
    }
}
