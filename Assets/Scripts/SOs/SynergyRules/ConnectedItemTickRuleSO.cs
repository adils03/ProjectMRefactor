using UnityEngine;

[CreateAssetMenu(menuName = "SynergyRules/ConnectedItemTickRule")]
public class ConnectedItemTickRuleSO : SynergyRuleSO
{
    public override SynergyRule CreateRuntimeRule()
    {
        return new ConnectedItemTickSynergyRule();
    }
}
