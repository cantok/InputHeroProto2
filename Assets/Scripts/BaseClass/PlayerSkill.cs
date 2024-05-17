using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill
{
    protected PlayerUnit Player => GameManager.Player;
    protected List<InputType> neededCombo;
    public List<InputType> NeededCombo => neededCombo;

    private static bool isUsing = false;
    public static bool IsUsing => isUsing;

    //��ų �����
    public virtual void Invoke()
    {
        isUsing = true;
    }

    //���� �˸�
    public virtual void End()
    {
        isUsing = false;
    }
}
