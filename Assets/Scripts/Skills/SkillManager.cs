﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<Skills> skills = new List<Skills>();

    Camera cam;

    public Transform aimingSpot;

    public GameObject visualPoint;

    public bool viewVisual;

    void Start()
    {
        cam = Camera.main;
        viewVisual = true;
    }

    void Update()
    {
        AimingPoint();

        if (viewVisual)
            visualPoint.SetActive(true);
        else
            visualPoint.SetActive(false);
    }

    public void PerformSkilOne()
    {
        skills[0].transform.position = this.transform.position;        
        skills[0].TriggerAbility();
    }

    public void PerfomSkillTwo()
    {
        skills[1].transform.position = this.transform.position;        
        skills[1].TriggerAbility();
    }

    public void PerfomSkillThree()
    {
        skills[2].transform.position = this.transform.position;        
        skills[2].TriggerAbility();
    }

    public void PerfomSkillFour()
    {
        skills[3].transform.position = this.transform.position;        
        skills[3].TriggerAbility();
    }

    public void AimingPoint()
    {
        int layerMask2 = 1 << 10;
        int layerMask = 1 << 9;
        //int layerMask3 = 1 << 11;
        layerMask = ~((layerMask) | (layerMask2) /*| (layerMask3*/);

        var ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20, layerMask))
        {
            aimingSpot.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            visualPoint.transform.position = aimingSpot.position;
        }
        
    }
}
