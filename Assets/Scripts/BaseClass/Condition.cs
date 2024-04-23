using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����̻�(���� ȿ��)�� �������� ����ϴ� ���� Ŭ����
/// �����̻󺰷� ���� Ŭ������ �ξ ���
/// ���ֿ��� ����Ʈ�� ����
/// </summary>
public class Condition
{
    /// <summary>
    /// ���ӽð�(��)
    /// </summary>
    private float duration;
    /// <summary>
    /// ���ӽð�(��)
    /// </summary>
    public float Duration
    {
        get
        {
            return duration;
        }
        set
        {
            duration = value;
        }
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    private Unit owner;
    /// <summary>
    /// ���� ����
    /// </summary>
    public Unit Owner
    {
        get { return owner; }
        set { owner = value; }
    }

    /// <summary>
    /// �ο��� �� ȿ�� Ʈ����
    /// </summary>
    public void OnAddTriger()
    {
        OnAdd();
    }

    /// <summary>
    /// �ο��� �� ȿ��
    /// </summary>
    public virtual void OnAdd()
    {

    }

    /// <summary>
    /// ������ �� ȿ�� Ʈ����
    /// </summary>
    public void OnUpdateTriger()
    {
        OnUpdate();
    }

    /// <summary>
    /// ������ �� ȿ��
    /// </summary>
    public virtual void OnUpdate()
    {

    }

    /// <summary>
    /// ���ŵ� �� ȿ�� Ʈ����
    /// </summary>
    public virtual void OnRemoveTriger()
    {
        OnRemove();
    }

    /// <summary>
    /// ���ŵ� �� ȿ��
    /// </summary>
    public virtual void OnRemove() 
    { 

    }
}
