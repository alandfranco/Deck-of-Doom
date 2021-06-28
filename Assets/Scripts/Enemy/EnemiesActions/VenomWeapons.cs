using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VenomWeapons : GOAPAction
{
    bool poisoned;
    public List<Enemy> buffEnemies = new List<Enemy>();

    public float damageBonus;

    public float duration;

    public float range;

    public VenomWeapons()
    {
        addEffect("buffEnemy", true);
    }

    void Start()
    {
        StartCoroutine(PickTobuff());
    }

    void Update()
    {
        
    }

    public override void Reset()
    {
        //buffEnemies.Clear();
        poisoned = false;
        target = null;
    }

    public override bool IsDone()
    {
        //buffEnemies.Clear();
        return poisoned;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        if(buffEnemies.Count > 0 && !this.GetComponent<Enemy>().isDisable)
        {
            target = buffEnemies[0].gameObject;
            return true;
        }
        else
            return false;        
    }

    IEnumerator PickTobuff()
    {
        var posibleTarget = FindObjectsOfType<Enemy>().Where(x => !x.buffs.ContainsKey("Venom") 
        && Vector3.Distance(this.transform.position, x.transform.position) < range
        && x != this.GetComponent<Enemy>())
            .OrderBy(x => Random.value).Take(3).ToList();

        if (posibleTarget.Count > 0)
        {
            int _amount = 0;
            foreach (var item in posibleTarget)
            {
                var group = Physics.OverlapSphere(item.transform.position, 2f).Where(x => x.GetComponent<Enemy>()).Select(x => x.GetComponent<Enemy>()).ToList();
                _amount = group.Count > _amount ? group.Count : _amount;
                buffEnemies = group.Count == _amount ? group : buffEnemies;            
            }            
        }
        yield return new WaitForSeconds(0f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(PickTobuff());
        yield break;
    }

    public override bool Perform(GameObject agent)
    {
        Enemy currEnemy = agent.GetComponent<Enemy>();
        if (currEnemy.stamina >= (cost) && buffEnemies.Count > 0 && !currEnemy.isDisable)
        {
            Vector3 targetDirection = target.transform.position - this.transform.position;
            Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, targetDirection, 20 * Time.deltaTime, 0.0f);
            this.transform.rotation = Quaternion.LookRotation(newDirection);

            currEnemy.agent.isStopped = true;
            currEnemy.anim.Play("Cast1");            
            foreach (var item in buffEnemies)
            {
                if(!item.buffs.ContainsKey("Venom"))
                    item.AddBuff("Venom", damageBonus, duration);
            }
            //buffEnemies.Clear();
            currEnemy.stamina -= cost;

            poisoned = true;
            return true;
        }
        else
            return false;
    }
}
