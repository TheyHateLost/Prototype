using UnityEngine;
using UnityEngine.SceneManagement;

public class SendToWinningScene : MonoBehaviour
{
    public string sceneName;
    private void OnTriggerEnter(Collider other)
    {
        //Death
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
