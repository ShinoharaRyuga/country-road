using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>プレイ中のステージをもう一度遊ぶ </summary>
public class Replay : MonoBehaviour
{
   public void GameRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
