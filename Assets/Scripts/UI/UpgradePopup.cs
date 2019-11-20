using System;
using UnityEngine;
using UnityEngine.UI;
internal class UpgradePopup : MonoBehaviour
{
    private ILane _currentLane;
    private bool _isUpgradeAvailable = false;
    [SerializeField]
    private Text _upgradeCost, _upgradeText;

    public void Upgrade()
    {
        if (!_isUpgradeAvailable)
            return;
        GameManager.ChangeMoneyAmount(-_currentLane.UpgradeCost);
        UIManager.UpgradeLane();
        UpdateText();
    }
    public void Cancel()
    {
        gameObject.SetActive(false);
    }

    internal void Init(ILane lane)
    {
        gameObject.SetActive(true);
        _currentLane = lane;
        UpdateText();
    }

    private void UpdateText()
    {
        _upgradeCost.text = $"Upgrade price: { ((int)_currentLane.UpgradeCost).ToString()}";
        _upgradeText.text = "Upgrade is too costly for you";
    }

    private void Update()
    {
        _isUpgradeAvailable = _currentLane.UpgradeCost <= GameManager.Money;
        if (_isUpgradeAvailable)
        {
            _upgradeText.text = "Upgrade available";
        }
    }
}