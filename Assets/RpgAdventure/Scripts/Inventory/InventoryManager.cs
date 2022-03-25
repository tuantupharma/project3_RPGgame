﻿using System.Collections;
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

        public void OnItemPickup(ItemSpawner spawner)
        {
            AddItemFrom(spawner);
        }
        private void CreateInventory(int size)
        {
            for (int i = 0; i < size; i++)
            {
                inventory.Add(new InventorySlot(i));
                RegisterSlotHandler(i);
            }


        }

        private void RegisterSlotHandler(int slotIndex)
        {
            var slotBtn = inventoryPanel
                .GetChild(slotIndex)
                .GetComponent<Button>();
            slotBtn.onClick.AddListener(() =>
            {
                UseItem(slotIndex);


            });
        }
         private void UseItem(int slotIndex)
        {
          var inventorySlot =  GetSlotByIndex(slotIndex);
            if(inventorySlot.itemPrefab == null) { return; }

            PlayerController.Instance.UseItemFrom(inventorySlot);
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

        private InventorySlot GetSlotByIndex(int index)
        {
            return inventory.Find(slot => slot.index == index); 
        }




    }



}


