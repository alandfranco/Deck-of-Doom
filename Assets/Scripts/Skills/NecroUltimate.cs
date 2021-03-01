using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroUltimate : Skills
{
    [Header("Connections")]
    [SerializeField] private Animator animator = default;
    [Header("Visuals")]
    [SerializeField] private Renderer skinnedMesh = default;
    [SerializeField] private ParticleSystem particle = default;

    GameObject pl;

    public Transform target;

    public float damage;

    public float duration;

    private void Awake()
    {
        pl = FindObjectOfType<PlayerMovement>().gameObject;
    }

    private void Initialize()
    {
        target = FindObjectOfType<PlayerMovement>().transform;
        Activate();
    }

    public override void Skill()
    {
        Initialize();
        this.transform.position = target.transform.position;
        Explode();
    }

    void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 5f);
        foreach (var item in hitColliders)
        {
            if (item.TryGetComponent<Enemy>(out var enemy))
            {
                item.GetComponent<TakeDamage>().TakeDamageToHealth(damage, pl);
                //HACER APARECER LOS HUESOS
                enemy.GetStuned(duration);
            }
        }
        StartCoroutine(DeactivateFX());
        //this.gameObject.SetActive(false);
    }

    IEnumerator DeactivateFX()
    {
        yield return new WaitForSeconds(duration);
        Deactivate();
        yield break;
    }

    private void Update()
    {       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
