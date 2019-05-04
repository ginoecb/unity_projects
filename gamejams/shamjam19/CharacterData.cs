using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Data for each individual character
public class CharacterData
{
    public string name;
    public string bio;
    public string img_path;
    public Dictionary<string, Stat> stats;
    public int num_inspected;
    public bool inspected;
    public bool sacrificed;
    public bool dead;

    // For controlling each stat's visibility
    public class Stat
    {
        public bool revealed;
        public string name;
        public int data;
        public string description;

        // Constructor
        public Stat()
        {
            revealed = false;
            name = "";
            data = 0;
            description = "";
        }
    }

    // Variable for empty stat data
    private Stat empty = new Stat()
    {
        revealed = true,
        name = "",
        data = 0,
        description = ""
    };

    // Generates new character stats 
    void GenerateStats()
    {
        stats["Production"] = new Stat();
        stats["Morality"] = new Stat();
        stats["Happiness"] = new Stat();
    }

    // Reveals one of the character's stats
    public void Inspect()
    {
        // You can only inspect a character 3 times, once for each stat
        if (num_inspected < 3 && PlayerData.data.num_inspect > 0)
        {
            string[] stat_names = new string[] { "Production", "Morality", "Happiness" };
            List<int> not_revealed = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                if (!stats[stat_names[i]].revealed)
                {
                    not_revealed.Add(i);
                }
            }
            int rand = Random.Range(0, not_revealed.Count - 1);
            stats[stat_names[not_revealed[rand]]].revealed = true;
            num_inspected += 1;
            PlayerData.data.num_inspect -= 1;
        }
    }

    // Sacrifices this character to our lord, Cthulhu
    public void Sacrifice()
    {
        if (!PlayerData.data.sacrificed_this_round && !dead)
        {
            stats["Production"] = empty;
            stats["Morality"] = empty;
            stats["Happiness"] = empty;
            sacrificed = true;
            dead = true;
            PlayerData.data.sacrificed_this_round = true;

            PlayerData.data.StartNextRound();
        }
    }



    // Constructor
    public CharacterData(string in_name, string in_bio, string in_img_path)
    {
        name = in_name;
        bio = in_bio;
        img_path = "CharacterPortraits/" + in_img_path;
        stats = new Dictionary<string, Stat>();
        GenerateStats();
        num_inspected = 0;
        inspected = false;
        sacrificed = false;
        dead = false;
    }
}
