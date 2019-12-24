using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    public static BonusSpawner Instance;

    [SerializeField]
    private Bonus _doubleDamagePrefab, _doubleIncomePrefab, _instantMoneyPrefab;

    private float _doubleDamageCurrentDuration, _doubleIncomeCurrentDuration;
    private Bonuses _bonuses;
    private Coroutine _doubleDamageRoutine, _doubleIncomeRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogWarning("Destroyed additional BonusSpawner");
            Destroy(gameObject);
        }
        if (!_bonuses)
            _bonuses = Resources.Load<Bonuses>("Bonuses");
    }

    #region Double Damage
    public void SpawnDoubleDamage()
    {
        Bonus spawnedBonus = Instantiate(_doubleDamagePrefab, UIManager.Instance.gameObject.transform);
        spawnedBonus.Init(DoubleDamage, _bonuses.InstantMoneyFallDuration);
    }

    public void DoubleDamage()
    {
        if (_doubleDamageRoutine == null)
        {
            _doubleDamageRoutine = StartCoroutine(DoubleDamageRoutine());
        }
        else
            _doubleDamageCurrentDuration += _bonuses.DoubleDamage;
    }

    private IEnumerator DoubleDamageRoutine()
    {
        EventsManager.Broadcast(EventsType.DoubleDamage, true);
        _doubleDamageCurrentDuration = _bonuses.DoubleDamage;
        while (_doubleDamageCurrentDuration > 0)
        {
            _doubleDamageCurrentDuration -= Time.deltaTime;
            UIManager.Instance.SetDoubleDamageTimer(_doubleDamageCurrentDuration);
            yield return null;
        }
        EventsManager.Broadcast(EventsType.DoubleDamage, false);
    }
    #endregion
    #region Double Income
    public void SpawnDoubleIncome()
    {
        Bonus spawnedBonus = Instantiate(_doubleIncomePrefab, UIManager.Instance.gameObject.transform);
        spawnedBonus.Init(DoubleIncome, _bonuses.InstantMoneyFallDuration);
    }

    public void DoubleIncome()
    {
        if (_doubleIncomeRoutine == null)
        {
            _doubleIncomeRoutine = StartCoroutine(DoubleIncomeRoutine());
        }
        else
            _doubleIncomeCurrentDuration += _bonuses.DoubleIncome;
    }

    private IEnumerator DoubleIncomeRoutine()
    {
        EventsManager.Broadcast(EventsType.DoubleIncome, true);
        _doubleIncomeCurrentDuration = _bonuses.DoubleIncome;
        while (_doubleIncomeCurrentDuration > 0)
        {
            _doubleIncomeCurrentDuration -= Time.deltaTime;
            UIManager.Instance.SetDoubleIncomeTimer(_doubleIncomeCurrentDuration);
            yield return null;
        }
        EventsManager.Broadcast(EventsType.DoubleIncome, false);
    }
    #endregion
    #region InstantMoney
    public void SpawnInstantMoney()
    {
        Bonus spawnedBonus = Instantiate(_instantMoneyPrefab, UIManager.Instance.gameObject.transform);
        spawnedBonus.Init(() => EventsManager.Broadcast(EventsType.InstantMoney), _bonuses.InstantMoneyFallDuration);
    }
    #endregion
}
