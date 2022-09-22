using UnityEngine;

/// <summary>街人がゴールに到着時の処理をするクラス </summary>
public class ArrivalGoal : MonoBehaviour
{
    StageManager _manager = default;

    private void Start()
    {
        _manager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out HumanMove human))
        {
            _manager.CurrentHumanNumber--;
            other.gameObject.SetActive(false);

            if (_manager.CurrentHumanNumber == 0)   //ゲームクリア
            {
                _manager.StageClear();
                Debug.Log("ステージクリア");
            }
        }
    }
}
