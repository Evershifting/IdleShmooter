using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Lane : MonoBehaviour, ILane
{
    public List<IZombie> _zombies = new List<IZombie>();
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
    private bool _isDoubleDamage;

    [SerializeField]
    private Coroutine routine;

    public void Init(string name, int zombieAmount, float rewardPerZombie, float damagePerShot, float zombieHP, float zombieSpawnDelay, float shotDelay, float upgradeCost)
    {
        this.name = name;
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
    public float LaneIncomePerSecond
    {
        get
        {
            return DamagePerShot / _copAttackDelay / ZombieHP;
        }
    }

    private void OnEnable()
    {
        EventsManager.AddListener<IZombie>(EventsType.ZombieDied, OnZombieDied);
        EventsManager.AddListener<ICop>(EventsType.CopShot, OnCopShot);
        EventsManager.AddListener<ICop>(EventsType.CopClicked, OnCopClicked);
        EventsManager.AddListener<bool>(EventsType.DoubleDamage, OnDoubleDamage);
        if (routine==null)
            routine = StartCoroutine(InitialZombieSpawn());
    }

    private void OnDisable()
    {
        EventsManager.RemoveListener<IZombie>(EventsType.ZombieDied, OnZombieDied);
        EventsManager.RemoveListener<ICop>(EventsType.CopShot, OnCopShot);
        EventsManager.RemoveListener<ICop>(EventsType.CopClicked, OnCopClicked);

        foreach (Transform zombie in _zombieParent.transform)
        {
            Destroy(zombie.gameObject);
        }
        _zombies.Clear();
        ZombieSpawner.ClearZombies();
        routine = null;
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
        if (routine == null)
            routine = StartCoroutine(InitialZombieSpawn());
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

    public void LaneClicked()
    {
        Shoot();
    }
    private void Shoot()
    {
        _cop.Shoot();
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

    public void AddZombie(IZombie zombie)
    {
        if (!_zombies.Contains(zombie))
            _zombies.Add(zombie);
    }

    #region Events
    private void OnZombieDied(IZombie zombie)
    {
        if (_zombies.Contains(zombie))
        {
            _zombies.Remove(zombie);
            SpawnZombie();
        }
    }

    private void OnCopShot(ICop cop)
    {
        if (cop as UnityEngine.Object == _cop && _zombies.Count > 0)
            _zombies[0].ReceiveDamage(_isDoubleDamage ? DamagePerShot * 2 : DamagePerShot);
    }

    private void OnCopClicked(ICop cop)
    {
        if (cop as UnityEngine.Object == _cop)
            UIManager.UpgradeLane(this, UpgradeLane);
    }

    private void OnDoubleDamage(bool value)
    {
        _isDoubleDamage = value;
    }
    #endregion
}
