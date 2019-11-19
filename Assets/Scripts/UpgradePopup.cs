using UnityEngine;
internal class UpgradePopup : MonoBehaviour
{
    public void Upgrade()
    {
        UIManager.UpgradeLane();
    }
    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}