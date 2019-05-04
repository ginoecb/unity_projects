using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
    public PlayerController player;

    public Button back;
    public Button ranged_for_sale;
    public Button melee_for_sale;
    public Button random_for_sale;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        Time.timeScale = 0;

        // Button functionality
        ranged_for_sale.onClick.AddListener(OnClickRanged);
        melee_for_sale.onClick.AddListener(OnClickMelee);
        random_for_sale.onClick.AddListener(OnClickRandom);
        back.onClick.AddListener(OnClickBack);
    }

    // Ranged weapon for sale
    private void OnClickRanged()
    {
        print(ShopState.data.Ranged_selling.name);
        player.SendMessage("ResetWeapon", ShopState.data.Ranged_selling);
        ranged_for_sale.onClick.RemoveListener(OnClickRanged);
        OnPurchase(ShopState.data.Ranged_selling);
    }

    // Melee weapon for sale
    private void OnClickMelee()
    {
        print(ShopState.data.Melee_selling.name);
        player.SendMessage("ResetWeapon", ShopState.data.Melee_selling);
        melee_for_sale.onClick.RemoveListener(OnClickMelee);
        OnPurchase(ShopState.data.Melee_selling);
    }

    // Random weapon for sale
    private void OnClickRandom()
    {
        print(ShopState.data.Random_selling.name);
        player.SendMessage("ResetWeapon", ShopState.data.Random_selling);
        random_for_sale.onClick.RemoveListener(OnClickRandom);
        OnPurchase(ShopState.data.Random_selling);
    }

    // Back button
    private void OnClickBack()
    {
        Time.timeScale = 1;
        player.shop_active = false;
        Destroy(this.gameObject);
    }

    // Reduce health on purchase
    private void OnPurchase(WeaponState.Weapon weapon)
    {
        PlayerState.data.HP -= weapon.cost;
    }
}
