using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DirectionalLightShader : MonoBehaviour
{
    [SerializeField]
    Light dirLight;

    void Update()
    {
        Shader.SetGlobalVector("_lightDirectionVec", -transform.forward);
        Shader.SetGlobalFloat("_lightIntensity", dirLight.intensity);
    }

    //Drop this into the DirectionalLight and set the light into the slot
}
