using UnityEngine;

/// <summary>�X�l���S�[���ɓ������̏���������N���X </summary>
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

            if (_manager.CurrentHumanNumber == 0)   //�Q�[���N���A
            {
                _manager.StageClear();
                Debug.Log("�X�e�[�W�N���A");
            }
        }
    }
}
