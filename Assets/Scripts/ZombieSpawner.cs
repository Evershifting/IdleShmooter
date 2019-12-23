using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    private static Zombie _zombiePrefabRef;
    private static IMoveBehaviour _moveBehaviour, _meleeBehaviour;
    private static float _zombieSpeedRef;
    private static Vector3 _spawnPositionDelta;
    private static Stack<IZombie> _spawnableZombies = new Stack<IZombie>();

    [SerializeField]
    private Zombie _zombiePrefab;

    private float _zombieSpeed;
    private static Settings _settings;

    private void Awake()
    {
        if (!_settings)
            _settings = Resources.Load<Settings>("Settings");
        _spawnPositionDelta = new Vector3(UnityEngine.Random.Range(-1, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
        _zombiePrefabRef = _zombiePrefab;
        _zombieSpeedRef =  _settings.ZombieSpeed;
    }

    internal static void Spawn(ILane lane)
    {
        Zombie zombie;
        if (_spawnableZombies.Count > 0)
        {
            zombie = _spawnableZombies.Pop() as Zombie;
            zombie.gameObject.SetActive(true);
        }
        else
            zombie = Instantiate(_zombiePrefabRef, lane.ZombieParent);
        zombie.transform.localRotation = Quaternion.Euler(0, -90, 0);
        zombie.transform.position = lane.ZombieParent.position 
            + zombie.transform.forward * UnityEngine.Random.Range(_settings.ZombieSpawnDisplaycement.x,-_settings.ZombieSpawnDisplaycement.x) 
            + zombie.transform.right * UnityEngine.Random.Range(_settings.ZombieSpawnDisplaycement.y, -_settings.ZombieSpawnDisplaycement.y);

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
