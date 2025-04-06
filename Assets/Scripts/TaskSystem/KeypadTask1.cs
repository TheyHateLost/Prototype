using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeypadTask1 : MonoBehaviour
{
    //Code given to player
    public TextMeshProUGUI generatedCode;

    //Players input
    public TextMeshProUGUI inputCode;

    //Updates number of tasks done
    public GameEventsManager gameManager;

    //Task objects
    public GameObject keypadUI;
    public GameObject taskObject; 

    //Task Code
    public int codeLength;
    float codeResetTime;
    bool isResetting = false;

    public void ButtonClick(int number)
    {
        if (isResetting) { return; }

        //the generated code is set to 
        inputCode.text += number;

        if (inputCode.text == generatedCode.text)
        {
            inputCode.text = "Correct";
            keypadUI.SetActive(false);
            gameObject.SendMessage("DisableKeyPad");
            //this.gameObject.SetActive(false);
            //taskObject.SetActive(false);
            //gameManager.tasksRemaining -= 1;
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
}
