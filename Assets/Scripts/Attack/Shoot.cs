using UnityEngine;

/// <summary>
/// ���� �ӵ��� ����ü. ���� Ŭ�������� ����ؼ� ���
/// </summary>
public class Shoot : Projectile
{
    /// <summary>
    /// �����ϴ� ����
    /// </summary>
    protected Unit attackUnit;
    /// <summary>
    /// �����ϴ� ����
    /// </summary>
    public Unit AttackUnit
    {
        get
        {
            return attackUnit;
        }
    }

    private Vector2 previousPosition;
    private Vector2 currentPosition => transform.position;

    private bool isDestroying = false;
    private TrailRenderer trail;


    /// <summary>
    /// �����. ���߿� ���� ���� �� �����̻� ���� �� ������ ��
    /// </summary>
    public float damage;

    [SerializeField]
    private bool isNotSlowed = false;



    //�ʱ�ȭ
    public override void Initialize(Vector2 dir, float speed, float lifeTime = -1f, float lifeDistance = -1f)
    {
        base.Initialize(dir, speed, lifeTime, lifeDistance);

        previousPosition = currentPosition;
        trail = GetComponentInChildren<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        //�� �����ӿ� �̵��� �Ÿ�
        Vector2 moveDist = direction * speed * (isNotSlowed ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime);

        //���� �߻�� ���� ����� ������ ��� ã��
        var ray = Physics2D.LinecastAll(previousPosition, previousPosition + moveDist);
        if (1 <= ray.Length)
        {
            //�浹�� ������ ���
            Transform target = null;
            Vector2 hitPos = Vector2.zero;

            foreach (var hit in ray)
            {
                if (hit.transform == target || hit.transform.GetComponent<IBulletHitChecker>() == null)
                {
                    continue;
                }

                //���� ����� Ÿ�� ã��
                if (target == null ||
                    ((previousPosition - (Vector2)target.position).sqrMagnitude < (previousPosition - (Vector2)hit.transform.position).sqrMagnitude))
                {
                    target = hit.transform;
                    hitPos = hit.point;
                }

            }

            //���θ����°�?
            if (HitCheck(target))
            {
                transform.Translate(hitPos);
                Destroy();
                return;
            }

        }

        transform.Translate(moveDist);
    }

    protected virtual bool HitCheck(Transform target)
    {
        if (target != null)
        {
            // �浹 ó��
            Debug.Log("Hit");


            // �浹�� ���� ó�� �߰�
        }
        return false;
    }

    protected virtual void Hit(Unit targetUnit)
    {
        UnitManager.Instance.DamageUnitToUnit(targetUnit, AttackUnit, damage);
        return;
    }

    protected override void Destroy()
    {
        if (trail is not null)
        {
            trail.transform.SetParent(null);
            Destroy(trail.gameObject, trail.time);
        }
        base.Destroy();
    }
}
