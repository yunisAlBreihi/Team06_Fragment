using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryShardLightUpTrigger : MonoBehaviour
{
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered == false && other.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            MemoryShardLightUp shardLightUp = other.gameObject.GetComponent<MemoryShardLightUp>();

            if (shardLightUp != null)
            {
                shardLightUp.LightUpShard();
            }
        }
    }
}
