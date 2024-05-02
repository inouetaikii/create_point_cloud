using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavePosQuaToText : MonoBehaviour
{
    public string[] strings = { "bunny", "bunny (1)", "bunny (2)", "bunny (3)", "bunny (4)", "bunny (5)", "bunny (6)", "bunny (7)", "bunny (8)" };
    public float _repeatSpan;    //繰り返す間隔
    public float _timeElapsed;   //経過時間
    public int count;

    private int fileIndex = 0;
    private string directoryPath = "SavedTextFiles/";

    // Start is called before the first frame update
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

        //経過時間が繰り返す間隔を経過したら
        if (_timeElapsed >= _repeatSpan　&& count < strings.Length)
        {
            //ここで処理を実行
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

            _timeElapsed = 0;   //経過時間をリセットする
        }

        if (_timeElapsed >= 2 * _repeatSpan && count == strings.Length)
        {
            SaveToText();
            fileIndex++;
            _timeElapsed = 0;   //経過時間をリセットする
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

        // 連番を更新する
        fileIndex++;
    }
}
