using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private static SettingsPopup _settingspopupRef;
    private static UpgradePopup _upgradePopupRef;
    private static Text _moneyRef;
    private static bool isCameraMoving = false;
    private static Action LaneUpgrade;

    [SerializeField]
    private Text _money;
    [SerializeField]
    private UpgradePopup _upgradePopup;
    [SerializeField]
    private SettingsPopup _settingsPopup;
    private Vector3 _startingCameraPosition;
    private Settings _settings;

    private void Awake()
    {
        if (!_settings)
            _settings = Resources.Load<Settings>("Settings");
        _startingCameraPosition = Camera.main.transform.position;
    }
    private void Start()
    {
        _moneyRef = _money;
        _upgradePopupRef = _upgradePopup;
        _settingspopupRef = _settingsPopup;
    }
    public static void UpdateMoney(float value)
    {
        if (_moneyRef)
            _moneyRef.text = $"$: {((int)value).ToString()}";
    }


    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    RaycastAnalysis(hit);
                }
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit, 1000.0f))
                {
                    RaycastAnalysis(hit);
                }
            }
        }
    }

    private static void RaycastAnalysis(RaycastHit hit)
    {
        if (hit.collider.tag == "Lane")
        {
            if (hit.collider.gameObject.transform.parent.GetComponentInParent<Lane>())
            {
                hit.collider.gameObject.transform.parent.GetComponentInParent<Lane>().Shoot();
            }
        }

        if (hit.collider.tag == "Cop")
        {
            if (hit.collider.GetComponent<Cop>())
            {
                hit.collider.GetComponent<Cop>().CopClicked();
            }
        }
    }

    public static void UpgradeLane(Lane lane, Action upgradeLane)
    {
        LaneUpgrade = upgradeLane;
        _upgradePopupRef.Init(lane);
    }

    public static void UpgradeLane()
    {
        LaneUpgrade?.Invoke();
    }
    public void SwitchLane(bool up)
    {
        if (!isCameraMoving)
            if (up)
            {
                if (Camera.main.transform.position.z < _startingCameraPosition.z + _settings.LaneSpacing * _settings.LanesAmount)
                {
                    isCameraMoving = true;
                    Tween t = Camera.main.transform.DOMove(Camera.main.transform.position + Vector3.forward * _settings.LaneSpacing, 0.2f);
                    t.onComplete += () => isCameraMoving = false;
                    t.Play();
                }
            }
            else
            {
                if (Camera.main.transform.position.z > _startingCameraPosition.z)
                {
                    isCameraMoving = true;
                    Tween t = Camera.main.transform.DOMove(Camera.main.transform.position - Vector3.forward * _settings.LaneSpacing, 0.2f);
                    t.onComplete += () => isCameraMoving = false;
                    t.Play();
                }
            }
    }
}
