using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;  // 공격 속도
    public int maxAmmo;     // 보유한 총알 최대 개수
    public int currentAmmo; // 현재 보유한 총알

    public BoxCollider meleeArea; // 공격 범위 콜라이더
    public TrailRenderer trailEffect; // 휘두르기 이펙트

    public Transform bulletPosition;    // 총알이 발사될 위치
    public GameObject bullet;       // 생성된 총알 프리팹 객체
    public Transform bulletCasePosition; // 탄피 위치
    public GameObject bulletCase; // 탄피


    private void Awake()
    {
        if (meleeArea)
            meleeArea.enabled = false;
    }

    public void Use()
    {

        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if (type == Type.Range && currentAmmo > 0)
        {
            currentAmmo--;
            StartCoroutine("Shot");
        }

    }

    // 코루틴
    IEnumerator Swing()
    {
        // 1
        yield return new WaitForSeconds(0.1f); // 0.1초 대기
        if (meleeArea && trailEffect)
        {
            meleeArea.enabled = true;
            trailEffect.enabled = true;
        }
        // 2
        yield return new WaitForSeconds(1.0f); // 0.2초 대기
        if (meleeArea)
        {
            meleeArea.enabled = false;
        }

        // 3
        yield return new WaitForSeconds(.4f); // 0.3초 대기
        if (trailEffect)
        {
            trailEffect.enabled = false;
        }
        /*
         // 1
         yield return null; // 1프래임 대기
         // 2
         yield return null; // 1프래임 대기
         // 3 
         yield return null; // 1프래임 대기

         yield break; // 코루틴 종료
        */
    }
    IEnumerator Shot()
    {
        // 1 총알 발사
        GameObject instantBullet = Instantiate(bullet, bulletPosition.position, bulletPosition.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPosition.forward * 50;

        yield return null;

        // 2 탄피 배출
        GameObject instantCase = Instantiate(bulletCase, bulletCasePosition.position, bulletCasePosition.rotation);
        Rigidbody bulletCaseRigid = instantCase.GetComponent<Rigidbody>();
        //bulletCaseRigid.velocity = bulletCasePosition.forward * 50;

        Vector3 caseVec = bulletCasePosition.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        bulletCaseRigid.AddForce(caseVec, ForceMode.Impulse);
        bulletCaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);


    }
}
