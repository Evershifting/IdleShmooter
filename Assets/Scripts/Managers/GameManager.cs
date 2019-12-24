using System;
using UnityEngine;
using Random = UnityEngine.Random;

internal class GameManager : MonoBehaviour
{
    public static float Money { get; private set; } = 0f;
    public static float PassiveIncome
    {
        get
        {
            Debug.LogError("Add Passive Income Logic!");
            return 1f;
        }
    }

    public GameObject GameField, BossRoom, CameraGameField, CameraBoss;

    Bonuses _bonuses;

    private bool _isDoubleIncome;
    private float _instantMoneySpawnCurrentTimer = 45;

    private void OnEnable()
    {
        EventsManager.AddListener<IZombie>(EventsType.ZombieDied, OnZombieDied);
        EventsManager.AddListener<bool>(EventsType.DoubleIncome, OnDoubleIncome);
        EventsManager.AddListener(EventsType.InstantMoney, OnInstantMoney);
        EventsManager.AddListener(EventsType.BossKilled, BossFight);
    }
    private void OnDisable()
    {
        EventsManager.RemoveListener<IZombie>(EventsType.ZombieDied, OnZombieDied);
        EventsManager.RemoveListener<bool>(EventsType.DoubleIncome, OnDoubleIncome);
        EventsManager.RemoveListener(EventsType.InstantMoney, OnInstantMoney);
        EventsManager.RemoveListener(EventsType.BossKilled, BossFight);
    }
    private void Awake()
    {
        if (!_bonuses)
            _bonuses = Resources.Load<Bonuses>("Bonuses");
    }
    private void Update()
    {
        _instantMoneySpawnCurrentTimer += Time.deltaTime;
        if (_instantMoneySpawnCurrentTimer >_bonuses.InstantMoneySpawn)
        {
            BonusSpawner.Instance.SpawnInstantMoney();
            _instantMoneySpawnCurrentTimer = Random.Range(-_bonuses.InstantMoneySpawn * .5f, _bonuses.InstantMoneySpawn * .5f);
        }
    }

    public static void ChangeMoneyAmount(float value)
    {
        Money += value;
        UIManager.UpdateMoney(Money);
    }

    public void BossFight()
    {
        CameraGameField.SetActive(!GameField.activeInHierarchy);
        GameField.SetActive(!GameField.activeInHierarchy);
        CameraBoss.SetActive(!BossRoom.activeInHierarchy);
        BossRoom.SetActive(!BossRoom.activeInHierarchy);
    }

    private void OnInstantMoney()
    {
        ChangeMoneyAmount(_bonuses.InstantMoney);
    }
    private void OnZombieDied(IZombie zombie)
    {
        ChangeMoneyAmount(_isDoubleIncome ? zombie.Reward * 2 : zombie.Reward);
    }
    private void OnDoubleIncome(bool value)
    {
        _isDoubleIncome = value;
    }
    }