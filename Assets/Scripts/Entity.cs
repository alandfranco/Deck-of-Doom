using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float stamina;
    public float maxStamina;
    public float staminaRecoverPS;

    [HideInInspector]
    public Animator anim;

    public virtual void ReduceStamina(float amount)
    {
        stamina -= amount;

        if (stamina <= 0) stamina = 0;
    }
}
