using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LifeSteal : Skills
{
    [Header("Connections")]
    [SerializeField] private Animator animator = default;
    [Header("Visuals")]
    [SerializeField] private Renderer skinnedMesh = default;
    [SerializeField] private ParticleSystem particle = default;

    public Transform target;

    public float damage;

    private void Initialize()
    {
        target = FindObjectOfType<PlayerMovement>().transform;
        Activate();
    }

    public override void Skill()
    {
        Initialize();
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {   
        yield return new WaitForSeconds(1f);        
        Explode();
        yield return new WaitForSeconds(0f);        
        yield return new WaitForSeconds(1f);
        Explode();
        yield return new WaitForSeconds(0f);        
        yield return new WaitForSeconds(1f);
        Explode();
        yield return new WaitForSeconds(0f);
        Deactivate();
        yield break;
    }
     
    void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 5f);
        foreach (var item in hitColliders)
        {
            if (item.GetComponent<Enemy>())
            {
                item.GetComponent<TakeDamage>().TakeDamageToHealth(damage + (damage * PlayerPassives.instance.skillAndCardBonus), pl.gameObject);
                pl.GetComponent<TakeDamage>().Heal(damage + (damage * PlayerPassives.instance.skillAndCardBonus) / 2);
            }
        }
        //this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(visual.activeInHierarchy)
            this.transform.position = target.transform.position;
    }

    private void OnTriggerEnter(Collider c)
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
