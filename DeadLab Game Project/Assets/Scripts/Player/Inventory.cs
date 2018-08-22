using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public IList<Item> items;

    void Awake()
    {
        items = new List<Item>();
    }

    public Item GetItem(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (id == items[i].id)
            {
                return items[i];
            }
        }
        return null;
    }

    public Item GetItem(int id, ItemType type)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (id == items[i].id && type == items[i].type)
            {
                return items[i];
            }
        }
        return null;
    }

    public IList<Item> GetItemByType(ItemType type)
    {
        IList<Item> itemsByType = new List<Item>();
        for (int i = 0; i < items.Count; i++)
        {
            if (type == items[i].type)
            {
                itemsByType.Add(items[i]);
            }
        }
        return itemsByType;
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        item.transform.SetParent(transform);
        item.gameObject.SetActive(false);
        item.locked = true;
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public void RemoveItem(int id)
    {
        items.Remove(GetItem(id));
    }
}
