using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    //�̱���
    private static TimeManager instance;
    public static TimeManager Instance => instance;

    //���� ������
    private const int framePerSec = 50;

    //�ð� ���� �� �̺�Ʈ
    private static event EventHandler<float> onTimeScaleChanged;
    public static event EventHandler<float> OnTimeScaleChanged
    {
        add { onTimeScaleChanged += value; }
        remove { onTimeScaleChanged -= value; }
    }

    //���ο�
    [SerializeField] private float slowRate = .5f;//���ο� ����
    public static float SlowRate => instance.slowRate;//���ο� ����

    [SerializeField]
    private float slowTime = 4f;//���ο� ���� �ð�

    private bool isSlowed = false;//���ο� ���ΰ�?
    public static bool IsSlowed => instance.isSlowed;
    private bool isUsingSkills = false;//��ų ��� ���ΰ�?
    public static bool IsUsingSkills
    {
        get
        {
            return instance.isUsingSkills;
        }
        set
        {
            instance.isUsingSkills = value;
        }
    }


    private TickTimer slowTimer;//���ο� ���� �ð� Ÿ�̸�
    [SerializeField] private Slider slowSlider;//���ο� ���� �ð� UI

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //�̱���
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
            instance = this;
    }

    private void Start()
    {
        Time.fixedDeltaTime = Time.timeScale / framePerSec;
        slowTimer = new(unscaledTime: true);
    }

    private void Update()
    {
        //���ο� ���߿�
        if (isSlowed && !isUsingSkills)
        {
            float remainTime = slowTimer.GetRemain(slowTime);
            slowSlider.value = remainTime / instance.slowTime;//������ ����
            //���ο� �ð��� �� ��ٸ�
            if (remainTime <= 0)
            {
                StartCoroutine(StartSkillQueue());//��ų ���� ����
            }
        }
    }

    #region �Ҹ�Ÿ��

    /// <summary>
    /// �Ҹ�Ÿ�� ����
    /// </summary>
    public static void StartSlow()
    {
        instance.isSlowed = true;
        SetTimeScale(instance.slowRate);
        instance.slowTimer.Reset();
    }

    //��ų ���
    private IEnumerator StartSkillQueue()
    {
        isUsingSkills = true;
        ComboManager.FindCombos(GameManager.Player.SkillList);

        //��ų �����
        while (true)
        {
            PlayerSkill skill = ComboManager.GetFindedSkill();//ť���� ��ų ã�ƿ���
            if (skill == null)//���� ��ų�� ���ٸ� ��
            {
                break;
            }
            else
            {
                skill.Invoke();//��ų ����
                yield return new WaitUntil(() => !PlayerSkill.IsUsing);//������ ��ų�� ���� ������ ���
            }
        }


        EndSlow();
        isUsingSkills = false;
    }

    /// <summary>
    /// �Ҹ�Ÿ�� ����(��ų ������)
    /// </summary>
    public static void EndSlow()
    {
        SetTimeScale(1);
        ComboManager.Reset();
        instance.isSlowed = false;
    }

    #endregion

    //�ð� ���� ����
    public static void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = Time.timeScale / framePerSec;
        onTimeScaleChanged?.Invoke(instance, Time.timeScale);
    }

    //�ð� �Ҹ�
    public static void DecreaseComboTime(float time)
    {
        instance.slowTimer.AddOffset(time);
    }
}
