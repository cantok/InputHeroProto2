using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_EnemyFast : Shoot
{
    protected override bool HitCheck(Transform target)
    {
        if (target != null)
        {
            // �浹 ó��
            Debug.Log("Hit");

            //�÷��̾� ��Ʈ�ڽ���
            if (target.GetComponent<HitBox>() != null && target.CompareTag("Player"))
            {
                Hit(target.GetComponent<HitBox>().Unit);
                return true;
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
