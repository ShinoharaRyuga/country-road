using UnityEngine;

/// <summary>�^�C���̎q�I�u�W�F�N�g�̃|�C���g���Ǘ�����N���X </summary>
public class RoadPoint : MonoBehaviour
{
    [SerializeField, Tooltip("�|�C���g�̏��")] PointStatus _pointStatus = PointStatus.None;
    /// <summary>�e�^�C�� </summary>
    MapTile MapTile => transform.parent.GetComponent<MapTile>();
    public PointStatus PointStatus { get => _pointStatus; set => _pointStatus = value; }

    private void OnTriggerEnter(Collider other)
    {
        //�|�C���g�ɊX�̐l���Փ˂������̏���
        if (other.gameObject.CompareTag("Human"))
        {
            var human = other.gameObject.GetComponent<HumanMove>();

            if (MapTile != human.CurrentMapTile && human.CheckPoints(this))�@//���̃^�C���i��
            {
                if (_pointStatus == PointStatus.End)  //�i�������|�C���g���G���h�|�C���g�������珈������
                {
                    MapTile.Swap();
                }

                Debug.Log("���̃^�C���ɐi�����܂���");
                Debug.Log($"�Ă񂾐l{MapTile.gameObject.name}");
            }
        }

        //�^�C�����ړ�����đ��̃^�C���̌q���肪�o�������̏��� �q�������^�C������e�^�C���ŕێ�������
        if (other.gameObject.CompareTag("Point"))
        {
            if (_pointStatus == PointStatus.Start)  //�X�^�[�g�|�C���g�Ɍq���肪�o������
            {
                MapTile.StartConnectionTile = other.transform.parent.GetComponent<MapTile>();
            }
            else if (_pointStatus == PointStatus.End)  //�G���h�|�C���g�Ɍq���肪�o������
            {
                MapTile.EndConnectionTile = other.transform.parent.GetComponent<MapTile>();
            }
        }
    }

    /// <summary>
    /// �^�C�����ړ�����đ��̃^�C���Ƃ̌q���肪�Ȃ��Ȃ����ꍇ�̏��� 
    /// start��end�Ɍq�����Ă����^�C�������폜����
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Point"))
        {
            if (_pointStatus == PointStatus.Start)
            {
                MapTile.StartConnectionTile = null;
            }
            else if (_pointStatus == PointStatus.End)
            {
                MapTile.EndConnectionTile = null;
            }
        }
    }
}

public enum PointStatus
{
    Start = 0,
    Middle = 1,
    End = 2,
    None = 3,
}
