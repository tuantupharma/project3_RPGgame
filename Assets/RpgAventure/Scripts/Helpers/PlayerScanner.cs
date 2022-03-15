using RpgAdventure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerScanner
{
    public float detectionRadius = 10f;
    public float detectionAngle = 90f;

    public PlayerController Detect(Transform detector)
    {
        if (PlayerController.Instance == null)
        {
            return null;
        }
        
        Vector3 toPlayer = PlayerController.Instance.transform.position - detector.position;
        toPlayer.y = 0;

        if (toPlayer.magnitude <= detectionRadius)
        {

            // so sanh cos goc giua vec to forward =1, vector toplayer.normalized = 1,
            // thi tich vo huong vector3.dot la 1*1*cos(normalized, forward),
            // so voi cos 1/2goc nhin dectectionAngle. neu tich vo huong lon hon thi trong vung thay duoc
            if (Vector3.Dot(toPlayer.normalized, detector.forward) > 
                Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {


                return PlayerController.Instance;

            }

        }

        return null;
    }


}

