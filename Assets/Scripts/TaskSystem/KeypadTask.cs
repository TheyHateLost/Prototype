using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeypadTask : MonoBehaviour
{
    //Code given to player
    //[SerializeField] TextMeshProUGUI generatedCode;

    //Players input
    [SerializeField] TextMeshProUGUI inputCode;

    //Task objects
    [SerializeField] GameObject KeyPadUI;
    [SerializeField] GameObject KeyPadTaskDetection;
    [SerializeField] GameObject Correct_Text;
    [SerializeField] GameObject Failed_Text;
    [SerializeField] GameObject Player;
    [SerializeField] CheckOut PlayerCodeScript;

    //Task Code
    int codeLength;
    [SerializeField]float codeResetTime = 2f;
    public static bool isResetting = false;

    void OnEnable()
    {
        inputCode.text = string.Empty; 
    }
    public void NumPadButtonClick(int number)
    {
        //the generated code is set to 
        inputCode.text += number;
    }

    public void SubmitCode()
    {
        //Code is Correct
        if (inputCode.text == PlayerCodeScript.PlayerCode.text)
        {
            Correct_Text.SetActive(true);
            KeyPadUI.SetActive(false);
            KeyPadTaskDetection.SetActive(false);
        }
        //Secret code
        else if (inputCode.text == "072104")
        {
            Correct_Text.SetActive(true);
            KeyPadUI.SetActive(false);



            MonoBehaviour taskScript = KeyPadTaskDetection.GetComponent<MonoBehaviour>();
            taskScript.StartCoroutine(ResetKeyPad());
        }
        else if (inputCode.text == "03141319030804")
        {
            Player.tag = "Invisible";
        }
        else
        {
            Failed_Text.SetActive(true);
            KeyPadUI.SetActive(false);

            //Coroutine uses monobahaviour from KeyPadTaskDetection to activate
            MonoBehaviour taskScript = KeyPadTaskDetection.GetComponent<MonoBehaviour>();
            taskScript.StartCoroutine(ResetWrongCode());
        }
    }
    //When keypad is not up, code gets reset
    IEnumerator ResetWrongCode()
    {
        isResetting = true;

        yield return new WaitForSeconds(codeResetTime);

        inputCode.text = string.Empty;
        isResetting = false;
        Failed_Text.SetActive(false);
    }
    IEnumerator ResetKeyPad()
    {
        isResetting = true;

        yield return new WaitForSeconds(codeResetTime);

        inputCode.text = string.Empty;
        isResetting = false;
        Correct_Text.SetActive(false);
    }
}
