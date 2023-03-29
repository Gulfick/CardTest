using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class Request
{
    public static async UniTask<Texture2D> GetTexture(string uri, CancellationToken cancellationToken)
    {
        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(uri);
        var asyncOp = request.SendWebRequest();

        await asyncOp.WithCancellation(cancellationToken);
        
        if(request.result == UnityWebRequest.Result.Success)
            return DownloadHandlerTexture.GetContent(asyncOp.webRequest);
        
        //In other case there's error
        Debug.LogError(request.error);
        return null;
    }
}
