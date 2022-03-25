using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RpgAdventure {


    public class InventoryManager : MonoBehaviour
    {
     //  public Dictionary<string,GameObject> inventory = new Dictionary<string,GameObject>();
       public List<InventorySlot> inventory = new List<InventorySlot>();
       
       public Transform inventoryPanel; 
        
       private int m_InventorySize;

        private void Awake()
        {
            m_InventorySize = inventoryPanel.childCount;
            CreateInventory(m_InventorySize);
        }


        private void CreateInventory(int size)
        {
            for (int i = 0; i < size; i++)
            {
                inventory.Add(new InventorySlot(i));
            }


        }
        public void OnItemPickup(ItemSpawner spawner)
        {
            AddItemFrom(spawner);
        }
        public void AddItemFrom(ItemSpawner spawner)
        {
            var inventorySlot = GetFreeSlot();
            if(inventorySlot == null)
            {
         
                return;
            }

            var item = spawner.itemPrefab;
            inventorySlot.Place(spawner.itemPrefab);
            inventoryPanel
                .GetChild(inventorySlot.index)
                .GetComponentInChildren<Text>().text = item.name;
            Destroy(spawner.gameObject);


        }

        private InventorySlot GetFreeSlot()
        {
            return inventory.Find(slot => slot.itemName == null);

        }

    }



}


