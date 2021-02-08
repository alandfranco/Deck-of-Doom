using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NecroHand : Skills
{
    [Header("Connections")]
    [SerializeField] private Animator animator = default;
    [Header("Visuals")]
    [SerializeField] private Renderer skinnedMesh = default;
    [SerializeField] private ParticleSystem particle = default;

    public Vector3 target;

    GameObject pl;

    public float dmg;
    public float dmgInExplotion;

    private void OnEnable()
    {
        pl = FindObjectOfType<PlayerMovement>().gameObject;
        target = GameObject.Find("AimingSpot").transform.position;
    }

    public override void Skill()
    {        
        Sequence sq = DOTween.Sequence()
            .Insert(0, transform.DOMove(target, 3f))
            .AppendCallback(() => Explode());
    }

    void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 5f);
        foreach (var item in hitColliders)
        {            
            if(item.GetComponent<Enemy>())
            {
                item.GetComponent<TakeDamage>().TakeDamageToHealth(dmgInExplotion, pl);
            }            
        }
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<Enemy>())
            c.GetComponent<TakeDamage>().TakeDamageToHealth(dmg, pl);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
