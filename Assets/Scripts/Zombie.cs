﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private float health, moveSpeed, reward;
    private Animator _animator;
    private IMoveBehaviour _behaviour, _moveBehaviour, _meleeBehaviour;
    private bool _isDead = false;

    public float Health { get => health; }
    public float MoveSpeed { get => moveSpeed; }
    public float Reward { get => reward; }

    public void Init(float health, float moveSpeed, float reward, IMoveBehaviour moveBehaviour, IMoveBehaviour meleeBehaviour)
    {
        this.health = health;
        this.moveSpeed = moveSpeed;
        this.reward = reward;
        this._moveBehaviour = moveBehaviour;
        this._meleeBehaviour = meleeBehaviour;
    }

    private void OnEnable()
    {
        if (!_animator)
            _animator = GetComponent<Animator>();
        //_animator.runtimeAnimatorController.
        _isDead = false;
        _animator.SetBool("Dead", false);
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
        health -= value;
        if (health <= 0)
           Die();
    }

    private void Die()
    {
        _isDead = true;
        _animator.SetBool("Dead", true);
    }

    private void OnDied()
    {
        EventsManager.Broadcast(EventsType.ZombieDied, this);
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
