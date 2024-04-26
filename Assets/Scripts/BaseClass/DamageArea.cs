using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����+�ΰ�ȿ���� ��Ÿ���� Damage�� ������ ���� ����(���� ������Ʈ)
/// ��ü�����δ� �浹 ���� ��Ʈ�ڽ��� ����, ���� Ʈ���Ŵ� Attack���� ��
/// </summary>
public class DamageArea : MonoBehaviour
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
    
    private List<HitBox> hitBoxList = new();//�浹 ���� ��Ʈ�ڽ� ����Ʈ
    public List<HitBox> HitBoxList
    { get { return hitBoxList; } }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�浹 ���� ��Ʈ�ڽ� ���
        HitBox hitBox = collision.GetComponent<HitBox>();
        if (hitBox != null)
        {
            hitBoxList.Add(hitBox);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�浹 ���� ��Ʈ�ڽ� ��� ����
        HitBox hitBox = collision.GetComponent<HitBox>();
        if (hitBox != null && hitBoxList.Contains(hitBox))
        {
            hitBoxList.Remove(hitBox);
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
