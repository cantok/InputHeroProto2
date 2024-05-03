using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ����+�ΰ�ȿ���� ��Ÿ���� Damage�� ������ ���� ����(���� ������Ʈ)
/// ��ü�����δ� �浹 ���� ��Ʈ�ڽ��� ����, ���� Ʈ���Ŵ� Attack���� ��
/// </summary>
public class DamageArea : CollisionChecker
{
    /// <summary>
    /// ������
    /// </summary>
    private Attack source;
    /// <summary>
    /// ������
    /// </summary>
    public Attack Source
    {
        get { return source; }
        set { source = value; }
    }
    /// <summary>
    /// �����. ���߿� ���� ���� �� �����̻� ���� �� ������ ��
    /// </summary>
    public float damage;

    /// <summary>
    /// �켱��. ���� ���� �켱��.
    /// </summary>
    [SerializeField]
    private int priority = 0;
    /// <summary>
    /// �켱��. ���� ���� �켱��.
    /// </summary>
    public int Priority
    {
        get
        { return priority; }
    }

    /// <summary>
    /// 1 �̻��̶�� �ش� Ƚ�� �浹 ���� �Ҹ�
    /// </summary>
    public int destroyHitCounter = -1;

    //private List<HitBox> hitBoxList = new();//�浹 ���� ��Ʈ�ڽ� ����Ʈ
    public List<HitBox> HitBoxList
    {
        get
        {
            PerformanceManager.StartTimer("DamageArea.HitBoxList.get");
            List<HitBox> temp = new();
            foreach (var collider in EnteredColliders)
            {
                HitBox hitBox = collider.GetComponent<HitBox>();
                if (hitBox is not null)
                {
                    temp.Add(hitBox);
                }
            }
            PerformanceManager.StopTimer("DamageArea.HitBoxList.get");
            return temp;
        }
    }

    /// <summary>
    /// �ش� DamageArea�� ���� ���ظ� �ֵ��� ����
    /// </summary>
    /// <param name="target">���ݹ޴� ���</param>
    public void DealDamage(Unit target)
    {
        UnitManager.Instance.DamageUnitToUnit(target, source.AttackUnit, this);

        if (destroyHitCounter != -1)
        {
            destroyHitCounter--;
            if (destroyHitCounter == 0)
            {
                Destroy();
            }
        }
    }

    public void DealDamage(HitBox hitBox)
    {
        UnitManager.Instance.DamageUnitToHitbox(hitBox, source.AttackUnit, this);

        if (destroyHitCounter != -1)
        {
            destroyHitCounter--;
            if (destroyHitCounter == 0)
            {
                Destroy();
            }
        }
    }


    /// <summary>
    /// ��� ���� �� ����. ����Ƽ�� Destroy���� �̰� �� ��
    /// </summary>
    public void Destroy()
    {
        source.WithdrawDamage(this);
        Destroy(gameObject);
    }
}
