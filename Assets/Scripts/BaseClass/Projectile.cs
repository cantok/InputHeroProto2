using System.Collections;
using UnityEngine;

/// <summary>
/// ����ü�� �⺻ Ŭ����. ���/���Ÿ� ���. ���� ������ �� �浹 �� ó���� ���� Ŭ�������� �ۼ�
/// </summary>
public class Projectile : MonoBehaviour
{
    protected Vector2 originPos;
    protected Vector2 direction;//���� ����
    protected float speed;
    protected float lifeTime = -1f;
    protected float lifeDistance = -1f;
    protected bool isInitialized = false;
    protected bool isDestroyed = false;

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
            StartCoroutine(DestroyDelay());
        }

        PerformanceManager.StopTimer("Projectile.Initialize");
    }

    protected virtual void Update()
    {
        PerformanceManager.StartTimer("Projectile.Update");
        if (lifeDistance > 0 && (originPos - (Vector2)transform.position).sqrMagnitude >= lifeDistance * lifeDistance)
        {
            Destroy();
            PerformanceManager.StopTimer("Projectile.Update");
            return;
        }

        PerformanceManager.StopTimer("Projectile.Update");
    }

    /// <summary>
    /// Destroy ���
    /// </summary>
    protected virtual void Destroy()
    {
        var da = GetComponent<DamageArea>();
        if (da is not null)
        {
            isDestroyed = true;
            da.Destroy();
        }
        else
        {
            isDestroyed = true;
            Destroy(gameObject);
        }
    }


    protected IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy();
    }
}