using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Store : MonoBehaviour
{
    private Dictionary<int, Item> _items = new Dictionary<int, Item>();
    [SerializeField] private Sprite[] itemSprites;
    private int _lastSelectedItem;

    public static Action<string, string, int, Sprite> onPressItem;
    void Start()
    {
        InitDictionary();
    }

    void InitDictionary()
    {
        _items.Add(1, Create_items(0,"Red Orb", "Consumable. Adds 100xp to your current level (to reach next level, you need 10 red orbs)", 50));
        _items.Add(2, Create_items(1,"Blue Orb", "Consumable. Start your next run with increased max hp. Only one blue orb can be stored", 70));
        _items.Add(3, Create_items(2,"Green Orb", "Consumable. Can be used during game to heal wounds. Only one blue orb can be stored", 40));
        _items.Add(4, Create_items(3,"Bow", "Customization. Exclusive hunter bow to hit every target. " +
                                            "Currently unavailable (don't buy it, it will still remove 20000 orbs from you)", 20000));
    }

    Item Create_items(int id,string name, string description, int price)
    {
        var item = new Item(name, description, price, itemSprites[id]);
        return item;
    }

    public void GetItemDetails(int id)
    {
        var item = _items[id];
        _lastSelectedItem = id;
        onPressItem?.Invoke(item.Name, item.Description, item.Price, item.Sprite);
    }

    public void BuyItem()
    {
        var item = _items[_lastSelectedItem];
        var price = item.Price;
        if (price <= PlayerDataManager.Instance.Data.LightOrbs){
            PlayerDataManager.Instance.OnBoughtItem(_lastSelectedItem, item.Price);
            PlayerDataManager.Instance.SavePlayerData();
            UiManager.Instance.UpdateLightOrbs();
        }
    }
}
