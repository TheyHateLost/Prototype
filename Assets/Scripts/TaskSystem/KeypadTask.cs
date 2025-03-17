using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeypadTask : MonoBehaviour
{
    //Code given to player
    public TextMeshProUGUI cardCode;

    //Players input
    public TextMeshProUGUI inputCode;

    //Updates number of tasks done
    public GameEventsManager gameManager;

    //Task objects
    public GameObject keypad;
    public GameObject taskObject; 

    //Task Code
    public int codeLength;
    float codeResetTime;
    bool isResetting = false;

    void OnEnable()
    {
        codeLength = Random.Range(5,9);
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

        //the generated code is set to 
        inputCode.text += number;

        if (inputCode.text == cardCode.text)
        {
            inputCode.text = "Correct";
            keypad.SetActive(false);
            taskObject.SetActive(false);
            gameManager.tasksRemaining -= 1;
            StartCoroutine(ResetCode());
        }
        else if (inputCode.text.Length >= codeLength)
        {
            inputCode.text = "Failed";
            StartCoroutine(ResetCode()); 
        }
    }
    //When keypad is not up, code gets reset
    private IEnumerator ResetCode()
    {
        isResetting = true;

        yield return new WaitForSeconds(codeResetTime);

        inputCode.text = string.Empty;
        isResetting = false;
    }
    private IEnumerator WinCode()
    {
        isResetting = true;

        yield return new WaitForSeconds(codeResetTime);

        inputCode.text = string.Empty;
        isResetting = false;
    }
}
