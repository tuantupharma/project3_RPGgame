using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RpgAdventure {
public class FixedUpdateFollow : MonoBehaviour
{
        public Transform toFollow;
    private void FixedUpdate()
    {
        transform.position = toFollow.position;
        transform.rotation = toFollow.rotation;
    }
}
}