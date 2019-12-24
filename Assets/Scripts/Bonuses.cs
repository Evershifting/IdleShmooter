using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Bonuses", fileName = "Bonuses", order = 1)]
public class Bonuses : ScriptableObject
{
    [SerializeField, Header("Bonus Durations"), Tooltip("Duration in milliseconds")]
    private float _doubleDamage = 180f;
    [SerializeField, Tooltip("Duration in milliseconds")]
    private float _doubleIncome = 180f;

    [SerializeField, Header("Bonus Values")]
    private float _instantMoneyBase = 30f;

    [SerializeField, Header("Bonus Spawn Timers"), Tooltip("Value in seconds")]
    private float _instantMoneySpawn = 45f;

    [SerializeField, Header("Bonus Spawn Timers"), Tooltip("Value in seconds")]
    private float _instantMoneyFallDuration = 5f;

    //Bonus Durations
    public float DoubleDamage { get => _doubleDamage; }
    public float DoubleIncome { get => _doubleIncome; }

    //Bonus Values
    public float InstantMoney { get => _instantMoneyBase * GameManager.PassiveIncome; }
    public float InstantMoneySpawn { get => _instantMoneySpawn; }
    public float InstantMoneyFallDuration { get => _instantMoneyFallDuration; }
}

