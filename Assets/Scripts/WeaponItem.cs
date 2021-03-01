using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;
    public List<string> attacks = new List<string>();

    //REPLACE WITH LIST
    [Header("Attack Clips")]
    public string OH_Light_Attack_1;
    public string OH_Light_Attack_2;
    public string OH_Heavy_Attack_1;
    //

    [Header("Weapon VFX")]
    public GameObject hitFX;
}
