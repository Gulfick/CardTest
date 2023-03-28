using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private Transform _backCard;
    [SerializeField] private Transform _frontCard;

    private void Start()
    {
        
    }

    public async UniTask FlipAnimation()
    {
        _backCard.DOScaleX(0, 0.3f);
        await UniTask.Delay(300);
        _frontCard.DOScaleX(1, 0.3f);
    }

    public void Restart()
    {
        _backCard.DOScaleX(1, 0);
    }
}
