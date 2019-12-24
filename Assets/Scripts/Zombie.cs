using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour, IZombie
{
    private float _health;
    private Animator _animator;
    private IMoveBehaviour _behaviour, _moveBehaviour, _meleeBehaviour;
    private bool _isDead = false;

    public float MoveSpeed { get; private set; }
    public float Reward { get; private set; }
    public GameObject GameObject { get => gameObject; }

    public void Init(float health, float moveSpeed, float reward, IMoveBehaviour moveBehaviour, IMoveBehaviour meleeBehaviour)
    {
        _health = health;
        MoveSpeed = moveSpeed;
        Reward = reward;
        _moveBehaviour = moveBehaviour;
        _meleeBehaviour = meleeBehaviour;
    }

    private void OnEnable()
    {
        if (!_animator)
            _animator = GetComponent<Animator>();
        _isDead = false;
        _animator.SetBool("Dead", false);
        _animator.SetBool("Melee", false);
        _behaviour = _moveBehaviour;
    }
    private void OnDisable()
    {
        ZombieSpawner.Free(this);
    }
    private void Start()
    {
        _behaviour = _moveBehaviour;
    }
    private void Update()
    {
        if (!_isDead)
            _behaviour.Move();
    }

    public void ReceiveDamage(float value)
    {
        _health -= value;
        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        _isDead = true;
        _animator.SetBool("Dead", true);
        EventsManager.Broadcast(EventsType.ZombieDied, this);
    }


    //This method is called by AnimationEvent
    private void OnDied()
    {
        ZombieSpawner.Free(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cop")
            ChangeBehaviour();
    }

    private void ChangeBehaviour()
    {
        _animator.SetBool("Melee", true);
        _behaviour = _meleeBehaviour;
    }
}
