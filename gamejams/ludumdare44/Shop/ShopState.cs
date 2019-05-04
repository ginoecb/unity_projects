using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopState : MonoBehaviour
{
    public static ShopState data;
    public WeaponState.Weapon Ranged_selling;
    public WeaponState.Weapon Melee_selling;
    public WeaponState.Weapon Random_selling;

    // Ensures only one instance of ShopState exists
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

    // Get (3) weapons for shop to sell
    public void GetShopItems()
    {
        // Get local copies of all weapons
        List<WeaponState.Weapon> Ranged_copy = WeaponState.data.Ranged_Weapons;
        List<WeaponState.Weapon> Melee_copy = WeaponState.data.Melee_Weapons;
        // Select weapons
        Ranged_selling = GetItemInList(Ranged_copy);
        Melee_selling = GetItemInList(Melee_copy);
        Random_selling = GetRandomWeapon(Ranged_copy, Melee_copy);
    }

    // Returns (1) item from the list provided
    public WeaponState.Weapon GetItemInList(List<WeaponState.Weapon> list)
    {
        int i = Random.Range(0, list.Count - 1);
        WeaponState.Weapon weapon = list[i];
        list.RemoveAt(i);
        return weapon;
    }

    // Returns (1) item from on of the lists provided
    public WeaponState.Weapon GetRandomWeapon(List<WeaponState.Weapon> list_0, List<WeaponState.Weapon> list_1)
    {
        int i = Random.Range(0, 1);
        return (i == 0) ? GetItemInList(list_0) : GetItemInList(list_1);
    }
}
