using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RpgAdventure
{
    public partial class Damageable : MonoBehaviour
    {
        public int maxHitPoints;
        [Range(0,360f)]
        public float hitAngle = 360f;

        public void ApplyDamage()
        {
            //Debug.Log("Applying dmg");
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(0.0f,0.0f,1.0f,0.5f);

            Vector3 rotatedForward = 
                Quaternion.AngleAxis(
                -hitAngle * 0.5f, transform.up) * 
                transform.forward;

            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                transform.up,
                rotatedForward,
                hitAngle,
                1.0f );
        }


#endif
    }
}

