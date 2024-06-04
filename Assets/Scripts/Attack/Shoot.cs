using UnityEngine;

/// <summary>
/// �� ����, ���� �ӵ�
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

    [SerializeField]
    private bool isNotSlowed = false;


    public override void Initialize(Vector2 dir, float speed, float lifeTime = -1f, float lifeDistance = -1f)
    {
        base.Initialize(dir, speed, lifeTime, lifeDistance);

        previousPosition = currentPosition;
        PerformanceManager.StopTimer("Bullet.Initialize");
    }

    private void FixedUpdate()
    {
        //�� �����ӿ� �̵��� �Ÿ�
        Vector2 moveDist = direction * speed * (isNotSlowed ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime);

        var ray = Physics2D.RaycastAll(previousPosition, moveDist.normalized, moveDist.magnitude);
        if (1 <= ray.Length)
        {
            foreach (var hit in ray)
            {
                if (hit.collider == null)
                {
                    continue;
                }

                var hitTarget = hit.transform.GetComponent<IBulletHitChecker>();
                if (hitTarget == null)
                {
                    continue;
                }


            }
            // �浹 ó��
            Debug.Log("Hit: ");
            // �ʿ��� ���, �浹 ������ ���� ó�� �߰�
        }

        transform.Translate(moveDist);
    }
}
