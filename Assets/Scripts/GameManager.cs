using System;
using UnityEngine;
internal class GameManager : MonoBehaviour
{
    private float money = 0f;
    private void OnEnable() => EventsManager.AddListener<Zombie>(EventsType.ZombieDied, OnZombieDied);
    private void OnDisable() => EventsManager.RemoveListener<Zombie>(EventsType.ZombieDied, OnZombieDied);

    private void OnZombieDied(Zombie zombie)
    {
        money += zombie.Reward;
        UIManager.UpdateMoney(money);
    }
}