using System.Threading;
using UnityEngine;

public class MonsterAttackEvent : MonoBehaviour
{
    private MonsterNormal _owner;

    private void Start()
    {
        _owner = GetComponentInParent<MonsterNormal>();
    }

    //public void AttackEvent()
    //{
    //    _owner.PlayerAttack();
    //}
}