
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


namespace RpgAdventure {
public class BanditBehaviour : MonoBehaviour
{
        public PlayerScanner playerScanner;
        [SerializeField] float timeTostopPursuit = 2.0f;
        [SerializeField] float timeToWaitOnPursuit = 2f;
        public float attackDistance = 1.1f;

        public bool hasFollowTarget { get { return m_FollowTarget != null; } }  

        private PlayerController m_FollowTarget;
        private EnemyController m_EnemyController;
        private Animator m_Animator;
        private float m_timeSinceLostTarget = 0f;
        private Vector3 m_OriginalPosition;
        private Quaternion m_OriginalRotation;

        private readonly int m_HashInPursuit = Animator.StringToHash("InPursuit");
        private readonly int m_HashNearBase = Animator.StringToHash("NearBase");
        private readonly int m_HashAttack = Animator.StringToHash("Attack");

        private void Awake()
        {
            m_EnemyController = GetComponent<EnemyController>();
            m_Animator = GetComponent<Animator>();
            m_OriginalPosition = transform.position;
            m_OriginalRotation = transform.rotation;
        }

        private void Update()
        {
            var detectedTarget = playerScanner.Detect(transform);
            bool hasDetectedTarget = detectedTarget != null;

            if (detectedTarget != null) { m_FollowTarget = detectedTarget; }
            if(hasFollowTarget)
            {
                AttackOrFollowTarget();
                if(hasDetectedTarget)
                { 
                    m_timeSinceLostTarget = 0;
                } 
                else
                { 
                    StopPursuit(); 
                }

            }

            CheckIfNearBase();

        }

        private void CheckIfNearBase()
        {
            Vector3 toBase = m_OriginalPosition - transform.position;
            toBase.y = 0;

            bool nearBase = toBase.magnitude < 0.01f;
            m_Animator.SetBool(m_HashNearBase, nearBase);

            if (nearBase)
            {
                Quaternion targetRotation = Quaternion.RotateTowards(
                    transform.rotation,
                    m_OriginalRotation,
                    360 * Time.deltaTime
                    );
                transform.rotation = targetRotation;

            }
        }

        private void StopPursuit()
        {
            m_timeSinceLostTarget += Time.deltaTime;
            if (m_timeSinceLostTarget >= timeTostopPursuit)
            {
                m_FollowTarget = null;
                m_Animator.SetBool(m_HashInPursuit, false);
                StartCoroutine(WaitOnPursuit());
            }
        }

        private void AttackOrFollowTarget()
        {
            Vector3 toTarget = m_FollowTarget.transform.position - transform.position;
            if (toTarget.magnitude <= attackDistance)
            {
                m_EnemyController.StopFollowTarget();
                //  m_Animator.ResetTrigger(m_HashAttack);
                m_Animator.SetTrigger(m_HashAttack);
                //  m_Animator.SetBool(m_HashInPursuit,false);
            }
            else
            {
                m_Animator.SetBool(m_HashInPursuit, true);
                m_EnemyController.FollowTarget(m_FollowTarget.transform.position);
            }
        }
        private IEnumerator WaitOnPursuit()
        {
            yield return new WaitForSeconds(timeToWaitOnPursuit);
            
            m_EnemyController.FollowTarget(m_OriginalPosition);

        }

      

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Color color = new Color(0,0,0.7f,0.4f);
            UnityEditor.Handles.color = color;
            Vector3 rotateForward = Quaternion.Euler(
                0,
                -playerScanner.detectionAngle*0.5f,
                0)*transform.forward;
            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up, rotateForward,
                playerScanner.detectionAngle,
                playerScanner.detectionRadius);

            UnityEditor.Handles.DrawSolidArc(
               transform.position,
               Vector3.up, rotateForward,
               360,
               playerScanner.meleeDetectionRadius);
        }
#endif

    }

}