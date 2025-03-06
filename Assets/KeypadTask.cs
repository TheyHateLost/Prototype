using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Editor;
using UnityEngine.UI;

public class KeypadTask : MonoBehaviour
{
    public TextMeshProUGUI cardCode;
    public TextMeshProUGUI inputCode;
    public int codeLength = 5;
    public float codeResetTime = 0.5f;
    bool isResetting = false;

    void OnEnable()
    {
        string code = string.Empty;

        for(int i = 0; i < codeLength; i++)
        {
            code += Random.Range(1,10);
        }

        cardCode.text = code;
        inputCode.text = string.Empty; 
    }
    public void ButtonClick(int number)
    {
        if (isResetting) { return; }

        inputCode.text += number;

        if (inputCode.text == cardCode.text)
        {
            inputCode.text = "Correct";
            StartCoroutine(ResetCode());
        }
        else if (inputCode.text.Length >= codeLength)
        {
            inputCode.text = "Failed";
            StartCoroutine(ResetCode()); 
        }
    }
    private IEnumerator ResetCode()
    {
        isResetting = true;

        yield return new WaitForSeconds(codeResetTime);

        inputCode.text = string.Empty;
        isResetting = false;
    }
}
