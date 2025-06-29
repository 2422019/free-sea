using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToTitle : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "TitleScene";
    [SerializeField] private KeyCode returnKey = KeyCode.R;

    void Update()
    {
        if (Input.GetKeyDown(returnKey))
        {
            Debug.Log("[デバッグ] タイトルに戻ります");
            SceneManager.LoadScene(titleSceneName);
        }
    }
}
