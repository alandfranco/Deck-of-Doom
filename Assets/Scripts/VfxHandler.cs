using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxHandler : MonoBehaviour
{
    public Animator anim;
    public GameObject[] allVfx = new GameObject[0];

    public int vfxNumber;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void StartVfx()
    {
        vfxNumber = anim.GetInteger("vfxNumber");

        if (allVfx[vfxNumber]!= null)
        {
            ReplayVfx();
        }
    }

    public void ReplayVfx()
    {        
        GameObject holder = allVfx[vfxNumber].gameObject;
        holder.SetActive(true);
        ParticleSystem[] particles = allVfx[vfxNumber].GetComponentsInChildren<ParticleSystem>();

        if (particles != null)
        {
            foreach (ParticleSystem particle in particles)
            {
                //particle.transform.parent = holder.transform;
                particle.transform.position = holder.transform.position;
                particle.transform.rotation = holder.transform.rotation;
                particle.transform.parent = null;
                particle.GetComponent<DisableParticule>().hasParent = false;
                particle.Play(true);
            }           
        }
    }

    public void CreateVfx(GameObject prefab, Transform position)
    {
        GameObject p = Instantiate(prefab);
        p.transform.localRotation = position.localRotation;
        p.transform.position = position.position;

        ParticleSystem[] particles = p.GetComponentsInChildren<ParticleSystem>();

        if (particles != null)
        {
            foreach (ParticleSystem particle in particles)
            {
                particle.Play(true);
            }
        }
    }
}
