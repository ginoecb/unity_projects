using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseHandler : MonoBehaviour
{
    public Button char_0;
    public Button char_1;
    public Button char_2;
    public Button char_3;
    public Text char_0_name;
    public Text char_1_name;
    public Text char_2_name;
    public Text char_3_name;
    public Button back;
    public Image crest;
    public Text inspect_count;

    public CharacterData[] characters; 

    // Start is called before the first frame update
    void Start()
    {
        characters = PlayerData.data.GetHouseByName(SceneManager.data.input);
        char_0.onClick.AddListener(OnClickChar0);
        char_1.onClick.AddListener(OnClickChar1);
        char_2.onClick.AddListener(OnClickChar2);
        char_3.onClick.AddListener(OnClickChar3);
        char_0.GetComponentInChildren<Text>().text = characters[0].name;
        char_1.GetComponentInChildren<Text>().text = characters[1].name;
        char_2.GetComponentInChildren<Text>().text = characters[2].name;
        char_3.GetComponentInChildren<Text>().text = characters[3].name;
        back.onClick.AddListener(BackButton);
        char_0.image.sprite = characters[0].dead ? Resources.Load<Sprite>("CharacterPortraits/dead") : Resources.Load<Sprite>(characters[0].img_path);
        char_1.image.sprite = characters[1].dead ? Resources.Load<Sprite>("CharacterPortraits/dead") : Resources.Load<Sprite>(characters[1].img_path);
        char_2.image.sprite = characters[2].dead ? Resources.Load<Sprite>("CharacterPortraits/dead") : Resources.Load<Sprite>(characters[2].img_path);
        char_3.image.sprite = characters[3].dead ? Resources.Load<Sprite>("CharacterPortraits/dead") : Resources.Load<Sprite>(characters[3].img_path);
        UpdateInspectCount();
    }

    void OnClickChar0()
    {
        CharacterButton(0);
    }

    void OnClickChar1()
    {
        CharacterButton(1);
    }

    void OnClickChar2()
    {
        CharacterButton(2);
    }

    void OnClickChar3()
    {
        CharacterButton(3);
    }

    void CharacterButton(int idx)
    {
        SceneManager.data.character = characters[idx];
        UnityEngine.SceneManagement.SceneManager.LoadScene("Character");
    }

    void BackButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScreen");
    }

    void UpdateInspectCount()
    {
        inspect_count.text = "Searches left: " + PlayerData.data.num_inspect.ToString() + "/" + PlayerData.data.num_inspect_max.ToString();
    }
}
