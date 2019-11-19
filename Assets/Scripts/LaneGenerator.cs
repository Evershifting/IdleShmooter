using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneGenerator : MonoBehaviour
{
    [SerializeField]
    private Lane _lanePrefab;

    private int _laneAmount = 20;
    private Settings _settings;

    private void Awake()
    {
        if (!_settings)
            _settings = Resources.Load<Settings>("Settings");
        _laneAmount = _settings.LanesAmount;
    }

    private void Start()
    {
        for (int i = 0; i < _laneAmount; i++)
        {
            Lane lane = Instantiate(_lanePrefab, transform.position + Vector3.forward * _settings.LaneSpacing * i, Quaternion.identity, transform);
            lane.name = $"Lane {(i + 1).ToString()}";
            lane.Init(
                _settings.ZombiesAmount,
                _settings.ZombiesReward * Mathf.Pow(_settings.ZombiesRewardGrowthLane, i),
                _settings.CopDamage * Mathf.Pow(_settings.CopDamageGrowthLane, i),
                _settings.ZombiesHP * Mathf.Pow(_settings.ZombiesHPGrowthLane, i),
                _settings.ZombiesSpawnDelay,
                _settings.CopAttackDelay * Mathf.Pow(_settings.CopAttackDelayGrowthLane, i),
                _settings.UpgradeCost * Mathf.Pow(_settings.UpgradeCostGrowthLane, i));
        }
    }
}
