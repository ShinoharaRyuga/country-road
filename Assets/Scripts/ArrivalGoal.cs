using UnityEngine;

/// <summary>�X�l���S�[���ɓ������̏���������N���X </summary>
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
