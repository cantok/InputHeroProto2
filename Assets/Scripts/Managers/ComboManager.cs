using System.Collections.Generic;
using UnityEngine;

public class ComboManager
{
    //�̱���
    private static ComboManager instance;
    public static ComboManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ComboManager();
            }
            return instance;
        }
    }

    [SerializeField]
    private List<InputType> comboInputs = new();
    private List<InputType> log = new();
    private Queue<PlayerSkill> findedSkills = new();

    [SerializeField, Range(1, 12)]
    private int maxCombo;

    /// <summary>
    /// ��ȿ�� Ű �Է�
    /// </summary>
    /// <param name="input">�Է��� Ű</param>
    /// <returns>�ִ�ġ���� á�°�?</returns>
    public bool InputLog(InputType input)
    {
        if (log.Count >= maxCombo)
        {
            return true;
        }

        if (comboInputs.Contains(input))
        {
            log.Add(input);
        }

        return log.Count >= maxCombo;
    }

    public void FindCombos(List<PlayerSkill> skillList)
    {
        //�α� ��ü�� ���� �޺��� ã�´�
        for (int i = 0; i < log.Count; i++)
        {
            for (int j = 0; j < i + 1; j++)
            {
                //index�� i�� item(i+1��° item)�� ���������� �Ͽ� j����ŭ ��

                //�迭 ����
                InputType[] temp = new InputType[j + 1];
                log.CopyTo(i - j, temp, 0, j + 1);

                //��ġ�ϴ� ��ų�� �ִ��� Ž��
                foreach (PlayerSkill skill in findedSkills)
                {
                    if (GameTools.CompareEnumList(skill.NeededCombo, temp))
                    {
                        findedSkills.Enqueue(skill);
                    }
                }
            }

        }
        log.Clear();
    }

    public void Reset()
    {
        comboInputs.Clear();
        log.Clear();
        findedSkills.Clear();
    }
}
