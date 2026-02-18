using UnityEngine;

public abstract class SynergyRuleSO : ScriptableObject
{
    public abstract SynergyRule CreateRuntimeRule();
}