using UnityEngine;

public class CloseTask : MonoBehaviour
{
    public GameObject taskUIToClose;

    //Should be assigned to empty object with trigger collider
    //Closes assigned task UI when leaving collider
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            taskUIToClose.SetActive(false);
        }
    }
}
