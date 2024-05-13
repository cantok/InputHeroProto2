using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ü�� �⺻ Ŭ����. ���/���Ÿ� ���
/// </summary>
public class Projectile : MonoBehaviour
{
    protected Vector2 originPos;
    protected Vector2 direction;//���� ����
    protected float speed;
    protected float lifeTime = -1f;
    protected float lifeDistance = -1f;
    protected bool isInitialized = false;

    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    /// <param name="dir">�̵� ����(or ������)</param>
    /// <param name="speed">�̵� �ӵ�</param>
    /// <param name="lifeTime">����(��)</param>
    /// <param name="lifeDistance">����(�Ÿ�)</param>
    public virtual void Initialize(Vector2 dir, float speed, float lifeTime = -1f, float lifeDistance = -1f)
    {
        PerformanceManager.StartTimer("Projectile.Initialize");

        originPos = transform.position;
        direction = dir;
        this.speed = speed;
        this.lifeTime = lifeTime;
        this.lifeDistance = lifeDistance;
        isInitialized = true;

        if (lifeTime > 0)
        {
            TryDestroy();
        }

        PerformanceManager.StopTimer("Projectile.Initialize");
    }

    protected virtual void Update()
    {
        PerformanceManager.StartTimer("Projectile.Update");
        if (lifeDistance > 0 && (originPos - (Vector2)transform.position).sqrMagnitude >= lifeDistance * lifeDistance)
        {
            TryDestroy();
            PerformanceManager.StopTimer("Projectile.Update");
            return;
        }

        PerformanceManager.StopTimer("Projectile.Update");
    }

    /// <summary>
    /// Destroy �õ�. DamageArea�� ������ DamageArea�� ���� ��� ���
    /// </summary>
    protected void TryDestroy()
    {
        if (lifeTime > 0)
        {
            StartCoroutine(DestroyDelay());
        }
        else
        {
            var da = GetComponent<DamageArea>();
            if (da is not null)
            {
                da.Destroy();
            }
            else
            {
                Destroy(gameObject);
            }
        }

    }

    /// <summary>
    /// Destroy ���
    /// </summary>
    protected void Destroy()
    {
        var da = GetComponent<DamageArea>();
        if (da is not null)
        {
            da.Destroy();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    protected IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy();
    }
}