﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    private static Zombie _zombiePrefabRef;
    private static IMoveBehaviour _moveBehaviour, _meleeBehaviour;
    private static float _zombieSpeedRef;
    private static Vector3 _spawnPositionDelta;
    private static Stack<Zombie> _spawnableZombies = new Stack<Zombie>();

    [SerializeField]
    private Zombie _zombiePrefab;
    [SerializeField]
    private float _zombieSpeed;

    private void Awake()
    {
        _spawnPositionDelta = new Vector3(UnityEngine.Random.Range(-1, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
        _zombiePrefabRef = _zombiePrefab;
        _zombieSpeedRef = _zombieSpeed;
    }

    internal static void Spawn(Lane lane)
    {
        Zombie zombie;
        if (_spawnableZombies.Count > 0)
        {
            zombie = _spawnableZombies.Pop();
            zombie.gameObject.SetActive(true);
        }
        else
            zombie = Instantiate(_zombiePrefabRef, lane.ZombieParent);
        zombie.transform.localRotation = Quaternion.Euler(0, -90, 0);
        zombie.transform.position = lane.ZombieParent.position 
            + zombie.transform.forward * UnityEngine.Random.Range(-1f, 1f) + zombie.transform.right * UnityEngine.Random.Range(-1f, 1f);

        _moveBehaviour = new ZombieMoveBehaviour(zombie);
        _meleeBehaviour = new ZombieMeleeBehaviour(zombie);
        zombie.Init(lane.ZombieHP, _zombieSpeedRef, lane.RewardPerZombie, _moveBehaviour, _meleeBehaviour);

        lane.AddZombie(zombie);
    }

    internal static void Free(Zombie zombie)
    {
        _spawnableZombies.Push(zombie);
        zombie.gameObject.SetActive(false);
    }
}
