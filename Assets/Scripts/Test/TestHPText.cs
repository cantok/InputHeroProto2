using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestHPText : MonoBehaviour
{
    public Unit Player;
    public Unit Enemy;
    public TextMeshProUGUI textMeshPro;

    // Update is called once per frame
    void Update()
    {
        string text = $"PlayerHp: {Player.Stats.health}\n";
        text += $"EnemyHp: {Enemy.Stats.health}\n";

        textMeshPro.text = text;
    }
}
