
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RpgAdventure
{

    public class ItemSpawner : MonoBehaviour
    {

        public GameObject itemPrefab;
        public LayerMask targetLayers;
        [Serializable]
        public class  pickupEvent : UnityEvent<ItemSpawner> { };
        public  pickupEvent onItemPickup;
        // Start is called before the first frame update
        void Awake()
        {
            Instantiate(itemPrefab, transform);
            Destroy(transform.GetChild(0).gameObject);

            onItemPickup.AddListener(FindObjectOfType<InventoryManager>().OnItemPickup);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(0!= (targetLayers.value & 1 << other.gameObject.layer))
            {
               onItemPickup.Invoke(this);
               // Destroy(gameObject);


            }


        }




    }



}
