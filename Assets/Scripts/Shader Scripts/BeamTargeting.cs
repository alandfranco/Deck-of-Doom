using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamTargeting : MonoBehaviour
{
    public GameObject beamPrefab;
    public GameObject spawnPoint;

    private GameObject spawnedBeam;
    void Start()
    {
        spawnedBeam = Instantiate(beamPrefab, spawnPoint.transform) as GameObject;
        spawnedBeam.transform.forward = spawnPoint.transform.forward;
        spawnedBeam.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            EnableBeam();
        }

        if (Input.GetMouseButton(0))
        {
            UpdateBeam();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DisableBeam();
        }
    }

    void EnableBeam()
    {
        spawnedBeam.SetActive(true);
    }

    void UpdateBeam()
    {
        if (spawnPoint != null)
        {
            spawnedBeam.transform.position = spawnPoint.transform.position;
        }
    }

    void DisableBeam()
    {
        spawnedBeam.SetActive(false);
    }

}
