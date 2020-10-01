using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMaskCenter : MonoBehaviour
{
    public Material AppearShaderMat;
    public Material DisappearShaderMat;
    public Material SwapShaderMat;

    void Update()
    {
        if (AppearShaderMat != null)
        {
            AppearShaderMat.SetVector("_sphereMaskCenter", transform.position);
            DisappearShaderMat.SetVector("_sphereMaskCenter", transform.position);
            SwapShaderMat.SetVector("_sphereMaskCenter", transform.position);
        }
    }
}
