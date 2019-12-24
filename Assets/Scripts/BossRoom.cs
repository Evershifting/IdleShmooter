using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossRoom : MonoBehaviour, ILane
{
    public List<IZombie> _zombies = new List<IZombie>();
    public float _copsSpawnRadius = 2f, _zombieSpawnRadius = 16f;
    public float shotDuration = 0.3f;
    public int _copsAmount, _initialZombiesAmount = 15;
    public float _initialZombieSpawnDelay = 0.15f, _zombieSpawnDelay = 0.33f, _copAttackDelay = 1f;
    public GameObject _copSpawner, _zombieSpawner;
    public Cop _copPrefab;
    public int ZombiesToKill = 100;
    public int ZombiesLeftToKill;
    public TextMeshProUGUI ZOmbieCounter;

    public List<Cop> _cops = new List<Cop>();
    private float _currentShotDelay, _currentZombieSpawnDelay;
    private WaitForSeconds _zombieSpawnDelayWait;

    public float DamagePerShot => 4;

    public float RewardPerZombie => 10;

    public float UpgradeCost => 500;

    public float ZombieHP => 10;

    public Transform ZombieParent => _zombieSpawner.transform;

    private void OnEnable()
    {
        EventsManager.AddListener<IZombie>(EventsType.ZombieDied, OnZombieDied);
        EventsManager.AddListener<ICop>(EventsType.CopShot, OnCopShot);
        ZOmbieCounter.gameObject.SetActive(true);
        ZombiesLeftToKill = ZombiesToKill;
        UpdateZombiesNumber(ZombiesLeftToKill);
        InitBossRoom();
    }


    private void OnDisable()
    {
        EventsManager.RemoveListener<IZombie>(EventsType.ZombieDied, OnZombieDied);
        EventsManager.RemoveListener<ICop>(EventsType.CopShot, OnCopShot);
        ZOmbieCounter.gameObject.SetActive(false);
        StopAllCoroutines();
        Clean();
    }

    private void Clean()
    {
        foreach (Cop cop in _cops)
        {
            Destroy(cop.gameObject);
        }
        _cops.Clear();


        foreach (Transform zombie in _zombieSpawner.transform)
        {
            Destroy(zombie.gameObject);
        }
        _zombies.Clear();
        ZombieSpawner.ClearZombies();
    }


    // Start is called before the first frame update
    private void Start()
    {
    }

    private void InitBossRoom()
    {
        _currentShotDelay = Random.Range(-1.5f, _copAttackDelay);
        _zombieSpawnDelayWait = new WaitForSeconds(_initialZombieSpawnDelay);
        for (int i = 0; i < _copsAmount; i++)
        {
            GameObject cop = Instantiate(_copPrefab.gameObject, _copSpawner.transform);
            Vector2 copPosition = Random.insideUnitCircle * _copsSpawnRadius;
            cop.transform.localPosition = new Vector3(copPosition.x, 0, copPosition.y);
            _cops.Add(cop.GetComponent<Cop>());
        }
        StartCoroutine(InitialZombieSpawn());
    }
    private IEnumerator InitialZombieSpawn()
    {
        Debug.Log("InitialZombieSpawn");
        for (int i = 0; i < _initialZombiesAmount; i++)
        {
            SpawnZombie();
            yield return _zombieSpawnDelayWait;
        }
    }

    private void SpawnZombie()
    {
        ZombieSpawner.Spawn(this, _zombieSpawnRadius, _zombieSpawner);
    }




    // Update is called once per frame
    private void Update()
    {
        _currentShotDelay += Time.deltaTime;
        if (_currentShotDelay >= _copAttackDelay)
        {
            _currentShotDelay = 0;
            StartCoroutine(AllCopsShootRoutine());
        }
        _currentZombieSpawnDelay += Time.deltaTime;
        if (_currentZombieSpawnDelay >= _zombieSpawnDelay)
        {
            _currentZombieSpawnDelay = 0;
            SpawnZombie();
        }
    }

    private IEnumerator AllCopsShootRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(shotDuration / _cops.Count);
        List<Cop> _tempCops = _cops;
        foreach (Cop cop in _tempCops)
        {
            if (cop)
            {
                cop.Shoot();
                yield return delay;
            }
        }
        yield return null;
    }
    private void OnCopShot(ICop obj)
    {
        //if (cop as UnityEngine.Object == _cop && _zombies.Count > 0)
        if (_zombies.Count == 0)
            SpawnZombie();
        _zombies[0].ReceiveDamage(DamagePerShot);
    }
    private void OnZombieDied(IZombie zombie)
    {
        ZombiesLeftToKill--;
        UpdateZombiesNumber(ZombiesLeftToKill);
        if (ZombiesLeftToKill <= 0)
        {
            EventsManager.Broadcast(EventsType.BossKilled);
        }
        _zombies.Remove(zombie);
    }

    public void Init(string name, int zombieAmount, float rewardPerZombie, float damagePerShot, float zombieHP, float zombieSpawnDelay, float shotDelay, float upgradeCost)
    {
        throw new NotImplementedException();
    }

    public void AddZombie(IZombie zombie)
    {
        if (!_zombies.Contains(zombie))
            _zombies.Add(zombie);
    }

    public void LaneClicked()
    {
        StartCoroutine(AllCopsShootRoutine());
    }



    private void UpdateZombiesNumber(int zombiesLeftToKill)
    {
        ZOmbieCounter.text = $"Kill {zombiesLeftToKill.ToString()} \nzombies to finish event";
    }
}
