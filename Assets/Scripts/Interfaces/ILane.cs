using UnityEngine;

public interface ILane
{
    float DamagePerShot { get; }
    float RewardPerZombie { get; }
    float UpgradeCost { get; }
    float ZombieHP { get; }
    Transform ZombieParent { get; }

    void Init(string name, int zombieAmount, float rewardPerZombie, float damagePerShot, float zombieHP, float zombieSpawnDelay, float shotDelay, float upgradeCost);
    void AddZombie(IZombie zombie);
    void LaneClicked();
}