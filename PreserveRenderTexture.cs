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
    public float _repeatSpan;    //�J��Ԃ��Ԋu
    public float _timeElapsed;   //�o�ߎ���
    public int count;

    private int fileIndex = 0;
    private string directoryPath = "SavedRenderTexture/";

    // Use this for initialization
    void Start()
    {
        _repeatSpan = 3;    //���s�Ԋu���T�ɐݒ�
        _timeElapsed = 0;   //�o�ߎ��Ԃ����Z�b�g
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;     //���Ԃ��J�E���g����

        SavePosQuaToText playerscript = GetComponent<SavePosQuaToText>();
        count = playerscript.count;

        if (_timeElapsed >= (2 + strings.Length) * _repeatSpan && count == strings.Length)
        {
            // �[�x�摜��ۑ�
            SaveDepthRenderTexture();
            Debug.Log("Start SaveDepthRenderTexture");
            // �J���[�摜��ۑ�
            SaveColorRenderTexture(RenderTextureCol);
            Debug.Log("Start SaveColorRenderTexture");

            count = 0;
            _timeElapsed = 0;
            // �A�Ԃ��X�V����
            fileIndex++;

        }
    }

    void SaveDepthRenderTexture()
    {
        // �e�N�X�`���̃t�H�[�}�b�g��R16�ɕύX����
        Texture2D tex = new Texture2D(RenderTextureDep.width, RenderTextureDep.height, TextureFormat.R16, false);

        // ���݂̃����_�[�^�[�Q�b�g���o���Ă���
        RenderTexture currentTarget = RenderTexture.active;

        // tex�Ɠ��T�C�Y��RenderTexture��p�ӂ���
        // depthTexture�̃t�H�[�}�b�g��R16�ɕύX����
        RenderTexture depthTexture = RenderTexture.GetTemporary(RenderTextureDep.width, RenderTextureDep.height, 0, RenderTextureFormat.R16);

        // �[�x���J���[�o�b�t�@��Ƀ����_�����O���邽�߂̃}�e���A�������
        Material blitMaterial = new Material(Shader.Find("Unlit/DistanceInMillimeters"));

        // �}�e���A����camera�̃j�A�E�t�@�[�v���[���̋t����^����
        blitMaterial.SetVector("_ReciprocalNearFar", new Vector4(1.0f / camera.nearClipPlane, 1.0f / camera.farClipPlane));

        // RenderTextureRef��depthTexture��Ƀ����_�����O����
        Graphics.Blit(RenderTextureDep, depthTexture, blitMaterial);

        // Blit�ɂ���Č��݂̃����_�[�^�[�Q�b�g��depthTexture�ɐ؂�ւ���Ă���̂ŁA���̓��e��tex�ɓǂݎ��
        tex.ReadPixels(new Rect(0, 0, RenderTextureDep.width, RenderTextureDep.height), 0, 0);
        tex.Apply();

        // �����_�[�^�[�Q�b�g�����ɖ߂�
        RenderTexture.active = currentTarget;

        // depthTexture�AblitMaterial�͂����s�v�Ȃ̂ō폜����
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
        // �A�N�e�B�u�ȃ����_�����O�e�N�X�`����ݒ�
        RenderTexture.active = rt;

        // Texture2D���쐬���ăf�[�^��ǂݍ���
        Texture2D screenshot = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        screenshot.Apply();

        // �摜���o�C�g�f�[�^�ɕϊ�
        byte[] bytes = screenshot.EncodeToPNG();

        // �t�@�C���ɕۑ�
        string fileName = "ColorRenderTexture_" + fileIndex.ToString() + ".png";
        string filePath = directoryPath + fileName;
        System.IO.File.WriteAllBytes(filePath, bytes);

        // �A�N�e�B�u�ȃ����_�����O�e�N�X�`����null�ɐݒ�
        RenderTexture.active = null;
    }


}