using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckOut : MonoBehaviour
{
    int codeLength;
    public Text PlayerCode;
    void Start()
    {
        codeLength = Random.Range(4, 6);
        string code = string.Empty;

        for (int i = 0; i < codeLength; i++)
        {
            code += Random.Range(1, 10);
        }

        PlayerCode.text = code;
    }

}