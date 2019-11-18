using UnityEngine;

internal class ZombieMoveBehaviour : IMoveBehaviour
{
    private Zombie _zombie;

    public ZombieMoveBehaviour(Zombie zombie)
    {
        _zombie = zombie;
    }

    public void Move()
    {
        _zombie.transform.position = Vector3.MoveTowards(_zombie.transform.position, _zombie.transform.position+_zombie.transform.forward, _zombie.MoveSpeed * Time.deltaTime);
    }
}
