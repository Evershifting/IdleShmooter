using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _money;
    private static Text _moneyRef;
    private void Start()
    {
        _moneyRef = _money;
    }
    public static void UpdateMoney(float value)
    {
        if (_moneyRef)
            _moneyRef.text = value.ToString();
    }
}
