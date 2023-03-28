using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class Request
{
    public static async UniTask<Texture2D> GetTexture(string uri)
    {
        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(uri);
        
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) 
            return DownloadHandlerTexture.GetContent(request);
        
        //In other case there's error
        Debug.LogError(request.error);
        return null;
    }
}
