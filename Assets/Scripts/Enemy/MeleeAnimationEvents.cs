using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAnimationEvents : MonoBehaviour
{
    [SerializeField] BoxCollider weaponCollider;

    private void Awake()
    {
        weaponCollider.enabled = false;
    }

    public void ActivateCollider()
    {
        weaponCollider.enabled = true;
    }

    public void DeactivateCollider()
    {
        weaponCollider.enabled = false;
        GameManager.instance.enemiesAttacking--;
    }
}
