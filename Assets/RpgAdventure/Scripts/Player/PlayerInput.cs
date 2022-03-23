﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure 
{



    public class PlayerInput : MonoBehaviour
    {

        public static PlayerInput Instance {get {return s_Instance;} }
      //  [SerializeField] float distanceToInteractWithNpc = 2.0f;

        private static PlayerInput s_Instance;
        private Vector3 m_Movement;
        private bool m_IsAttack;
       
        private Collider m_OptionClickTarget;

        public Collider OptionClickTarget { get { return m_OptionClickTarget; } }
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

      

        private void Awake()
        {
            s_Instance = this;
        }

        // Update is called once per frame
        void Update()
    {
            m_Movement.Set(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
                );

            bool isLeftMouseClick = Input.GetMouseButtonDown(0);
            bool isRightMouseClick = Input.GetMouseButtonDown(1);

            if (isLeftMouseClick && !m_IsAttack)
            {
                HandleLeftMouseBtnDown();
            }
            if(isRightMouseClick && !m_IsAttack)
            {

                HandleRightMouseBtnDown();

            }
    }

        private void HandleLeftMouseBtnDown()
        {
            StartCoroutine(TriggerAttack());
        }

        private void HandleRightMouseBtnDown()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool hasHit = Physics.Raycast(ray, out RaycastHit hit);

            if (hasHit)
            {
                m_OptionClickTarget = hit.collider;
                StartCoroutine(TriggerOptionTarget(hit.collider));

            }
        }

        private IEnumerator TriggerOptionTarget(Collider other)
        {
            m_OptionClickTarget = other;
            yield return new WaitForSeconds(0.05f);
            m_OptionClickTarget = null;
        }
        private IEnumerator TriggerAttack()
        {
            m_IsAttack = true;
            yield return new  WaitForSeconds(0.05f);
            m_IsAttack = false;
        }

}
}