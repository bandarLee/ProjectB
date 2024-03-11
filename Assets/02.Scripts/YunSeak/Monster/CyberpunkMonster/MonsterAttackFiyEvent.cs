using System.Threading;
using UnityEngine;

public class MonsterAttackFlyEvent : MonoBehaviour
{
    private MonsterFiy _owner;

    private void Start()
    {
        _owner = GetComponentInParent<MonsterFiy>();
    }

    public void AttackEvent()
    {
        _owner.PlayerAttack();
    }
}