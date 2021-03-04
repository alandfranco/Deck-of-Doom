using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAction : GOAPAction
{
    private bool teleported = false;
    private Transform teleporTarget;
    private bool isTeleporting = false;

    Enemy currE;
    PlayerManager player;

    public TeleportAction()
    {
        addEffect("stayAlive", true);
        cost = 20;
        requiesVision = false;
    }

    void Awake()
    {
        currE = this.GetComponent<Enemy>();
        player = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
    }

    public override void Reset()
    {
        teleported = false;
        target = null;
    }

    public override bool IsDone()
    {
        return teleported;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {

        if (currE.stamina >= cost && !currE.isDisable)
        {
            //teleporTarget = FindObjectOfType<TeleportSpots>().teleportSpots
            //.OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position))
            //.Last();

            target = teleporTarget.gameObject;
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
        if (currEnemy.stamina >= (cost) && !isTeleporting && !currEnemy.isDisable)
        {
            //currEnemy.agent.speed *= dashSpeed;
            //target.GetComponent<TakeDamage>().TakeDamage();

            currEnemy.stamina -= cost;

            isTeleporting = true;

            /*teleporTarget = FindObjectOfType<HideSpots>().hideSpots
            .OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position))
            .Last();*/
            currEnemy.agent.destination = teleporTarget.position;
            target = teleporTarget.gameObject;

            teleported = true;
            return true;
        }
        else
        {
            target = null;
            return false;
        }
    }
}
