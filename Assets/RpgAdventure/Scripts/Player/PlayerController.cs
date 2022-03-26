using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure { 

public class PlayerController : MonoBehaviour, IAttackAnimListener, IMessageReceiver
    {
       public static PlayerController Instance 
        { get { return s_Instance; } }

        [SerializeField] MeleeWeapon meleeWeapon;
        [SerializeField] float maxForwardSpeed = 8.0f;
        [SerializeField] float speed;
        [SerializeField] float rotationSpeed;
        [SerializeField] private float m_MaxRotationSpeed = 1200f;
        [SerializeField] private float m_MinRotationSpeed = 800f;
        [SerializeField] float gravity = 20f;
        public Transform attackHand;

        private static PlayerController s_Instance;
        private PlayerInput m_PlayerInput;
        CharacterController m_CharController;
        private Animator m_Animator;
        CameraController m_CameraController;
        private HudManager m_HudManager;
        private Quaternion m_TargetRotation;

        private float m_DersiredForwardSpeed;
        private float m_ForwardSpeed;
        private float m_VerticalSpeed;
        const float k_Acceleration = 20f;
        const float k_Deceleration = 35f;

        private readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        private readonly int m_HashMeleeAttack = Animator.StringToHash("MeleeAttack");


        //video 10 add player input
        //private Vector3 m_Movement;

        private void Awake()
        {
            m_CharController = GetComponent<CharacterController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            m_Animator = GetComponent<Animator>();
            m_CameraController = Camera.main.GetComponent<CameraController>();
            m_HudManager = FindObjectOfType<HudManager>();
            s_Instance = this;

            m_HudManager.SetMaxHealth(GetComponent<Damageable>().maxHitPoints);

         //   meleeWeapon.SetOwner(gameObject);
        }
        // Update is called once per frame
        void FixedUpdate()
    {
            ComputeForwardMovement();
            ComputeVerticalMovement();
            ComputeRotation();


            if (m_PlayerInput.IsMoveInput)
            {
                float rotationSpeed = Mathf.Lerp(
                    m_MaxRotationSpeed,
                    m_MinRotationSpeed,
                    m_ForwardSpeed/m_DersiredForwardSpeed);

                m_TargetRotation = Quaternion.RotateTowards(
                    transform.rotation,
                    m_TargetRotation,
                    rotationSpeed * Time.fixedDeltaTime);
                transform.rotation = m_TargetRotation;
            }

            m_Animator.ResetTrigger(m_HashMeleeAttack);
            if (m_PlayerInput.IsAttack)
            {
                m_Animator.SetTrigger(m_HashMeleeAttack);
                
            }
        


           

        }

        private void OnAnimatorMove()
        {
            Vector3 movement = m_Animator.deltaPosition;
            movement += m_VerticalSpeed * Vector3.up* Time.fixedDeltaTime;
            m_CharController.Move(movement);
        }
        // this method is called by animation event
        
        public void MeleeAttackStart()
        {
            if(meleeWeapon != null)
            {
                meleeWeapon.BeginAttack();
            }
           
        }
        // this method is called by animation event
        public void MeleeAttackEnd()
        {
            if(meleeWeapon != null)
            {
                meleeWeapon.EndAttack();
            }
           
        }
        public void OnReceiveMessage(MessageType type, object sender, object message)
        {
           if(type == MessageType.DAMAGE)
            {
                m_HudManager.SetHealth((sender as Damageable).CurrentHitPoints) ;
               
            }
        }


        public void UseItemFrom(InventorySlot slot)
        {
            if(meleeWeapon != null)
            {
                if(slot.itemPrefab.name == meleeWeapon.name) { return; }
                else
                {
                    Destroy(meleeWeapon.gameObject);
                }
            }

            meleeWeapon = Instantiate(slot.itemPrefab, transform)
                .GetComponent<MeleeWeapon>();
            meleeWeapon.GetComponent<FixedUpdateFollow>().SetFollowee(attackHand);
            meleeWeapon.name = slot.itemPrefab.name;
            meleeWeapon.SetOwner(gameObject);
        }
       
        private void ComputeVerticalMovement()
        {
            m_VerticalSpeed = -gravity;
        }

        private void ComputeForwardMovement()
        {
            Vector3 moveInput = m_PlayerInput.MoveInput.normalized;
            m_DersiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;

            float acceleration = m_PlayerInput.IsMoveInput ? k_Acceleration : k_Deceleration;
            m_ForwardSpeed = Mathf.MoveTowards(
                m_ForwardSpeed,
                m_DersiredForwardSpeed,
                Time.fixedDeltaTime* acceleration);

            
            m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);
        }

        private void ComputeRotation()
        {
            Vector3 moveInput = m_PlayerInput.MoveInput.normalized;

           
             Vector3 cameraDirection = Quaternion.Euler(
                 0,
                m_CameraController.PlayerCam.m_XAxis.Value,
                 0)*Vector3.forward;
            Quaternion targetRotation;
            if (Mathf.Approximately(Vector3.Dot(moveInput, Vector3.forward), -1.0f))
            {
                // lam nhan vat di chuyen quay mat ve phia camera
                targetRotation = Quaternion.LookRotation(-cameraDirection);
            }
            else
            {
                Quaternion movementRotation = Quaternion.FromToRotation(Vector3.forward, moveInput);
                 targetRotation = Quaternion.LookRotation(movementRotation * cameraDirection);

            }


            m_TargetRotation = targetRotation;

        }

       
    }


}