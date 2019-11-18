using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Lane : MonoBehaviour
{
    private List<Zombie> _zombies = new List<Zombie>();
    private WaitForSeconds _zombieSpawnDelayWait;
    private float _currentShotDelay;

    [SerializeField]
    private int _zombieAmount = 4;
    [SerializeField]
    private Transform _zombieParent;
    [SerializeField]
    private Animator _cop;
    [SerializeField]
    private float rewardPerZombie, damagePerShot, zombieHP;
    [SerializeField]
    private float _zombieSpawnDelay = 0.3f, _shotDelay = 1f;

    public float RewardPerZombie { get => rewardPerZombie; }
    public float DamagePerShot { get => damagePerShot; }
    public float ZombieHP { get => zombieHP; }
    public Transform ZombieParent { get => _zombieParent; }

    private void OnEnable() => EventsManager.AddListener<Zombie>(EventsType.ZombieDied, OnZombieDied);
    private void OnDisable() => EventsManager.RemoveListener<Zombie>(EventsType.ZombieDied, OnZombieDied);

    private void OnZombieDied(Zombie zombie)
    {
        if (_zombies.Contains(zombie))
        {
            _zombies.Remove(zombie);
            SpawnZombie();
        }
    }

    private void Start()
    {
        _currentShotDelay = Random.Range(-1.5f, _shotDelay);
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
        if (_currentShotDelay >= _shotDelay)
        {
            _currentShotDelay = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        _cop.SetTrigger("Shoot");
        if (_zombies.Count > 0)
            _zombies[0].ReceiveDamage(DamagePerShot);
    }

    public void AddZombie(Zombie zombie)
    {
        if (!_zombies.Contains(zombie))
            _zombies.Add(zombie);
    }
}
