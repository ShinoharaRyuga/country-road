using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>�v���C���̃X�e�[�W��������x�V�� </summary>
public class Replay : MonoBehaviour
{
   public void GameRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
