using System.Threading;
using UnityEngine;

public class MonsterAttackEvent : MonoBehaviour
{
    private MonsterMove _owner;

    private void Start()
    {
        _owner = GetComponentInParent<MonsterMove>();
    }

    //public void AttackEvent()
    //{
    //    _owner.PlayerAttack();
    //}수정
}