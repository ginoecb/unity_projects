using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Video;

public class PlayerData : MonoBehaviour
{
    public static PlayerData data;
    public VideoPlayer vidPlayer;
    public AudioSource audioSource;
    public int round;
    public int round_max = 5;
    public float production;
    public float morality;
    public float happiness;
    public int production_mod;
    public int morality_mod;
    public int happiness_mod;
    public float gods_love_raw;
    public float gods_love;
    public int num_inspect;
    public int num_inspect_max = 6;
    public bool sacrificed_this_round;
    public CharacterData[] civilians;
    public Dictionary<int, List<StatDescInfo>> production_desc;
    public Dictionary<int, List<StatDescInfo>> morality_desc;
    public Dictionary<int, List<StatDescInfo>> happiness_desc;
    public VideoPlayerHandler playerHandler;

    // Ensures only one instance of PlayerData exists
    private void Awake()
    {
        if (data == null)
        {
            DontDestroyOnLoad(gameObject);
            data = this;
            vidPlayer = this.GetComponent<VideoPlayer>();
            vidPlayer.Prepare();
        }
        else if (data != this)
        {
            Destroy(gameObject);
        }
    }

    // Set variables
    void Start()
    {
        round = 0;
        num_inspect = num_inspect_max;
        sacrificed_this_round = false;
        // Initialize 2D dictionary for stats
        production_desc = new Dictionary<int, List<StatDescInfo>>();
        morality_desc = new Dictionary<int, List<StatDescInfo>>();
        happiness_desc = new Dictionary<int, List<StatDescInfo>>();
        GetStatDescriptions("Assets/Resources/InputTextFiles/StatData.txt");
        // Initialize civilians array
        civilians = new CharacterData[16];
        GenerateCivilians();
        // Initialize God's Love
        gods_love_raw = 100;
        gods_love = 1;

        // TESTING

    }

    // Exactly what you think it is
    public void GetGodsLove()
    {
        // But seriously, it's calculated as follows ...
        // Treat God's love as a 100 point base stat
        // Find the reduction modifier
        float reduction = 36;
        string[] stat_names = new string[] { "Production", "Morality", "Happiness" };
        for (int stat_idx = 0; stat_idx < 3; stat_idx++)
        {
            for (int i = 0; i < 16; i++)
            {
                // Reduction magnitude reduced if stat is positive
                if (civilians[i].stats[stat_names[stat_idx]].data > 0)
                {
                    reduction -= civilians[i].stats[stat_names[stat_idx]].data;
                }
            }
        }
        // Reduce total by modifier
        gods_love_raw -= 12 + reduction * 2;
        // Value for slider must be a float
        gods_love = gods_love_raw / 100;
    }

    // Load stat descriptions from input text file
    void GetStatDescriptions(string path)
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
                // name, stat, value, description
				// In hindsight, this could have been done w/ readlines instead of parsing by '*'
                var values = text.Split('*');
                // Generate new data
                var new_stat_desc = new StatDescInfo()
                {
                    name = values[0],
                    description = values[3]
                };
                // Determine which list to add to
                var my_dict = production_desc;
                if (values[1] == "Morality")
                {
                    my_dict = morality_desc;
                }
                else if (values[1] == "Happiness")
                {
                    my_dict = happiness_desc;
                }
                // Add value to appropriate dictionary
                try
                {
                    my_dict[int.Parse(values[2])].Add(new_stat_desc);
                }
                catch (KeyNotFoundException)
                {
                    var new_list = new List<StatDescInfo>();
                    my_dict.Add(int.Parse(values[2]), new_list);
                    my_dict[int.Parse(values[2])].Add(new_stat_desc);
                }
            }
            // End of file
            else
            {
                break;
            }
        }
    }

    // Gets the total stat value of the specified key
    // Key should be "Production", "Morality", or "Happiness"
    int GetStatTotal(string key)
    {
        // Get stat values from civilians
        try
        {
            int total = 0;
            for (int i = 0; i < civilians.Length; i++)
            {
                total += civilians[i].stats[key].data;
            }
            return total;
        }
        catch (KeyNotFoundException)
        {
            Debug.Log("Invalid key");
            return 0;
        }
        // Remember to add global modifier onto total
    }

    // Assigns the total stat value of the specified key
    // With the global modifier
    public void GetStat(string key)
    {
        // Gets the total stat value of the specified key
        int stat_total = GetStatTotal(key);
        // Add in the global modifier
        if (key == "Production")
        {
            production = RawScore(stat_total, production_mod);
        }
        else if (key == "Morality")
        {
            morality = RawScore(stat_total, morality_mod);
        }
        else if (key == "Happiness")
        {
            happiness = RawScore(stat_total, happiness_mod);
        }
        else
        {
            print("Invalid name");
        }
    }

    // Formula for calculating scores
    float RawScore(int stat_total, int stat_mod)
    {
        // Additive bonus from civilian scores
        float raw = 0.5f + (0.5f * (stat_total) / 12);
        // Multiplicative bonus from event modifiers
        raw *= 100 + stat_mod;
        raw /= 100;
        return raw;
    }

    // Generate character data from input text file
    void GetCharacterData(string path)
    {
        // Read from file
        StreamReader reader = new StreamReader(path);
        int i = 0;
        string text = "";
        while (true)
        {
            text = reader.ReadLine();
            // While there is stuff to read
            if (text != null)
            {
                // name, bio, character_portrait_filename
                var values = text.Split('*');
                civilians[i] = new CharacterData(values[0], values[1], values[2]);
                i += 1;
            }
            // End of the file
            else
            {
                break;
            }
        }
    }

    // Randomly assigns stats to civilians
    // A predetermined list is used to ensure more balanced gameplay
    void AssignCivilianStats()
    {
        // Repeats (3) times, once for stat
        string[] stat_names = new string[] { "Production", "Morality", "Happiness" };
        for (int stat_idx = 0; stat_idx < 3; stat_idx++)
        {
            // Randomly assigns values from a list of -2, -1, 1, and 2
            // (4) of each value are assignable
            List<int> stat_values = new List<int>();
            // Repeats (4) times, 16 possible stats in the pool
            for (int j = 0; j < 4; j++)
            {
                stat_values.Add(-2);
                stat_values.Add(-1);
                stat_values.Add(1);
                stat_values.Add(2);
            }
            // Assigns values to each civilian
            for (int j = 0; j < 16; j++)
            {
                // Randomly assign one of the stats from the pool
                int idx = Random.Range(0, stat_values.Count);
                civilians[j].stats[stat_names[stat_idx]].data = stat_values[idx];
                // Determine which list to add to
                var stat_desc_list = production_desc[stat_values[idx]];
                if (stat_names[stat_idx] == "Morality")
                {
                    stat_desc_list = morality_desc[stat_values[idx]];
                }
                else if (stat_names[stat_idx] == "Happiness")
                {
                    stat_desc_list = happiness_desc[stat_values[idx]];
                }
                // Randomly assign a description depending on the stat's value
                int desc_idx = Random.Range(0, stat_desc_list.Count);
                civilians[j].stats[stat_names[stat_idx]].name = stat_desc_list[desc_idx].name;
                civilians[j].stats[stat_names[stat_idx]].description = stat_desc_list[desc_idx].description;
                // Remove the selected stat from the pool
                stat_values.RemoveAt(idx);
            }
            GetStat(stat_names[stat_idx]);
        }
    }

    // Creates 16 new civilians
    void GenerateCivilians()
    {
        // Creating character data for each civilian
        GetCharacterData("Assets/Resources/InputTextFiles/CharacterData.txt");
        // Assigning each civilian stat values
        AssignCivilianStats();
    }

    // When starting a new round
    public void StartNextRound()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MainScreen");
        if (round < round_max)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Event");
        }
        num_inspect = num_inspect_max;
        sacrificed_this_round = false;
        round += 1;
        GetGodsLove();
    }

    // Gets character data by housename
    // Housename should be "Greyling", "Morrow", "Westerling", or "Pickman"
    public CharacterData[] GetHouseByName(string housename)
    {
        CharacterData[] house_data = new CharacterData[4];
        // Get offset based on housename
        int offset = 0;
        if (housename == "Morrow")
        {
            offset = 4;
        }
        else if (housename == "Westerling")
        {
            offset = 8;
        }
        else if (housename == "Pickman")
        {
            offset = 12;
        }
        else if (housename != "Greyling")
        {
            Debug.Log("Invalid name");
        }
        // Found offset, now get relevant character data
        for (int i = offset, j = 0; j < 4; i++, j++)
        {
            house_data[j] = civilians[i];
        }
        return house_data;
    }

    // Gets the index from civilians for the head of the specified house
    // It is expected that civilians are ordered in decreasing order of inheritance
    // Housename should be "Greyling", "Morrow", "Westerling", or "Pickman"
    int GetHouseHeadIdx(string housename)
    {
        int idx = -1;
        // Get offset based on housename
        int offset = 0;
        if (housename == "Morrow")
        {
            offset = 4;
        }
        else if (housename == "Westerling")
        {
            offset = 8;
        }
        else if (housename == "Pickman")
        {
            offset = 12;
        }
        // Return an index of -1 if the name is invalid
        else if (housename != "Greyling")
        {
            Debug.Log("Invalid name");
            return idx;
        }
        // Query for living house members
        CharacterData[] house_data = GetHouseByName(housename);
        bool someoneIsAlive = false;
        for (int i = 0; i < 4; i++)
        {
            if (!house_data[i].dead)
            {
                idx = i;
                someoneIsAlive = true;
                break;
            }
        }
        // Return an index of the current living head of the house
        if (someoneIsAlive)
        {
            return idx + offset;
        }
        // Return an index of -1 if everyone in the house is dead
        else
        {
            return idx;
        }
    }

    // Destroys self
    public void ResetData()
    {
        Destroy(gameObject);
    }

    // Individual stat's unique name & description
    public class StatDescInfo
    {
        public string name;
        public string description;
    }
}
