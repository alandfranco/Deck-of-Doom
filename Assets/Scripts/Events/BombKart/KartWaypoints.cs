using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartWaypoints : MonoBehaviour
{
    public int index;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider c)
    {
        if(c.TryGetComponent<BombKart>(out var kart))
        {
            kart.WaypointPassed(index);
        }
    }
}
