using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private int health, moveSpeed, reward;
    private Animator _animator;
    private IMoveBehaviour _behaviour, _moveBehaviour, _meleeBehaviour;
    private bool isDead = false;

    public void Init(int health, int moveSpeed, int reward, IMoveBehaviour moveBehaviour, IMoveBehaviour meleeBehaviour)
    {
        this.health = health;
        this.moveSpeed = moveSpeed;
        this.reward = reward;
        this._moveBehaviour = moveBehaviour;
        this._meleeBehaviour = meleeBehaviour;
    }

    private void Start()
    {
        _behaviour = _moveBehaviour;
    }
    private void Update()
    {
        if (!isDead)
            _behaviour.Move();
    }

    public void ReceiveDamage(int value)
    {
        health -= value;
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        _animator.SetBool("isDead", true);
    }

    public void OnDied()
    {
        ZombieSpawner.Free(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MeleeZone")
            ChangeBehaviour();
    }

    private void ChangeBehaviour()
    {
        _behaviour = _meleeBehaviour;
    }
}
