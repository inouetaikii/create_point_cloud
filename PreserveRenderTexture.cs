using UnityEngine;
using System.Collections;
using System.IO;

public class PreserveRenderTexture : MonoBehaviour
{
    [SerializeField]
    public RenderTexture RenderTextureDep;
    [SerializeField]
    public RenderTexture RenderTextureCol;

    [SerializeField]
    public Camera camera;

    public string[] strings = { "bunny", "bunny (1)", "bunny (2)", "bunny (3)", "bunny (4)", "bunny (5)", "bunny (6)", "bunny (7)", "bunny (8)" };
    public float _repeatSpan;    //繰り返す間隔
    public float _timeElapsed;   //経過時間
    public int count;

    private int fileIndex = 0;
    private string directoryPath = "SavedRenderTexture/";

    // Use this for initialization
    void Start()
    {
        _repeatSpan = 3;    //実行間隔を５に設定
        _timeElapsed = 0;   //経過時間をリセット
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;     //時間をカウントする

        SavePosQuaToText playerscript = GetComponent<SavePosQuaToText>();
        count = playerscript.count;

        if (_timeElapsed >= (2 + strings.Length) * _repeatSpan && count == strings.Length)
        {
            // 深度画像を保存
            SaveDepthRenderTexture();
            Debug.Log("Start SaveDepthRenderTexture");
            // カラー画像を保存
            SaveColorRenderTexture(RenderTextureCol);
            Debug.Log("Start SaveColorRenderTexture");

            count = 0;
            _timeElapsed = 0;
            // 連番を更新する
            fileIndex++;

        }
    }

    void SaveDepthRenderTexture()
    {
        // テクスチャのフォーマットをR16に変更する
        Texture2D tex = new Texture2D(RenderTextureDep.width, RenderTextureDep.height, TextureFormat.R16, false);

        // 現在のレンダーターゲットを覚えておく
        RenderTexture currentTarget = RenderTexture.active;

        // texと同サイズのRenderTextureを用意する
        // depthTextureのフォーマットもR16に変更する
        RenderTexture depthTexture = RenderTexture.GetTemporary(RenderTextureDep.width, RenderTextureDep.height, 0, RenderTextureFormat.R16);

        // 深度をカラーバッファ上にレンダリングするためのマテリアルを作る
        Material blitMaterial = new Material(Shader.Find("Unlit/DistanceInMillimeters"));

        // マテリアルにcameraのニア・ファープレーンの逆数を与える
        blitMaterial.SetVector("_ReciprocalNearFar", new Vector4(1.0f / camera.nearClipPlane, 1.0f / camera.farClipPlane));

        // RenderTextureRefをdepthTexture上にレンダリングする
        Graphics.Blit(RenderTextureDep, depthTexture, blitMaterial);

        // Blitによって現在のレンダーターゲットはdepthTextureに切り替わっているので、その内容をtexに読み取る
        tex.ReadPixels(new Rect(0, 0, RenderTextureDep.width, RenderTextureDep.height), 0, 0);
        tex.Apply();

        // レンダーターゲットを元に戻す
        RenderTexture.active = currentTarget;

        // depthTexture、blitMaterialはもう不要なので削除する
        RenderTexture.ReleaseTemporary(depthTexture);
        Object.Destroy(blitMaterial);

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Object.Destroy(tex);

        //Write to a file in the project folder
        string fileName = "DepthRenderTexture_" + fileIndex.ToString() + ".png";
        string filePath = directoryPath + fileName;
        File.WriteAllBytes(filePath, bytes);

    }

    void SaveColorRenderTexture(RenderTexture rt)
    {
        // アクティブなレンダリングテクスチャを設定
        RenderTexture.active = rt;

        // Texture2Dを作成してデータを読み込む
        Texture2D screenshot = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        screenshot.Apply();

        // 画像をバイトデータに変換
        byte[] bytes = screenshot.EncodeToPNG();

        // ファイルに保存
        string fileName = "ColorRenderTexture_" + fileIndex.ToString() + ".png";
        string filePath = directoryPath + fileName;
        System.IO.File.WriteAllBytes(filePath, bytes);

        // アクティブなレンダリングテクスチャをnullに設定
        RenderTexture.active = null;
    }


}