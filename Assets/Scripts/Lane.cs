using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Lane : MonoBehaviour
{
    public List<Zombie> _zombies = new List<Zombie>();
    private WaitForSeconds _zombieSpawnDelayWait;
    private float _currentShotDelay;
    private Settings _settings;
    private int _currentUpgradeLevel;

    [SerializeField]
    private int _zombieAmount = 4;
    [SerializeField]
    private float _rewardPerZombie, _copDamage, _zombieHP;
    [SerializeField]
    private float _zombieSpawnDelay = 0.3f, _copAttackDelay = 1f;
    [SerializeField]
    private float _upgradeCost;


    [SerializeField]
    private Transform _zombieParent;
    [SerializeField]
    private Cop _cop;

    public void Init(int zombieAmount, float rewardPerZombie, float damagePerShot, float zombieHP, float zombieSpawnDelay, float shotDelay, float upgradeCost)
    {
        _zombieAmount = zombieAmount;
        _rewardPerZombie = rewardPerZombie;
        _copDamage = damagePerShot;
        _zombieHP = zombieHP;
        _zombieSpawnDelay = zombieSpawnDelay;
        _copAttackDelay = shotDelay;
        _upgradeCost = upgradeCost;
    }

    public float RewardPerZombie { get => _rewardPerZombie; }
    public float DamagePerShot { get => _copDamage; }
    public float ZombieHP { get => _zombieHP; }
    public Transform ZombieParent { get => _zombieParent; }
    public float UpgradeCost { get => _upgradeCost; }

    private void OnEnable()
    {
        EventsManager.AddListener<Zombie>(EventsType.ZombieDied, OnZombieDied);
        EventsManager.AddListener<Cop>(EventsType.CopShot, OnCopShot);
        EventsManager.AddListener<Cop>(EventsType.CopClicked, OnCopClicked);
    }

    private void OnDisable()
    {
        EventsManager.RemoveListener<Zombie>(EventsType.ZombieDied, OnZombieDied);
        EventsManager.RemoveListener<Cop>(EventsType.CopShot, OnCopShot);
        EventsManager.RemoveListener<Cop>(EventsType.CopClicked, OnCopClicked);
    }

    private void OnZombieDied(Zombie zombie)
    {
        if (_zombies.Contains(zombie))
        {
            _zombies.Remove(zombie);
            SpawnZombie();
        }
    }
    private void Awake()
    {
        if (!_settings)
            _settings = Resources.Load<Settings>("Settings");
    }

    private void Start()
    {
        _currentShotDelay = Random.Range(-1.5f, _copAttackDelay);
        _zombieSpawnDelayWait = new WaitForSeconds(_zombieSpawnDelay);
        StartCoroutine(InitialZombieSpawn());
    }

    private IEnumerator InitialZombieSpawn()
    {
        for (int i = 0; i < _zombieAmount; i++)
        {
            yield return _zombieSpawnDelayWait;
            SpawnZombie();
        }
    }

    private void SpawnZombie()
    {
        ZombieSpawner.Spawn(this);
    }

    private void Update()
    {
        _currentShotDelay += Time.deltaTime;
        if (_currentShotDelay >= _copAttackDelay)
        {
            _currentShotDelay = 0;
            Shoot();
        }
    }
    public void OnMouseDown()
    {
        _cop.Shoot();
    }
    public void Shoot()
    {
        _cop.Shoot();
    }

    private void OnCopClicked(Cop cop)
    {
        if (cop == _cop)
            UIManager.UpgradeLane(this, UpgradeLane);
    }

    private void UpgradeLane()
    {
        Debug.Log($"{name} Upgraded");
        _currentUpgradeLevel++;
        _zombieHP *= _settings.ZombiesHPGrowthLevel;
        _rewardPerZombie *= _settings.ZombiesRewardGrowthLevel;
        _copDamage *= _settings.CopDamageGrowthLevel;
        _copAttackDelay *= _settings.CopAttackDelayGrowthLevel;
        _upgradeCost *= _settings.UpgradeCostGrowthLevel;
    }
    private void OnCopShot(Cop cop)
    {
        if (cop == _cop && _zombies.Count > 0)
            _zombies[0].ReceiveDamage(DamagePerShot);
    }

    public void AddZombie(Zombie zombie)
    {
        if (!_zombies.Contains(zombie))
            _zombies.Add(zombie);
    }
}
