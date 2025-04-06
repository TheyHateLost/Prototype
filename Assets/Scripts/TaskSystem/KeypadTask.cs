using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeypadTask : MonoBehaviour
{
    //Code given to player
    [SerializeField] TextMeshProUGUI generatedCode;

    //Players input
    [SerializeField] TextMeshProUGUI inputCode;

    //Task objects
    [SerializeField] GameObject KeyPadUI;
    [SerializeField] GameObject KeyPadTaskDetection;
    [SerializeField] GameObject Correct_Text;
    [SerializeField] GameObject Failed_Text;

    //Task Code
    int codeLength;
    [SerializeField]float codeResetTime = 2f;
    public static bool isResetting = false;

    void OnEnable()
    {
        codeLength = Random.Range(5,9);
        string code = string.Empty;

        for(int i = 0; i < codeLength; i++)
        {
            code += Random.Range(1,10);
        }

        generatedCode.text = code;
        inputCode.text = string.Empty; 
    }
    public void NumPadButtonClick(int number)
    {
        //if (isResetting) { return; }

        //the generated code is set to 
        inputCode.text += number;
    }

    public void SubmitCode()
    {
        //Code is Correct
        if (inputCode.text == generatedCode.text)
        {
            Correct_Text.SetActive(true);
            KeyPadUI.SetActive(false);
            KeyPadTaskDetection.SetActive(false);
            GameEventsManager.tasksRemaining -= 1;
            //StartCoroutine(ResetCode());
        }
        else
        {
            Failed_Text.SetActive(true);
            KeyPadUI.SetActive(false);

            //Coroutine uses monobahaviour from KeyPadTaskDetection to activate
            MonoBehaviour taskScript = KeyPadTaskDetection.GetComponent<MonoBehaviour>();
            taskScript.StartCoroutine(ResetCode());
        }
    }
    //When keypad is not up, code gets reset
    private IEnumerator ResetCode()
    {
        isResetting = true;

        yield return new WaitForSeconds(codeResetTime);

        inputCode.text = string.Empty;
        isResetting = false;
        Failed_Text.SetActive(false);
    }
}
