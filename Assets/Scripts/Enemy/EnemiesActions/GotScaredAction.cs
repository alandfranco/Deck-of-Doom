using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GotScaredAction : GOAPAction
{
    private bool isScared;
    private bool endScared;
    public float count;
    private Transform dashTarget;

    public GotScaredAction()
    {
        addEffect("stayAlive", true);
        requiesVision = false;
        cost = 1;
        endScared = false;
    }

    private void Update()
    {
        Enemy curr = GetComponent<Enemy>();

        isScared = curr.isScared;
        if(isScared)
        {
            count -= Time.deltaTime;
            if (count <= 0)
            {
                isScared = false;
                curr.isScared = false;
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
        return endScared;
    }

    public override bool RequiresInRange()
    {
        return false;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        Enemy currE = agent.GetComponent<Enemy>();

        dashTarget = FindObjectOfType<HideSpots>().hideSpots
            .OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position))
            .Last();

        if (isScared)
        {
            target = dashTarget.gameObject;
            currE.agent.isStopped = false;
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
        if (isScared)
        {
            dashTarget = FindObjectOfType<HideSpots>().hideSpots
            .OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position))
            .Last();
            currEnemy.agent.destination = dashTarget.position;
            target = dashTarget.gameObject;
            endScared = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}
