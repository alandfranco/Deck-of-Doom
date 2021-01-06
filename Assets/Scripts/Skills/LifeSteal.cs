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

    private void OnEnable()
    {
        target = FindObjectOfType<PlayerMovement>().transform;
    }

    public override void Skill()
    {     
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
        this.gameObject.SetActive(false);
        yield break;
    }
     
    void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 5f);
        foreach (var item in hitColliders)
        {
            if (item.GetComponent<Enemy>())
            {
                //Debug.Log("Te pegue ñery " + item.name, item);
            }
        }
        Debug.Log("Explote");
        //this.gameObject.SetActive(false);
    }

    private void Update()
    {
        this.transform.position = target.transform.position;
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<Enemy>())
            Debug.Log("Te pegue ñeri en movimiento");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
