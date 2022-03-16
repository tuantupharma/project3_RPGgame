﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure 
{



public class PlayerInput : MonoBehaviour
{
        private Vector3 m_Movement;
        private bool m_IsAttack;
        public Vector3 MoveInput
        {
            get { return m_Movement; }
        }
        public bool IsMoveInput
        {
            get 
            { 
                return !Mathf.Approximately(MoveInput.magnitude, 0);
            }
        }

        public bool IsAttack
        {
            get
            {
                return m_IsAttack;
            }
        }

    // Update is called once per frame
    void Update()
    {
            m_Movement.Set(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
                );

            if (Input.GetButtonDown("Fire1"))
            {
                m_IsAttack = true;
            }

    }
}
}