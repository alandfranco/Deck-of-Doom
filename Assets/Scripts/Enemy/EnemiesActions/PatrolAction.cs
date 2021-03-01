using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PatrolAction : GOAPAction
{
    private bool reachedDestination = false;

    Enemy currEnemy;

    [SerializeField]GameObject pointer;

    PlayerManager pl;

    public PatrolAction()
    {
        addEffect("stayAlive", true);
        cost = 20f;
    }

    private void Start()
    {
        currEnemy = this.GetComponent<Enemy>();
        pl = FindObjectOfType<PlayerManager>();
        StartCoroutine(PatrolTo());
    }

    public override void Reset()
    {
        target = null;
    }

    public override bool IsDone()
    {
        return reachedDestination;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    IEnumerator PatrolTo()
    {
        pointer = transform.GetChild(transform.childCount - 1).gameObject;        
        var checkPos = Random.insideUnitSphere * 5;
        if (IsReachable(checkPos))
        {
            pointer.transform.position = new Vector3(checkPos.x, checkPos.y);
        }
        else
            pointer.transform.position = this.transform.position;

        yield return new WaitForSeconds(0f);
        yield return new WaitForSeconds(2f);
        StartCoroutine(PatrolTo());
        yield break;
    }

    public bool IsReachable(Vector3 pos)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        bool canReach = currEnemy.agent.CalculatePath(pos, navMeshPath);
        if (navMeshPath.status == NavMeshPathStatus.PathInvalid)
            return false;
        else
            return true;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        target = pointer;
        if (this.GetComponent<Enemy>().isDisable && GameManager.instance.enemiesAttacking >= GameManager.instance.maxAmountAttacking
            || Vector3.Distance(this.transform.position, pl.transform.position) >= currEnemy.config.aggroDist || !IsReachable(pl.transform.position))
        {
            target = null;
            return false;
        }
        else
            return target != null;
            
        /*if (!currEnemy.isDisable && Vector3.Distance(this.transform.position, pl.transform.position) >= currEnemy.config.aggroDist)
        {
            target = pointer;
            return target != null;
        }
        else
            return false;*/
    }

    public override bool Perform(GameObject agent)
    {
        if (currEnemy.stamina >= (cost) && !currEnemy.isDisable)
        {
            currEnemy.agent.isStopped = true;

            currEnemy.stamina -= cost;

            reachedDestination = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}
