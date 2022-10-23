using UnityEngine;

/// <summary>ŠXl‚ªƒS[ƒ‹‚É“’…‚Ìˆ—‚ğ‚·‚éƒNƒ‰ƒX </summary>
public class ArrivalGoal : MonoBehaviour
{
    StageManager _manager = default;

    private void Start()
    {
       //TODO «~‚ß‚é
      //  _manager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out HumanMove human))
        {
            _manager.CurrentHumanNumber--;
            other.gameObject.SetActive(false);

            if (_manager.CurrentHumanNumber == 0)   //ƒQ[ƒ€ƒNƒŠƒA
            {
                _manager.StageClear();
            }
        }
    }
}
