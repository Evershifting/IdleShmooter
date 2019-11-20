using UnityEngine;

internal class SettingsPopup : MonoBehaviour
{
    public void AddMoneyForCheaters()
    {
        GameManager.ChangeMoneyAmount(1000000f);
    }
}