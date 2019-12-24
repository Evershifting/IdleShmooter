using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    private Action _clickBehaviour;
    private Tween tween;
    public void Init(Action clickBehaviour, float duration)
    {
        _clickBehaviour = clickBehaviour;
        tween = transform.DOMove(transform.position - new Vector3(0, UIManager.Instance.GetComponent<Canvas>().pixelRect.height, 0), duration);
        tween.onComplete += () => Destroy(gameObject);
    }

    public void OnClick()
    {
        _clickBehaviour?.Invoke();
        tween.Kill();
        Destroy(gameObject);
    }
}
