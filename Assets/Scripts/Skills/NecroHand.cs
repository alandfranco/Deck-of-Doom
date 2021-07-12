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
    public GameObject targetPoint;

    GameObject plM;

    public float dmg;
    public float dmgInExplotion;

    public float speed;

    float duration;

    public Transform myEffectPos;

    public GameObject explotion;

    protected override void Awake()
    {
        base.Awake();
        plM = FindObjectOfType<PlayerMovement>().gameObject;        
    }

    private void Initialize()
    {   
        Activate();
        target = targetPoint.transform.position;
        this.gameObject.transform.position = pl.transform.position;
    }

    public override void Skill()
    {
        Initialize();
        duration = Vector3.Distance(target, plM.transform.position) / speed;
        if (duration >= cooldownTime)
            duration = cooldownTime - 0.5f;
        Sequence sq = DOTween.Sequence()
            .Insert(0, plM.transform.DOLookAt(new Vector3(target.x,plM.transform.position.y,target.z), 0.2f))
            .Insert(0, transform.DOMove(target, duration))
            .AppendCallback(() => Explode());
    }

    protected override void Activate()
    {
        base.Activate();
        this.transform.position = myEffectPos.position;
    }

    void Explode()
    {
        if(FindObjectOfType<PlayerPassives>() != null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 5f + (5 * PlayerPassives.instance.skillAndCardBonus));
            foreach (var item in hitColliders)
            {
                if (item.GetComponent<Enemy>())
                {
                    item.GetComponent<TakeDamage>().TakeDamageToHealth(dmgInExplotion + (dmgInExplotion * PlayerPassives.instance.skillAndCardBonus), plM);
                }
            }
            ObjectPooler.instance.GetPooledObject(explotion, this.transform.position);
            Deactivate();
        }
        else
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 5f);
            foreach (var item in hitColliders)
            {
                if (item.GetComponent<Enemy>())
                {
                    item.GetComponent<TakeDamage>().TakeDamageToHealth(dmgInExplotion, plM);
                }
            }
            ObjectPooler.instance.GetPooledObject(explotion, this.transform.position);
            Deactivate();
        }
       
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<Enemy>())
            c.GetComponent<TakeDamage>().TakeDamageToHealth(dmg + (dmg * PlayerPassives.instance.skillAndCardBonus), plM);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
