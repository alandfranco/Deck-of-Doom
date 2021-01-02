using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HideAction : GOAPAction
{
    private bool moved = false;
    private float dashSpeed;
    private Transform dashTarget;
    private bool isDashing = false;

    public HideAction()
    {
        addEffect("stayAlive", true);
        cost = 50;
        dashSpeed = 2;
        requiesVision = false;
    }

    private void Update()
    {
        Enemy curr = GetComponent<Enemy>();
        
        if(target != null && Vector3.Distance(target.transform.position, this.transform.position) <= curr.agent.stoppingDistance)
        {
            isDashing = false;
            target = null;
        }
    }

    public override void Reset()
    {
        moved = false;
        target = null;
    }

    public override bool IsDone()
    {
        return moved;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        Enemy currE = agent.GetComponent<Enemy>();
        var player = FindObjectOfType<PlayerMovement>();

        dashTarget = FindObjectOfType<HideSpots>().hideSpots
            .OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position))
            .Last();

        if (Vector3.Distance(this.transform.position, player.transform.position) <= 5 && currE.stamina >= cost)
        {
            target = dashTarget.gameObject;
            currE.agent.isStopped = false;
            return true;
        }
        return false;
    }

    public override bool Perform(GameObject agent)
    {
        Enemy currEnemy = agent.GetComponent<Enemy>();
            Debug.Log("DECIME QUE ENTSA");
        if (currEnemy.stamina >= (cost) && !isDashing)
        {
            currEnemy.agent.speed *= dashSpeed;
            //target.GetComponent<TakeDamage>().TakeDamage();
            
            currEnemy.stamina -= cost;

            isDashing = true;

            dashTarget = FindObjectOfType<HideSpots>().hideSpots
            .OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position))
            .Last();
            currEnemy.agent.destination = dashTarget.position;
            target = dashTarget.gameObject;

            moved = true;            
            return true;
        }
        else
            return false;
    }
}
