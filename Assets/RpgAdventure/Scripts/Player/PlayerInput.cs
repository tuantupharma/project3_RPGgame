﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure 
{



public class PlayerInput : MonoBehaviour
{
        [SerializeField] float distanceToInteractWithNpc = 2.0f;
        private Vector3 m_Movement;
        private bool m_IsAttack;
        private bool m_IsTalk;
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

        public bool IsTalk
        {
            get
            {
                return m_IsTalk;
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
            StartCoroutine(AttackAndWait());
        }

        private void HandleRightMouseBtnDown()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool hasHit = Physics.Raycast(ray, out RaycastHit hit);

            if (hasHit && hit.collider.CompareTag("QuestGiver"))
            {
                var distanceToTarget = (transform.position - hit.transform.position).magnitude;
                // other option   var distance = Vector3.Distance(transform.position, hit.transform.position);

                if (distanceToTarget <= distanceToInteractWithNpc)
                {
                    m_IsTalk = true;
                    
                }


            }
        }

        private IEnumerator AttackAndWait()
        {
            m_IsAttack = true;
            yield return new  WaitForSeconds(0.05f);
            m_IsAttack = false;
        }

}
}