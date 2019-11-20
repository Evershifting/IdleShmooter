using UnityEngine;

internal class ZombieMoveBehaviour : IMoveBehaviour
{
    private IZombie _zombie;
    Transform _transform;

    public ZombieMoveBehaviour(IZombie zombie, Transform transform)
    {
        _zombie = zombie;
        _transform = transform;
    }

    public void Move()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _transform.position+ _transform.forward, _zombie.MoveSpeed * Time.deltaTime);
    }
}
