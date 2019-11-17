using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    private static Zombie _zombiePrefabRef;
    private static IMoveBehaviour _moveBehaviour, _meleeBehaviour;
    private static int _zombieSpeedRef;

    [SerializeField]
    Zombie _zombiePrefab;
    [SerializeField]
    int _zombieSpeed;

    private void Awake()
    {
        _zombiePrefabRef = _zombiePrefab;
        _zombieSpeedRef = _zombieSpeed;
        _moveBehaviour = new ZombieMoveBehaviour();
        _meleeBehaviour = new ZombieMeleeBehaviour();
    }

    internal static void Spawn(Lane lane)
    {
        Zombie zombie = Instantiate(_zombiePrefabRef, lane.zombieParent);
        zombie.Init(lane.ZombieHP, _zombieSpeedRef, lane.RewardPerZombie, _moveBehaviour, _meleeBehaviour);
    }

    internal static void Free(Zombie zombie)
    {
        //throw new NotImplementedException();
        Destroy(zombie.gameObject);
    }
}
