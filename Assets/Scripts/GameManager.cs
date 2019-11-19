using System;
using UnityEngine;
internal class GameManager : MonoBehaviour
{
    public static float Money { get; private set; } = 0f;

    private void OnEnable() => EventsManager.AddListener<Zombie>(EventsType.ZombieDied, OnZombieDied);
    private void OnDisable() => EventsManager.RemoveListener<Zombie>(EventsType.ZombieDied, OnZombieDied);

    private void OnZombieDied(Zombie zombie)
    {
        ChangeMoneyAmount(zombie.Reward);
    }

    public static void ChangeMoneyAmount(float value)
    {
        Money += value;
        UIManager.UpdateMoney(Money);
    }
}