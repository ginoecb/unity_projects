using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBranchHandler : MonoBehaviour
{
    // UI elements
    public Button SPD_0;
    public Button SPD_1;
    public Button SPD_2;
    public Button SPD_3;
    public Button SPD_4;

    public Button cooldown_1;
    public Button cooldown_2;
    public Button cooldown_3;

    public Button ATK_0;
    public Button ATK_1;
    public Button ATK_2;
    public Button ATK_3;
    public Button ATK_4;

    public Button knockback_1;
    public Button knockback_2;
    public Button knockback_3;

    public Button DEF_0;
    public Button DEF_1;
    public Button DEF_2;
    public Button DEF_3;
    public Button DEF_4;

    public Button armor_1;
    public Button armor_2;
    public Button armor_3;

    // Disable locked stats
    private void Start()
    {
        // Button collections
        List<Button> SPD_list = new List<Button>()
        {
            SPD_0,
            SPD_1,
            SPD_2,
            SPD_3,
            SPD_4
        };

        List<Button> cooldown_list = new List<Button>()
        {
            cooldown_1,
            cooldown_2,
            cooldown_3
        };

        List<Button> ATK_list = new List<Button>()
        {
            ATK_0,
            ATK_1,
            ATK_2,
            ATK_3,
            ATK_4
        };

        List<Button> knockback_list = new List<Button>()
        {
            knockback_1,
            knockback_2,
            knockback_3
        };

        List<Button> DEF_list = new List<Button>()
        {
            DEF_0,
            DEF_1,
            DEF_2,
            DEF_3,
            DEF_4
        };

        List<Button> armor_list = new List<Button>()
        {
            armor_1,
            armor_2,
            armor_3
        };

        // Lock unacquirable stats
        LockBranch(
            PlayerState.data.SPD_max,
            PlayerState.data.cooldown_max,
            SPD_list,
            cooldown_list
            );

        LockBranch(
            PlayerState.data.ATK_max,
            PlayerState.data.knockback_max,
            ATK_list,
            knockback_list
            );

        LockBranch(
            PlayerState.data.DEF_max,
            PlayerState.data.armor_max,
            DEF_list,
            armor_list
            );
    }

    // Locks all unqcuirable stats in a single branch
    private void LockBranch(
        int primary_max,
        int secondary_max,
        List<Button> primary,
        List<Button> secondary
        )
    {
        // Lock primary stats
        LockNodes(primary_max, primary);
        // Lock secondary stats
        LockNodes(secondary_max, secondary);
    }

    // Locks nodes that are not acquirable
    private void LockNodes(int max, List<Button> list)
    {
        for (int i = 0; i < max; i++)
        {
            Destroy(list[i].gameObject);
            ///list[i].onClick.RemoveAllListeners();
            ///list[i].GetComponentInChildren<Text>().text = "ACTIVE";
        }

        for (int i = max + 1; i < list.Count; i++)
        {
            list[i].onClick.RemoveAllListeners();
            list[i].GetComponentInChildren<Text>().text = "LOCKED";
        }
    }
}
