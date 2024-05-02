using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PassDepth : MonoBehaviour
{
    [SerializeField]
    public Camera camera;
    [SerializeField]
    public RenderTexture colorRenderTexture;
    [SerializeField]
    public RenderTexture depthRenderTexture;

    // Start is called before the first frame update
    void Start()
    {
        var colorBuffer = colorRenderTexture.colorBuffer;
        var depthBuffer = depthRenderTexture.depthBuffer;

        camera.SetTargetBuffers(colorBuffer, depthBuffer);

        //https://zenn.dev/ryuryu/articles/20210808-depth-capture
        //のコードをそのまま実行
        //camera.depthTextureMode = DepthTextureMode.Depth;
        //camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.Depth);

    }

    // Update is called once per frame
    void Update()
    {
        Graphics.SetRenderTarget(colorRenderTexture);
        camera.Render();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //DisplayTextureInfo();

            //https://zenn.dev/ryuryu/articles/20210808-depth-capture
            //のコードをそのまま実行
            //DepthRenderToPng();


        }
    }

    void DisplayTextureInfo()
    {
        // depthRenderTextureの内容をTexture2Dにコピー
        Texture2D tex = new Texture2D(depthRenderTexture.width, depthRenderTexture.height);
        RenderTexture.active = depthRenderTexture;
        tex.ReadPixels(new Rect(0, 0, depthRenderTexture.width, depthRenderTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        Color[] pixels = tex.GetPixels();

        Debug.Log("(0, 0)" + tex.GetPixel(0, 0));
        Debug.Log("(tex.width, 0)" + tex.GetPixel(tex.width, 0));
        Debug.Log("(0, tex.height)" + tex.GetPixel(0, tex.height));
        Debug.Log("(tex.width, tex.height)" + tex.GetPixel(tex.width, tex.height));
    }

    void DepthRenderToPng()
    {
        Texture2D tex = new Texture2D(Screen.width, Screen.height);
        // RenderTexture.active = camera.targetTexture; // これは無理
        RenderTexture.active = new RenderTexture(tex.width, tex.height, 16);
        Graphics.Blit(camera.targetTexture, RenderTexture.active);

        tex.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/../DepthRenderTexture.png", bytes);
    }
}
