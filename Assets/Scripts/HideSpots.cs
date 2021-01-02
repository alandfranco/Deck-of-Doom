using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpots : MonoBehaviour
{
    public List<Transform> hideSpots = new List<Transform>();
    void Start()
    {
        foreach (var item in GetComponentsInChildren<Transform>())
        {
            hideSpots.Add(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
