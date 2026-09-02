using UnityEngine;

public static class HexGameRuleChecks
{
    public static void Run()
    {
        HexGameRuleTests.RunAll();
        Debug.Log("Hex game rule checks passed.");
    }
}
