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

    Enemy currE;
    PlayerManager player;

    public HideAction()
    {
        addEffect("stayAlive", true);
        cost = 50;
        dashSpeed = 2;
        requiesVision = false;
    }

    void Awake()
    {
        currE = this.GetComponent<Enemy>();
        player = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        
        if(target != null && Vector3.Distance(player.transform.position, this.transform.position) >= 10)
        {
            isDashing = false;
            currE.agent.isStopped = true;
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
        
        if (Vector3.Distance(this.transform.position, player.transform.position) <= 5 && currE.stamina >= cost && !currE.isDisable)
        {
            dashTarget = FindObjectOfType<HideSpots>().hideSpots
            .OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position))
            .Last();

            target = dashTarget.gameObject;
            currE.agent.isStopped = false;
            return true;
        }
        else
        {
            target = null;
            return false;
        }
    }

    public override bool Perform(GameObject agent)
    {
        Enemy currEnemy = agent.GetComponent<Enemy>();
        if (currEnemy.stamina >= (cost) && !isDashing && !currEnemy.isDisable && Vector3.Distance(this.transform.position, player.transform.position) >= 10)
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
        {
            target = null;
            return false;
        }
    }
}
