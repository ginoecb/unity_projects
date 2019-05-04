using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CharacterHandler : MonoBehaviour
{
    public Button inspect;
    public Button sacrifice;
    public Button back;
    public Text c_name;
    public Text c_bio;
    public Text c_production;
    public Text c_morality;
    public Text c_happiness;
    public Text pTrait;
    public Text mTrait;
    public Text hTrait;
    public Text inspect_count;
    public Image portrait;
    public Scrollbar scrollbar;
    public GameObject characterpanel;
    public GameObject screen;

    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(BackButton);
        inspect.onClick.AddListener(InspectButton);
        sacrifice.onClick.AddListener(SacrificeButton);
        c_name.text = SceneManager.data.character.name;
        c_bio.text = SceneManager.data.character.dead ? "Once in life, now in death. And what a pretty corpse they make!" :
            (SceneManager.data.character.bio
            + " " +SceneManager.data.character.stats["Production"].description
            + " " + SceneManager.data.character.stats["Morality"].description
            + " " + SceneManager.data.character.stats["Happiness"].description);
        portrait.sprite = SceneManager.data.character.dead ? Resources.Load<Sprite>("CharacterPortraits/dead") : Resources.Load<Sprite>(SceneManager.data.character.img_path);
        scrollbar.value = 1;
        OnClickUpdate();
        UpdateInspectCount();
    }

    void OnClickUpdate()
    {
        if (SceneManager.data.character.stats["Production"].revealed)
        {
            c_production.text = "Production "+ SceneManager.data.character.stats["Production"].data.ToString();
            pTrait.text = SceneManager.data.character.stats["Production"].name.ToString();
        }
        if (SceneManager.data.character.stats["Morality"].revealed)
        {
            c_morality.text = "Morality " + SceneManager.data.character.stats["Morality"].data.ToString();
            mTrait.text = SceneManager.data.character.stats["Morality"].name.ToString();
        }
        if (SceneManager.data.character.stats["Happiness"].revealed)
        {
            c_happiness.text = "Happiness " + SceneManager.data.character.stats["Happiness"].data.ToString();
            hTrait.text = SceneManager.data.character.stats["Happiness"].name.ToString();
        }
        UpdateInspectCount();
    }

    void InspectButton()
    {
        SceneManager.data.character.Inspect();
        OnClickUpdate();
    }

    void SacrificeButton()
    {
        SceneManager.data.character.Sacrifice();
        OnClickUpdate();
    }

    void BackButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("House");
    }

    void UpdateInspectCount()
    {
        inspect_count.text = "Searches left: " + PlayerData.data.num_inspect.ToString() + "/" + PlayerData.data.num_inspect_max.ToString();
    }
}
