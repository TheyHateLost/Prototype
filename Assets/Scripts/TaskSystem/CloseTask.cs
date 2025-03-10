using UnityEngine;

public class CloseTask : MonoBehaviour
{
    public GameObject taskUIToClose;
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            taskUIToClose.SetActive(false);
        }
    }
}
