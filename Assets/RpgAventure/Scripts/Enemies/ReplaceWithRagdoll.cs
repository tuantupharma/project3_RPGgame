using System.Collections;
using UnityEngine;

namespace RpgAdventure
{
    public class ReplaceWithRagdoll : MonoBehaviour
    {

        public GameObject ragdollPrefab;
        public void Replace()
        {
          GameObject ragdollInstance = Instantiate(
              ragdollPrefab,
              transform.position,
              transform.rotation);

            // TODO iterate over all child of transform and copy position and rotation to the ragdoll
            Destroy(gameObject);




        }


    }
}