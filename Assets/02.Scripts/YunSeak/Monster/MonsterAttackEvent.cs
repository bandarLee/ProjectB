using System.Threading;
using UnityEngine;

public class MonsterAttackEvent : MonoBehaviour
{
    private MonsterNormalMove _owner;

    private void Start()
    {
        _owner = GetComponentInParent<MonsterNormalMove>();
    }

    //public void AttackEvent()
    //{
    //    _owner.PlayerAttack();
    //}
}