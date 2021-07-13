using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombKart : GlobalEvent
{
    PlayerManager pl;

    public float speed;
    int speedMod;

    public List<KartWaypoints> waypoints = new List<KartWaypoints>();
    public int index;

    public Transform currentTarget;

    void Start()
    {
        pl = FindObjectOfType<PlayerManager>();

        currentTarget = waypoints[0].transform;

        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            waypoints[i].index = i;
        }

        this.transform.LookAt(new Vector3(currentTarget.transform.position.x, this.transform.position.y, currentTarget.transform.position.z));
    }

    void Update()
    {
        this.transform.position += this.transform.forward * speed * speedMod * Time.deltaTime;
    }

    public void WaypointPassed(int n)
    {        
        currentTarget = waypoints[n + 1].transform;
        this.transform.LookAt(new Vector3(currentTarget.transform.position.x, this.transform.position.y, currentTarget.transform.position.z));
    }

    private void OnTriggerEnter(Collider c)
    {
        if(c.gameObject == pl.gameObject)
        {
            speedMod = 1;
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject == pl.gameObject)
        {
            speedMod = 0;
        }
    }
}