using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Card[] _cards;
    [SerializeField] private TMP_Dropdown _loadDropdown;

    private UniTask[] _tasks;
    private CancellationTokenSource _cancellation;

    private void Awake()
    {
        _tasks = new UniTask[_cards.Length];
    }

    public async void Load()
    {
        _cancellation = new CancellationTokenSource();
        await RestartCards();
        
        try
        {
            switch (_loadDropdown.value)
            {
                //all at once
                case 0:
                    await LoadAtOnce();
                    break;
                //one by one
                case 1:
                    await LoadOneByOne();
                    break;
                //when image ready
                case 2:
                    LoadWhenImageReady();
                    break;
            }
        }
        catch (OperationCanceledException ex)
        {
            Debug.LogWarning("Download was canceled");
        }
    }

    private async UniTask LoadAtOnce()
    {
        for (var i = 0; i < _cards.Length; i++)
        {
            _tasks[i] = _cards[i].LoadToImage(_cancellation.Token);
        }
        
        await UniTask.WhenAll(_tasks);

        for (var i = 0; i < _cards.Length; i++)
        {
            _tasks[i] = _cards[i].FlipToFront();
        }
        
        await UniTask.WhenAll(_tasks);
    }

    private async UniTask LoadOneByOne()
    {
        for (var i = 0; i < _cards.Length; i++)
        {
            _tasks[i] = _cards[i].LoadToImage(_cancellation.Token);
        }

        for (var i = 0; i < _cards.Length; i++)
        {
            await _tasks[i];
            await _cards[i].FlipToFront();
        }
    }

    private void LoadWhenImageReady()
    {
        foreach (var card in _cards)
        {
            _ = card.LoadAndShow(_cancellation.Token);
        }
    }

    private async UniTask RestartCards()
    {
        for (var i = 0; i < _cards.Length; i++)
        {
            _tasks[i] = _cards[i].FlipToBack();
        }
        
        await UniTask.WhenAll(_tasks);

        foreach (var card in _cards)
        {
            card.Dispose();
        }
    }

    public void Cancel()
    {
        _cancellation.Cancel();
    }
}
