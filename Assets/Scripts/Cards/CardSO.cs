using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "New Card", menuName = "Card/New Card")]
public class CardSO : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite image;

    public float chance;

    public float duration;

    public float dmg;
    public float dmgOvertime;

    public float cooldown;

    public bool isOwned;

    public CardType cardType;

    public enum CardType
    {
        attack, defense, special,
    }

    public GameObject myCard;

    [SerializeField] protected string id = string.Empty;

    public string Id => id;

    [ContextMenu("Generate Id")]
    private void GenerateId() => id = Guid.NewGuid().ToString();
}
