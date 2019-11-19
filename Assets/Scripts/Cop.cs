using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : MonoBehaviour
{
    public Animator Animator { get; private set; }
    private void Awake()
    {
        if (!Animator)
            Animator = GetComponent<Animator>();
    }
    
    public void Shoot()
    {
        Animator.SetTrigger("Shoot");
        EventsManager.Broadcast(EventsType.CopShot, this);
    }

    public void CopClicked()
    {
        EventsManager.Broadcast(EventsType.CopClicked, this);
    }
}
