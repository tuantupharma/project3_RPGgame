using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public class Dissolve : MonoBehaviour
    {

        public float dissolveTime = 6.0f;

        // Start is called before the first frame update
        private void Awake()
        {
            dissolveTime += Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            if(Time.time >= dissolveTime)
            {
                Destroy(gameObject);
            }
        }
    }


}
