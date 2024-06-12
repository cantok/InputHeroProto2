using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_EnemyFast : Shoot
{
    protected override bool HitCheck(Collider2D target)
    {
        if (target != null)
        {
            // 충돌 처리

            //플레이어 히트박스면
            HitBox hitBox = target.GetComponent<HitBox>();
            if (hitBox != null && target.CompareTag("Player"))
            {
                Debug.Log("Hit");
                hitBox.Damage(damage);
                //Hit(hitBox.Unit);
                return true;
            }

            //기타 장애물이면
            if (true)
            {

            }

            // 충돌에 대한 처리 추가
        }
        return false;
    }
}
