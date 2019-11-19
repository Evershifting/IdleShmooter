using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneGenerator : MonoBehaviour
{
    [SerializeField]
    private Lane _lanePrefab;
    [SerializeField]
    private int _laneAmount = 20;

    private void Start()
    {
        for (int i = 0; i < _laneAmount; i++)
        {
            Lane lane = Instantiate(_lanePrefab, transform.position + Vector3.forward * 2.5f * i, Quaternion.identity, transform);
            lane.name = $"{lane.name} {i.ToString()}";
        }
    }
}
