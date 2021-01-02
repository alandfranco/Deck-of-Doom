using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/New Enemy")]
public class EnemySO : ScriptableObject
{    
    public float maxHealth;

    public float speed;

    public float dmg;

    public float attackRate;

    public float aggroDist;

    public float attackDistance;

    public float maxStamina; //la stamina va a determinar las acciones que puede hacer la IA segun el costo de cada accion. Esta se regenera
    public float staminaRegen;

    public GameObject bodymodel;

    //public IA comportamiento;
}