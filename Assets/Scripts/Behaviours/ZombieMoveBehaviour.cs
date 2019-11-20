using UnityEngine;

internal class ZombieMoveBehaviour : IMoveBehaviour
{
    private IZombie _zombie;
    Transform _transform;

    public ZombieMoveBehaviour(IZombie zombie)
    {
        _zombie = zombie;
        _transform = zombie.GameObject.transform;
    }

    public void Move()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _transform.position+ _transform.forward, _zombie.MoveSpeed * Time.deltaTime);
    }
}
