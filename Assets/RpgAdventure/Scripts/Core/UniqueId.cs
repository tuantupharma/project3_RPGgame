using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RpgAdventure
{
    public class UniqueId : MonoBehaviour
    {
        [SerializeField]
        private string m_uid = Guid.NewGuid().ToString();
        
        
        public string Uid { get { return m_uid; } }
    }
}

