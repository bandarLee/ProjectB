using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Health,  // 체력을 줄인다
    Stamina, // 스피드를 줄인다.
    Bullet   // 현재 들고있는 총의 총알이 꽉찬다.
}

public class MonsterBullet : MonoBehaviour
{
    public BulletType BulletType;
    public int Count;

    public Bullet(BulletType BulletType, int count)
    {
        BulletType = BulletType;
        Count = count;
    }


    public bool TryUse()
    {
        if (Count == 0)
        {
            return false;
        }

        Count -= 1;

        switch (BulletType)
        {
            case BulletType.Health:
            {
                // Todo: 플레이어 체력 꽉차기
                PlayerMoveAbility playerMoveAbility = GameObject.FindWithTag("Player").GetComponent<PlayerMoveAbility>();
                playerMoveAbility.Health = playerMoveAbility.MaxHealth;
                break;
            }

            case BulletType.Stamina:
            {
                // Todo: 플레이어 스태미너 꽉차기
                PlayerMoveAbility playerMoveAbility = GameObject.FindWithTag("Player").GetComponent<PlayerMoveAbility>();
                playerMoveAbility.Stamina = PlayerMoveAbility.MaxStamina;
                break;
            }

            case BulletType.Bullet:
            {
                // Todo: 플레이어가 현재 들고있는 총의 총알이 꽉찬다.
                PlayerGunFireAbility ability = GameObject.FindWithTag("Player").GetComponent<PlayerGunFireAbility>();
                ability.CurrentGun.BulletRemainCount = ability.CurrentGun.BulletMaxCount;
                ability.RefreshUI();
                break;
            }
        }

        return true;
    }
}
