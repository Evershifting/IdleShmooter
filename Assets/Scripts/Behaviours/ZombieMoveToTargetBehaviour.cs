using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMoveToTargetBehaviour : IMoveBehaviour
{
    private IZombie _zombie;
    Transform _transform, _target;

    public ZombieMoveToTargetBehaviour(IZombie zombie, Transform target)
    {
        _zombie = zombie;
        _transform = zombie.GameObject.transform;
        _target = target;
    }

    public void Move()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _target.transform.position, _zombie.MoveSpeed * Time.deltaTime);
    }
}
