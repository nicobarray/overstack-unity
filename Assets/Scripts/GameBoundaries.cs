using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoundaries : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            Pubsub.Publish(Pubsub.Type.GameOver, null);
        }
    }
}
