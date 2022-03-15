
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


namespace RpgAdventure {
public class BanditBehaviour : MonoBehaviour
{
        [SerializeField] float detectionRadius = 10f ;
        [SerializeField] float detectionAngle = 90f;
        [SerializeField] float timeTostopPursuit = 2.0f;
        [SerializeField] float timeToWaitOnPrusuit = 2f;
        private PlayerController m_Target;
        private NavMeshAgent m_NavMeshAgent;
        private Animator m_Animator;
        private float m_timeSinceLostTarget = 0f;
        private Vector3 m_OriginalPosition;
        private readonly int m_HashInPursuit = Animator.StringToHash("InPursuit");
        private readonly int m_HashNearBase = Animator.StringToHash("NearBase");


        private void Awake()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_Animator = GetComponent<Animator>();
            m_OriginalPosition = transform.position;
        }

        private void Update()
        {
          var target =  lookForPlayer();
            if (m_Target == null) 
            {
                if (target != null)
                {
                    m_Target = target;
                }
                


            }
            else
            {
                m_NavMeshAgent.SetDestination(m_Target.transform.position);
                m_Animator.SetBool("InPursuit", true);
                if (target == null)
                {
                    m_timeSinceLostTarget += Time.deltaTime;
                    if(m_timeSinceLostTarget >= timeTostopPursuit)
                    {
                        m_Target = null;
                        m_NavMeshAgent.isStopped = true;
                        m_Animator.SetBool(m_HashInPursuit, false);
                        StartCoroutine(WaitOnPursuit());
                       
                    }
                }
                else
                {
                    m_timeSinceLostTarget = 0;
                }

                
            }
            Vector3 toBase= m_OriginalPosition - transform.position;
            toBase.y = 0;
            m_Animator.SetBool(m_HashNearBase, toBase.magnitude < 0.01f);

        }

        private IEnumerator WaitOnPursuit()
        {
            yield return new WaitForSeconds(timeToWaitOnPrusuit);
            m_NavMeshAgent.isStopped = false;
            m_NavMeshAgent.SetDestination(m_OriginalPosition);
        }

        private PlayerController lookForPlayer()
        {
            if(PlayerController.Instance == null)
            {
                return null;
            }
            Vector3 enemyPosition = transform.position;
            Vector3 toPlayer = PlayerController.Instance.transform.position - enemyPosition;
            toPlayer.y = 0;

            if(toPlayer.magnitude <= detectionRadius)
            {

                // so sanh cos goc giua vec to forward =1, vector toplayer.normalized = 1,
                // thi tich vo huong vector3.dot la 1*1*cos(normalized, forward),
                // so voi cos 1/2goc nhin dectectionAngle. neu tich vo huong lon hon thi trong vung thay duoc
                if(Vector3.Dot(toPlayer.normalized,transform.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
                {

                    
                   return PlayerController.Instance;
                    
                }

            }
            
            return null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Color color = new Color(0,0,0.7f,0.4f);
            UnityEditor.Handles.color = color;
            Vector3 rotateForward = Quaternion.Euler(
                0,
                -detectionAngle*0.5f,
                0)*transform.forward;
            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                Vector3.up, rotateForward, 
                detectionAngle, 
                detectionRadius);
        }
#endif

    }

}