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
            Debug.Log("[�f�o�b�O] �^�C�g���ɖ߂�܂�");
            SceneManager.LoadScene(titleSceneName);
        }
    }
}
