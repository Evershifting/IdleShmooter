using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/LaneAndZombies", fileName = "LaneAndZombies", order = 0)]
public class Settings : ScriptableObject
{
    [Header("Global Stats")]
    [SerializeField, Range(1, 40)]
    private int _lanesAmount = 1;
    [SerializeField, Range(0, 10)]
    private float _zombieSpeed;
    [SerializeField]
    private float _laneSpacing = 2.5f;
    [SerializeField]
    private Vector2 _zombieSpawnDisplaycement;

    [Header("Lane Stats")]
    [Header("Base Stats")]
    [SerializeField, Range(0, 20)]
    private int _zombiesAmount;
    [SerializeField, Range(0, float.MaxValue)]
    private float _zombiesHP, _zombiesReward, _zombiesSpawnDelay, _copDamage, _copAttackDelay, _upgradeCost;

    [Header("Upgrade Stats per Lane")]
    [SerializeField, Range(0, float.MaxValue)]
    private float _zombiesHPGrowthLane;
    [SerializeField, Range(0, float.MaxValue)]
    private float _zombiesRewardGrowthLane, _copDamageGrowthLane, _copAttackDelayGrowthLane, _upgradeCostGrowthLane;

    [Header("Upgrade Stats per Level")]
    [SerializeField, Range(0, float.MaxValue)]
    private float _zombiesHPGrowthLevel;
    [SerializeField, Range(0, float.MaxValue)]
    private float _zombiesRewardGrowthLevel, _copDamageGrowthLevel, _copAttackDelayGrowthLevel, _upgradeCostGrowthLevel;


    //Global Stats
    public int LanesAmount { get => _lanesAmount; }
    public float ZombieSpeed { get => _zombieSpeed; }
    public float LaneSpacing { get => _laneSpacing; }
    public Vector2 ZombieSpawnDisplaycement { get => _zombieSpawnDisplaycement; }

    //Base Stats
    public int ZombiesAmount { get => _zombiesAmount; }
    public float ZombiesHP { get => _zombiesHP; }
    public float ZombiesReward { get => _zombiesReward; }
    public float ZombiesSpawnDelay { get => _zombiesSpawnDelay; }
    public float CopDamage { get => _copDamage; }
    public float CopAttackDelay { get => _copAttackDelay; }
    public float UpgradeCost { get => _upgradeCost; }

    //Upgrade Stats per Lane
    public float ZombiesHPGrowthLane { get => _zombiesHPGrowthLane; }
    public float ZombiesRewardGrowthLane { get => _zombiesRewardGrowthLane; }
    public float CopDamageGrowthLane { get => _copDamageGrowthLane; }
    public float CopAttackDelayGrowthLane { get => _copAttackDelayGrowthLane; }
    public float UpgradeCostGrowthLane { get => _upgradeCostGrowthLane; }

    //Upgrade Stats per Level
    public float ZombiesHPGrowthLevel { get => _zombiesHPGrowthLevel; set => _zombiesHPGrowthLevel = value; }
    public float ZombiesRewardGrowthLevel { get => _zombiesRewardGrowthLevel; }
    public float CopDamageGrowthLevel { get => _copDamageGrowthLevel; }
    public float CopAttackDelayGrowthLevel { get => _copAttackDelayGrowthLevel; }
    public float UpgradeCostGrowthLevel { get => _upgradeCostGrowthLevel; }

}
