using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotStunedAction : GOAPAction
{
    private bool isStuned;
    private bool endStun;
    public float count;
    private Transform dashTarget;

    public GotStunedAction()
    {
        addEffect("stayAlive", true);
        requiesVision = false;
        cost = 1;
        endStun = false;
    }

    private void Update()
    {
        Enemy curr = GetComponent<Enemy>();

        isStuned = curr.isStuned;
        if (isStuned)
        {
            count -= Time.deltaTime;
            if (count <= 0)
            {
                isStuned = false;
                curr.isStuned = false;
            }
        }
    }

    public override void Reset()
    {
        //isScared = false;
        target = null;
    }

    public override bool IsDone()
    {
        //GetComponent<Enemy>().isScared = false;
        //isScared = false;
        return endStun;
    }

    public override bool RequiresInRange()
    {
        return false;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        Enemy currE = agent.GetComponent<Enemy>();

        if (isStuned)
        {
            target = this.gameObject;
            currE.agent.isStopped = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool Perform(GameObject agent)
    {
        Enemy currEnemy = agent.GetComponent<Enemy>();
        if (isStuned)
        {
            currEnemy.anim.Play("Idle");
            currEnemy.agent.isStopped = true;            
            endStun = true;
            return true;
        }
        else
        {
            currEnemy.agent.isStopped = false;
            return false;
        }
    }
}
