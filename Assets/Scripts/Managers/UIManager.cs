using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private static SettingsPopup _settingspopupRef;
    private static UpgradePopup _upgradePopupRef;
    private static Text _moneyRef;
    private static bool isCameraMoving = false;
    private static Action LaneUpgrade;

    [SerializeField]
    private float _cameraSpeed = 0.3f;
    [SerializeField]
    private Text _money;
    [SerializeField]
    private TextMeshProUGUI _doubleDamageDuration, _doubleIncomeDuration;
    [SerializeField]
    private UpgradePopup _upgradePopup;
    [SerializeField]
    private SettingsPopup _settingsPopup;
    private Vector3 _startingCameraPosition;
    private Settings _settings;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogWarning("Destroyed additional UIManager");
            Destroy(gameObject);
        }

        if (!_settings)
            _settings = Resources.Load<Settings>("Settings");
        _startingCameraPosition = Camera.main.transform.position;
    }
    private void Start()
    {
        _moneyRef = _money;
        _upgradePopupRef = _upgradePopup;
        _settingspopupRef = _settingsPopup;
        SetDoubleDamageTimer(0);
        SetDoubleIncomeTimer(0);
    }
    public static void UpdateMoney(float value)
    {
        if (_moneyRef)
            _moneyRef.text = $"$: {((int)value).ToString()}";

    }


    private void Update()
    {
        if (!GameManager.isBossFight)
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                Camera.main.transform.Translate(0, -touchDeltaPosition.y * _cameraSpeed * Time.deltaTime, 0);
            }
        if (Camera.main.transform.position.y >= _startingCameraPosition.y + _settings.LaneSpacing * _settings.LanesAmount)
        {
            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x,
                _startingCameraPosition.y + _settings.LaneSpacing * _settings.LanesAmount,
                Camera.main.transform.position.z);
        }
        if (Camera.main.transform.position.y <= _startingCameraPosition.y)
        {
            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x,
                _startingCameraPosition.y,
                Camera.main.transform.position.z);
        }

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
            if (hit.collider.gameObject.transform.parent.GetComponentInParent<ILane>() != null)
            {
                hit.collider.gameObject.transform.parent.GetComponentInParent<ILane>().LaneClicked();
            }
        }

        if (hit.collider.tag == "Cop")
        {
            if (hit.collider.GetComponent<ICop>() != null)
            {
                hit.collider.GetComponent<ICop>().CopClicked();
            }
        }
    }

    public static void UpgradeLane(ILane lane, Action upgradeLane)
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
                if (Camera.main.transform.position.y < _startingCameraPosition.y + _settings.LaneSpacing * _settings.LanesAmount)
                {
                    isCameraMoving = true;
                    Tween t = Camera.main.transform.DOMove(Camera.main.transform.position + Vector3.up * _settings.LaneSpacing, 0.2f);
                    t.onComplete += () => isCameraMoving = false;
                    t.Play();
                }
            }
            else
            {
                if (Camera.main.transform.position.y > _startingCameraPosition.y)
                {
                    isCameraMoving = true;
                    Tween t = Camera.main.transform.DOMove(Camera.main.transform.position - Vector3.up * _settings.LaneSpacing, 0.2f);
                    t.onComplete += () => isCameraMoving = false;
                    t.Play();
                }
            }
    }
    public void SetDoubleDamageTimer(float value)
    {
        _doubleDamageDuration.gameObject.SetActive(value > 0);
        TimeSpan t = TimeSpan.FromSeconds(value);
        _doubleDamageDuration.text = t.ToString(@"mm\:ss");
    }
    public void SetDoubleIncomeTimer(float value)
    {
        _doubleIncomeDuration.gameObject.SetActive(value > 0);
        TimeSpan t = TimeSpan.FromSeconds(value);
        _doubleIncomeDuration.text = t.ToString(@"mm\:ss");
    }
}
