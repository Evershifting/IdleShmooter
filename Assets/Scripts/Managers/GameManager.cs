using System;
using UnityEngine;
internal class GameManager : MonoBehaviour
{
    public static float Money { get; private set; } = 0f;

    private void OnEnable() => EventsManager.AddListener<IZombie>(EventsType.ZombieDied, OnZombieDied);
    private void OnDisable() => EventsManager.RemoveListener<IZombie>(EventsType.ZombieDied, OnZombieDied);

    private void OnZombieDied(IZombie zombie)
    {
        ChangeMoneyAmount(zombie.Reward);
    }

    public static void ChangeMoneyAmount(float value)
    {
        Money += value;
        UIManager.UpdateMoney(Money);
    }
}