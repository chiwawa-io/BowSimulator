using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Store : MonoBehaviour
{
    private Dictionary<int, Item> _items = new Dictionary<int, Item>();
    [SerializeField] private Sprite[] itemSprites;

    public static Action<string, string, int, Sprite> onPressItem;
    void Start()
    {
        InitDictionary();
    }

    void InitDictionary()
    {
        _items.Add(1, Create_items(0,"Red Orb", "Consumable. Adds 100xp to your current level (to reach next level, you need 10 red orbs)", 50));
        _items.Add(2, Create_items(1,"Blue Orb", "Consumable. Start your next run with increased max hp", 70));
        _items.Add(3, Create_items(2,"Green Orb", "Consumable. Can be used during game to heal wounds", 40));
        _items.Add(4, Create_items(3,"Bow", "Customization. Exclusive hunter bow to hit every target", 20000));
    }

    Item Create_items(int id,string name, string description, int price)
    {
        var item = new Item(name, description, price, itemSprites[id]);
        return item;
    }

    public void GetItemDetails(int id)
    {
        var item = _items[id];
        onPressItem?.Invoke(item.Name, item.Description, item.Price, item.Sprite);
    }
}
