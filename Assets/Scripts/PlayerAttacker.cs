using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorHandler animatorHandler;
    public Animator anim;
    InputHandler inputHandler;
    public string lastAttack;
    int comboCounter = 0;

    public CardsContainer cards;

    public bool DrawGizmos;

    public float damage;
    public float damageRadius;

    public float fadeAnim;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        inputHandler = GetComponent<InputHandler>();
        cards = this.GetComponent<CardsContainer>();
        //Time.timeScale = 0.2f;
        //anim = GetComponentInChildren<Animator>();
    }

    public void HandleAttack(WeaponItem weapon, bool combo)
    {
        if(combo)
        {
            comboCounter++;
            if (comboCounter > weapon.attacks.Count)
            {
                comboCounter = 1;
            }
            string animName = "combo_" + comboCounter;
            anim.Play(animName);
            //animatorHandler.PlayTargetAnimation(weapon.attacks[comboCounter], true, fadeAnim);            
            lastAttack = weapon.attacks[comboCounter-1];
        }
        else
        {
            comboCounter = 1;
            string animName = "combo_" + comboCounter;
            anim.Play(animName);
            //animatorHandler.PlayTargetAnimation(weapon.attacks[comboCounter], true, 0);
            lastAttack = weapon.attacks[comboCounter];
        }
    }

    public void EnemiesInFront(float radious, float dmgMultiplier)
    {
        var enemiesInFront = Physics.OverlapSphere(this.transform.position, radious);

        var _enemiesInFront = enemiesInFront.Where(x => Mathf.Abs(Vector3.Angle(this.transform.forward, x.transform.position - this.transform.position))
        <= 90 && !x.GetComponent<PlayerMovement>() && x.GetComponent<Enemy>()).Select(x => x.GetComponent<Enemy>());

        foreach (var item in _enemiesInFront)
        {
            item.GetComponent<TakeDamage>().TakeDamageToHealth(damage + (damage * PlayerPassives.instance.damageBonus), this.gameObject);
            if(cards.weaponSlot != null && item != null)
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