using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableParticule : MonoBehaviour
{
    ParticleSystem ps;
    public Transform parent;
    public bool hasParent;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        parent = this.gameObject.transform.parent;
        if (parent != null) hasParent = true;
    }

    private void Update()
    {
        if (!ps.IsAlive(true))
        {
            ps.Stop(true);
        }
        if (ps.isStopped && !hasParent)
        {
            transform.parent = parent;
            transform.position = parent.position;
            transform.rotation = parent.rotation;
        }
    }
}
