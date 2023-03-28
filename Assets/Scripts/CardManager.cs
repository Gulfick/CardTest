using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Card[] _cards;
    [SerializeField] private TMP_Dropdown _loadDropdown;
    

    private UniTask[] _tasks;
    private bool _isLoaded;

    private void Awake()
    {
        _tasks = new UniTask[_cards.Length];
    }

    public async void Load()
    {
        if (_isLoaded)
        {
            await RestartCards();
        }
        
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
                await LoadWhenImageReady();
                break;
        }
    }

    private async UniTask LoadAtOnce()
    {
        for (var i = 0; i < _cards.Length; i++)
        {
            _tasks[i] = _cards[i].LoadToImage();
        }
        
        await UniTask.WhenAll(_tasks);

        for (var i = 0; i < _cards.Length; i++)
        {
            _tasks[i] = _cards[i].FlipToFront();
        }
        
        await UniTask.WhenAll(_tasks);
        
        _isLoaded = true;
    }

    private async UniTask LoadOneByOne()
    {
        foreach (var card in _cards)
        {
            await card.LoadAndShow();
        }

        _isLoaded = true;
    }

    private async UniTask LoadWhenImageReady()
    {
        foreach (var card in _cards)
        {
            await card.LoadAndShow();
        }
        
        _isLoaded = true;
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

        _isLoaded = false;
    }
}
