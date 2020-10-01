using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidWobble : MonoBehaviour
{
    Renderer rend;
    Vector3 lastPos;
    Vector3 velocity;
    Vector3 lastRot;
    Vector3 angularVelocity;
    public float MaxWobble = 0.03f;
    public float WobbleSpeed = 1f;
    public float recovery = 1f;
    float wobbleAmountX;
    float wobbleAmountZ;
    float wobbleAmountToAddX;
    float wobbleAmountToAddZ;
    float pulse;
    float time = 0.5f;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        if(rend != null)
        {
            time += Time.deltaTime;

            //Decrease wobble over time
            wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * (recovery));
            wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * (recovery));

            //Make a sine wave of the decreasing wobble
            pulse = 2 * Mathf.PI * WobbleSpeed;
            wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
            wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);

            //Send it to the shader
            rend.sharedMaterials[0].SetFloat("_wobbleX", wobbleAmountX);
            rend.sharedMaterials[0].SetFloat("_wobbleZ", wobbleAmountZ);


            //Velocity
            velocity = (lastPos - transform.position) / Time.deltaTime;
            angularVelocity = transform.rotation.eulerAngles - lastRot;

            //Add clamped velocity to wobble
            wobbleAmountToAddX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
            wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);

            //Keep last position
            lastPos = transform.position;
            lastRot = transform.rotation.eulerAngles;
        }
    }
}
