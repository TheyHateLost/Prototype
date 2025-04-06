using TMPro;
using UnityEngine;
public class TaskSystem1 : MonoBehaviour, IInteractable
{
    //Attach to Physcial Task Objects, make interact scrip aware of thse objects
    //Brings up the assigned UI
    public void Interact()
    {
        if (this.gameObject.GetComponent<TaskSystem1>().enabled == true)
        keypadUI.SetActive(true);
    }

    //Genrated code given to player
    public TextMeshProUGUI generatedCode;

    //Players input
    public TextMeshProUGUI inputCode;

    public GameObject keypadUI;
    [SerializeField] Component taskScript;

    int codeLength;
    float codeResetTime;
    bool isResetting = false;

    void Start()
    {
        codeLength = Random.Range(5, 8);
        string code = string.Empty;

        for (int i = 0; i < codeLength; i++)
        {
            code += Random.Range(1, 10);
        }

        generatedCode.text = code;
        inputCode.text = string.Empty;
    }


    public void DisableKeyPad(bool taskCompleted)
    {
        if (taskCompleted == true)
        {
            taskCompleted = false;

            this.gameObject.GetComponent<TaskSystem1>().enabled = false;
            
        }
    }
}
