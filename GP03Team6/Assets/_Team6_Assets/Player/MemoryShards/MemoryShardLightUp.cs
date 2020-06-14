using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryShardLightUp : MonoBehaviour
{
    [SerializeField, Tooltip("Reference the memory shards here")] GameObject[] memoryShards;
    [SerializeField, Tooltip("The color which the Memory shards are gonna change to"), ColorUsage(true, true)] Color shardColor;

    int currentIndex = 0;

    public void LightUpShard()
    {
        memoryShards[currentIndex].GetComponent<Renderer>().material.SetColor("_EmissiveColor", shardColor);
        currentIndex += 1;
    }
}
