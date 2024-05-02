using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class Iteration : MonoBehaviour
{
    public string[] strings = { "bunny", "bunny (1)", "bunny (2)", "bunny (3)", "bunny (4)", "bunny (5)", "bunny (6)", "bunny (7)", "bunny (8)" };
    public float _repeatSpan;    //�J��Ԃ��Ԋu
    public float _timeElapsed;   //�o�ߎ���
    public int count;
    public Vector3[] bunnyInitPos;

    // Start is called before the first frame update
    void Start()
    {
        _repeatSpan = 3;    //���s�Ԋu���T�ɐݒ�
        _timeElapsed = 0;   //�o�ߎ��Ԃ����Z�b�g
        count = 0;
        bunnyInitPos = new Vector3[strings.Length];

        for (int i = 0; i < strings.Length; i++)
        {
            string ObjectName = strings[i];
            GameObject FocusObject = GameObject.Find(ObjectName);
            bunnyInitPos[i] = FocusObject.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;     //���Ԃ��J�E���g����


        //�o�ߎ��Ԃ��J��Ԃ��Ԋu���o�߂�����
        if (_timeElapsed >= _repeatSpan && count < strings.Length)
        {
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
            Debug.Log("_repeatSpan");
        }
        if (_timeElapsed >= 2 * _repeatSpan && count == strings.Length)
        {
            Debug.Log("SaveTime");
            for (int i = 0; i < strings.Length; i++)
            {
                string ObjectName = strings[i];
                GameObject FocusObject = GameObject.Find(ObjectName);
                FocusObject.transform.position = bunnyInitPos[i];
            }
            count = 0;
            _timeElapsed = 0;
        }
    }
}
