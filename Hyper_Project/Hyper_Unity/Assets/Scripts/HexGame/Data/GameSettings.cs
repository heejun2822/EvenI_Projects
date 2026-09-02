using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hex Game/Game Settings", fileName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private int initialScore = 0;
    [SerializeField] private int initialHealth = 100;
    [SerializeField, Min(.1f)] private float moveTimeLimit = 7f;
    [SerializeField, Min(0f)] private float overtimeHealthLossPerSecond = 2f;
    [SerializeField] private List<StageData> stages = new();

    public int InitialScore => initialScore;
    public int InitialHealth => initialHealth;
    public float MoveTimeLimit => moveTimeLimit;
    public float OvertimeHealthLossPerSecond => overtimeHealthLossPerSecond;
    public IReadOnlyList<StageData> Stages => stages;
}
