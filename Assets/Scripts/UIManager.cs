using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _money;
    [SerializeField]
    private GameObject _upgradePopup, _settingspopup;

    private static GameObject _upgradePopupRef, _settingspopupRef;
    private static Text _moneyRef;
    private static bool isCameraMoving = false;
    private static Action LaneUpgrade;
    private void Start()
    {
        _moneyRef = _money;
        _upgradePopupRef = _upgradePopup;
        _settingspopupRef = _settingspopup;
    }
    public static void UpdateMoney(float value)
    {
        if (_moneyRef)
            _moneyRef.text = value.ToString();
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

    public static void UpgradeLane(Action upgradeLane)
    {
        LaneUpgrade = upgradeLane;
        _upgradePopupRef.SetActive(true);
    }

    public static void UpgradeLane()
    {
        _upgradePopupRef.SetActive(false);
        LaneUpgrade?.Invoke();
    }
    public void SwitchLane(bool up)
    {
        if (!isCameraMoving)
            if (!up)
            {
                if (Camera.main.transform.position.z > 0)
                {
                    isCameraMoving = true;
                    Tween t = Camera.main.transform.DOMove(Camera.main.transform.position - Vector3.forward * 2.5f, 0.2f);
                    t.onComplete += () => isCameraMoving = false;
                    t.Play();
                }
            }
            else
            {
                isCameraMoving = true;
                Tween t = Camera.main.transform.DOMove(Camera.main.transform.position + Vector3.forward * 2.5f, 0.2f);
                t.onComplete += () => isCameraMoving = false;
                t.Play();
            }
    }
}
