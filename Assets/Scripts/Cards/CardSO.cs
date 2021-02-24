using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/New Card")]
public class CardSO : ScriptableObject
{
    public new string name;
    public string description;

    public float chance;

    public float duration;

    public float dmg;
    public float dmgOvertime;

    public float cooldown;

    public CardType cardType;

    public enum CardType
    {
        attack, defense, special, various
    }
}
