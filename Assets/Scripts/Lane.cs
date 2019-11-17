using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    private int rewardPerZombie, damagePerShot, zombieHP;
    public Transform zombieParent;

    public int RewardPerZombie { get => rewardPerZombie;}
    public int DamagePerShot { get => damagePerShot;}
    public int ZombieHP { get => zombieHP;}

    private void SpawnZombie()
    {
        ZombieSpawner.Spawn(this);
    }
}
