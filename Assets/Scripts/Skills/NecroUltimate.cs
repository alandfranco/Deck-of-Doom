using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class NecroUltimate : Skills
{
    [Header("Connections")]
    [SerializeField] private Animator animator = default;
    [Header("Visuals")]
    [SerializeField] private Renderer skinnedMesh = default;
    [SerializeField] private ParticleSystem particle = default;

    GameObject plM;

    public Transform target;

    public float damage;

    public float duration;

    public GameObject bonePrefab;

    protected override void Awake()
    {
        base.Awake();
        plM = FindObjectOfType<PlayerMovement>().gameObject;
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

        Collider[] hitColliders = new Collider[1];
        if (FindObjectOfType<PlayerPassives>() != null)
            hitColliders = Physics.OverlapSphere(this.transform.position, 10 + (10 * PlayerPassives.instance.skillAndCardBonus)).Where(x => x.GetComponent<Enemy>()).ToArray();
        else
            hitColliders = Physics.OverlapSphere(this.transform.position, 10).Where(x => x.GetComponent<Enemy>()).ToArray();
        foreach (var item in hitColliders)
        {
            if (item.TryGetComponent<Enemy>(out var enemy))
            {
                if (FindObjectOfType<PlayerPassives>() != null)
                    item.GetComponent<TakeDamage>().TakeDamageToHealth(damage + (damage * PlayerPassives.instance.skillAndCardBonus), plM);
                else
                    item.GetComponent<TakeDamage>().TakeDamageToHealth(damage, plM);
                var bone = ObjectPooler.instance.GetPooledObject(bonePrefab, item.transform.position);
                bone.transform.parent = visual.transform;
                //bone.GetComponent<Animator>().Play("UnderGround");
                bone.transform.eulerAngles = new Vector3(-90, Random.Range(0, 360), 0);
                item.transform.parent = bone.GetComponentInChildren<Transform>();
                //HACER APARECER LOS HUESOS
                if (FindObjectOfType<PlayerPassives>() != null)
                    enemy.GetStuned(duration + (duration * PlayerPassives.instance.skillAndCardBonus));
                else
                    enemy.GetStuned(duration);
            }

        }
        if(hitColliders.Length <= 0)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 pos = Random.insideUnitCircle * 10;
                var bone = ObjectPooler.instance.GetPooledObject(bonePrefab, 
                    new Vector3(this.transform.position.x + pos.x, this.transform.position.y, this.transform.position.z + pos.y));
                bone.transform.parent = visual.transform;
                //bone.GetComponent<Animator>().Play("UnderGround");
                bone.transform.eulerAngles = new Vector3(-90, Random.Range(0, 360), 0);
            }            
        }
        StartCoroutine(DeactivateFX());
        //this.gameObject.SetActive(false);
    }

    IEnumerator DeactivateFX()
    {
        if (FindObjectOfType<PlayerPassives>() != null)
            yield return new WaitForSeconds(duration + (duration * PlayerPassives.instance.skillAndCardBonus));
        else
            yield return new WaitForSeconds(duration);
        foreach (var item in GetComponentsInChildren<Enemy>())
        {
            item.transform.parent = null;
        }
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
        Gizmos.DrawWireSphere(transform.position, 10f);
    }
}
