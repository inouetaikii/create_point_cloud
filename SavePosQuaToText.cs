using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavePosQuaToText : MonoBehaviour
{
    public string[] strings = { "bunny", "bunny (1)", "bunny (2)", "bunny (3)", "bunny (4)", "bunny (5)", "bunny (6)", "bunny (7)", "bunny (8)" };
    public float _repeatSpan;    //�J��Ԃ��Ԋu
    public float _timeElapsed;   //�o�ߎ���
    public int count;

    private int fileIndex = 0;
    private string directoryPath = "SavedTextFiles/";

    // Start is called before the first frame update
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

        //�o�ߎ��Ԃ��J��Ԃ��Ԋu���o�߂�����
        if (_timeElapsed >= _repeatSpan�@&& count < strings.Length)
        {
            //�����ŏ��������s
            string ObjectName = strings[count];
            GameObject FocusObject = GameObject.Find(ObjectName);
            float angle_x = UnityEngine.Random.Range(0, 360);
            float angle_y = UnityEngine.Random.Range(0, 360);
            float angle_z = UnityEngine.Random.Range(0, 360);
            float pos_x = UnityEngine.Random.Range(-2.0f, 2.0f);
            float pos_z = UnityEngine.Random.Range(-2.0f, 2.0f);
            FocusObject.transform.position = new Vector3(pos_x, 10.0f, pos_z);
            FocusObject.transform.Rotate(angle_x, angle_y, angle_z);
            count = count + 1;

            _timeElapsed = 0;   //�o�ߎ��Ԃ����Z�b�g����
        }

        if (_timeElapsed >= 2 * _repeatSpan && count == strings.Length)
        {
            SaveToText();
            fileIndex++;
            _timeElapsed = 0;   //�o�ߎ��Ԃ����Z�b�g����
            count = 0;
        }

    }

    void SaveToText()
    {
        Debug.Log("START Append Text");

        //string[] strings = { "bunny", "bunny (1)", "bunny (2)", "bunny (3)", "bunny (4)", "bunny (5)", "bunny (6)", "bunny (7)", "bunny (8)" };

        for (int i = 0; i < strings.Length; i++)
        {
            string ObjectName = strings[i];
            GameObject FocusObject = GameObject.Find(ObjectName);

            string fileNamepos = "PosFile_" + fileIndex.ToString() + ".txt";
            string filePathpos = directoryPath + fileNamepos;
            string fileNamequa = "QuaFile_" + fileIndex.ToString() + ".txt";
            string filePathqua = directoryPath + fileNamequa;

            SavePosition(FocusObject, filePathpos);
            SaveQuaternionXYZ(FocusObject, filePathqua);
            //SaveTextFile(FocusObject);
        }

        Debug.Log("FINISH Append Text");
    }

    
    void SavePosition(GameObject FocusObject, string filePath)
    {
        Vector3 pos = FocusObject.transform.position;
        File.AppendAllText(filePath, Convert.ToString(pos.x) + "," + Convert.ToString(pos.y) + "," + Convert.ToString(pos.z) + "\n");
    }
    void SaveQuaternionXYZ(GameObject FocusObject, string filePathqua)
    {
        float x = FocusObject.transform.localEulerAngles.x;
        float y = FocusObject.transform.localEulerAngles.y;
        float z = FocusObject.transform.localEulerAngles.z;
        var rotX = Quaternion.AngleAxis(x, new Vector3(1, 0, 0));
        //var rotY = Quaternion.AngleAxis(y, new Vector3(0, 1, 0));
        //var rotZ = Quaternion.AngleAxis(z, new Vector3(0, 0, 1));
        var rotY = Quaternion.AngleAxis(z, new Vector3(0, 1, 0));
        var rotZ = Quaternion.AngleAxis(y, new Vector3(0, 0, 1));
        File.AppendAllText(filePathqua, Convert.ToString(rotX.x) + "," + Convert.ToString(rotX.y) + "," + Convert.ToString(rotX.z) + "," + Convert.ToString(rotX.w) + "\n");
        File.AppendAllText(filePathqua, Convert.ToString(rotY.x) + "," + Convert.ToString(rotY.y) + "," + Convert.ToString(rotY.z) + "," + Convert.ToString(rotY.w) + "\n");
        File.AppendAllText(filePathqua, Convert.ToString(rotZ.x) + "," + Convert.ToString(rotZ.y) + "," + Convert.ToString(rotZ.z) + "," + Convert.ToString(rotZ.w) + "\n");
    }

    public void SaveTextFile(GameObject FocusObject)
    {
        string fileName = "TextFile_" + fileIndex.ToString() + ".txt";
        string filePath = directoryPath + fileName;

        Vector3 pos = FocusObject.transform.position;
        File.AppendAllText(filePath, Convert.ToString(pos.x) + "," + Convert.ToString(pos.y) + "," + Convert.ToString(pos.z) + "\n");

        // �A�Ԃ��X�V����
        fileIndex++;
    }
}
