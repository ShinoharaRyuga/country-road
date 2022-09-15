using UnityEngine;

/// <summary>街人がゴールに到着時の処理をするクラス </summary>
public class ArrivalGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out HumanMove human))
        {
            human.MoveStop();
        }
    }
}
