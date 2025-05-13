using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeypadTask : MonoBehaviour
{
    //Players input
    [SerializeField] TextMeshProUGUI inputCode;

    //Task objects
    [SerializeField] GameObject KeyPad_Object;
    [SerializeField] GameObject Correct_Text;
    [SerializeField] GameObject Failed_Text;
    [SerializeField] GameObject Player;
    [SerializeField] CheckOut ComputerInputScript;
    [SerializeField] GameObject WinningArea;

    //Task Code
    int codeLength;
    [SerializeField]float codeResetTime = 4f;
    public static bool isResetting = false;


    [SerializeField] GameObject Secret_Help_UI;

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
        if (inputCode.text == ComputerInputScript.PlayerCode.text)
        {
            Correct_Text.SetActive(true);
            WinningArea.SetActive(true);
            gameObject.gameObject.SetActive(false);
        }
        //Spell "Help" to bring up old UI
        else if (inputCode.text == "08051216")
        {
            Secret_Help_UI.SetActive(true);

            Correct_Text.SetActive(true);
            gameObject.SetActive(false);

            MonoBehaviour taskScript = KeyPad_Object.GetComponent<MonoBehaviour>();
            taskScript.StartCoroutine(ResetKeyPad());
        }
        //Spell "Dont Die" to become invisible
        else if (inputCode.text == "04151420040905")
        {
            Player.tag = "Invisible";
        }
        else
        {
            Failed_Text.SetActive(true);
            gameObject.SetActive(false);

            //Coroutine uses monobahaviour from KeyPadTaskDetection to activate
            MonoBehaviour taskScript = KeyPad_Object.GetComponent<MonoBehaviour>();
            taskScript.StartCoroutine(ResetWrongCode());
        }
    }
    //When keypad is not up, code gets reset
    IEnumerator ResetWrongCode()
    {
        isResetting = true;

        yield return new WaitForSeconds(codeResetTime);

        inputCode.text = string.Empty;
        Failed_Text.SetActive(false);
        isResetting = false;
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
