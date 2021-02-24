using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorHandler animatorHandler;
    InputHandler inputHandler;
    public string lastAttack;

    CardsContainer cards;

    public bool DrawGizmos;

    public float damage;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        inputHandler = GetComponent<InputHandler>();
        cards = this.GetComponent<CardsContainer>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (inputHandler.comboFlag)
        {
            animatorHandler.anim.SetBool("canDoCombo", false);
            if (lastAttack == weapon.OH_Light_Attack_1)
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                lastAttack = weapon.OH_Light_Attack_2;
            }
            else if (lastAttack == weapon.OH_Light_Attack_2)
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
        lastAttack = weapon.OH_Light_Attack_1;
        EnemiesInFront(3f, 1);
    }

    public void HandleLightAttackCombo(WeaponItem weapon)
    {
        animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
        lastAttack = weapon.OH_Light_Attack_2;
        EnemiesInFront(3f, 1.2f);
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
        lastAttack = weapon.OH_Heavy_Attack_1;
        EnemiesInFront(3f, 2);
    }

    void EnemiesInFront(float radious, float dmgMultiplier)
    {
        var enemiesInFront = Physics.OverlapSphere(this.transform.position, radious);

        var _enemiesInFront = enemiesInFront.Where(x => Mathf.Abs(Vector3.Angle(this.transform.forward, x.transform.position - this.transform.position))
        <= 90 && !x.GetComponent<PlayerMovement>() && x.GetComponent<Enemy>()).Select(x => x.GetComponent<Enemy>());

        foreach (var item in _enemiesInFront)
        {
            item.GetComponent<TakeDamage>().TakeDamageToHealth(damage, this.gameObject);
            cards.weaponSlot.TriggerCard(item);
        }
    }

    private void OnDrawGizmos()
    {
        if (!DrawGizmos)
            return;
        Gizmos.color = Color.red;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 2.2f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 2.2f));

        Vector3 rightLimit = Quaternion.AngleAxis(60, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + (rightLimit * 2.2f));

        Vector3 leftLimit = Quaternion.AngleAxis(-60, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + (leftLimit * 2.2f));
    }
}