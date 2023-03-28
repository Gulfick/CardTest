using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Transform _backCard;
    [SerializeField] private Transform _frontCard;
    [SerializeField] private RawImage _cardImage;
    [SerializeField] private float _animTime = 0.2f;
    [SerializeField] private string _imgUrl = "https://picsum.photos/170/200";

    private Texture2D _texture;
    
    private void Start()
    {
        Restart();
    }

    public void Dispose()
    {
        Destroy(_texture);
    }
    
    public async UniTask<bool> LoadToImage()
    {
        _texture = await Request.GetTexture(_imgUrl);
        if (_texture != null)
        {
            _cardImage.texture = _texture;
            return true;
        }
        else
        {
            return false;
        }
    }

    public async UniTask LoadAndShow()
    {
        await LoadToImage();
        await FlipToFront();
    }

    public async UniTask FlipToFront()
    {
        await Flip(_backCard, _frontCard);
    }

    public async UniTask FlipToBack()
    {
        await Flip(_frontCard, _backCard);
    }

    private async UniTask Flip(Transform from, Transform to)
    {
        from.DOScaleX(0, _animTime);
        await UniTask.Delay((int)(_animTime * 1000));
        from.gameObject.SetActive(false);
        to.gameObject.SetActive(true);
        to.DOScaleX(1, _animTime);
    }

    public void Restart()
    {
        _backCard.DOScaleX(1, 0);
        _backCard.gameObject.SetActive(true);
        _frontCard.DOScaleX(0, 0);
        _frontCard.gameObject.SetActive(false);
    }
}
