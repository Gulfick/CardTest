using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownloadImage : MonoBehaviour
{
    private async void Start()
    {
        var img = GetComponent<RawImage>();
        img.texture = await GetTexture("https://picsum.photos/170/200");
    }

    private async UniTask<Texture2D> GetTexture(string uri)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(uri))
        {
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                return null;
            }
            else
            {
                 return DownloadHandlerTexture.GetContent(request);
            }
        }
    }
}
