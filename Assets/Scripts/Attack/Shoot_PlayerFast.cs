using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_PlayerFast : Shoot
{
    protected override bool HitCheck(Collider2D target)
    {
        if (target != null)
        {
            // �浹 ó��

            //�÷��̾� ��Ʈ�ڽ���
            HitBox hitBox = target.GetComponent<HitBox>();
            if (hitBox != null && target.CompareTag("Enemy"))
            {
                hitBox.Damage(damage);
                //Hit(hitBox.Unit);
                return true;
            }

            //�Ѿ��̸�
            //if (Time.timeScale < 1)
            {
                var temp = hitBox.GetComponent<TestBulletChecker>();
                if (temp != null)
                {
                    Debug.Log("Hit");

                    hitBox.GetComponentInChildren<DamageArea>()?.Destroy();
                    return true;
                }
            }

            //��Ÿ ��ֹ��̸�
            if (true)
            {

            }

            // �浹�� ���� ó�� �߰�
        }
        return false;
    }
}
