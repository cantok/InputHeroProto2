using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill
{
    protected PlayerUnit Player => GameManager.Player;
    protected List<InputType> neededCombo;
    public List<InputType> NeededCombo => neededCombo;

    //��ų �����
    public virtual void Invoke()
    {

    }
}
