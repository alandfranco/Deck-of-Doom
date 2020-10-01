using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconstructOrigin : MonoBehaviour
{
    void Update()
    {
        Shader.SetGlobalVector("_playerPosReconstruct", transform.position);
    }
}
