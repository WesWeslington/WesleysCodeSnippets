using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgePiece : MonoBehaviour
{
    [Tooltip("For the Parent of the bridge piece")]
   [SerializeField] private Transform SpawnSlot;

    public bool isEndPiece = false;

    //This is used for finding where to spawn the bridge objects
public Vector3 GetSpawnSlot()
    {
        return SpawnSlot.position;
    }
}
