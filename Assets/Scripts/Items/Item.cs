using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item
{
    private string _name;
    private string _description;
    private int _price;
    private Sprite _sprite;
    
    public string Name { get => _name; set => _name = value; }
    public string Description { get => _description; set => _description = value; }
    public int Price { get => _price; set => _price = value; }
    public Sprite Sprite { get => _sprite; set => _sprite = value; }
    public Item(string name, string description, int price, Sprite sprite)
    {
        this._name = name;
        this._description = description;
        this._price = price;
        this._sprite = sprite;
    }
}
