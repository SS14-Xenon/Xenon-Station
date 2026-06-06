using Content.Shared.Chemistry.Components;
using Content.Shared.EntityEffects;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Shared._Ganimed.Chemistry.Effects;

public sealed partial class AdjustSolutionPHReactionEffect : EntityEffect
{
    [DataField("targetPH")]
    public float TargetPH = ChemistryPH.NeutralPH;

    [DataField("strengthPerUnit")]
    public float StrengthPerUnit = 1f;

    public override void Effect(EntityEffectBaseArgs args)
    {
        if (args is not EntityEffectReagentArgs reagentArgs || reagentArgs.Source is not { } solution)
            return;

        AdjustPH(solution, reagentArgs.Quantity);
    }

    private void AdjustPH(Solution solution, FixedPoint2 quantity)
    {
        if (solution.Volume <= FixedPoint2.Zero || quantity <= FixedPoint2.Zero)
            return;

        var protoMan = IoCManager.Resolve<IPrototypeManager>();
        var currentPH = ChemistryPH.GetSolutionPH(solution, protoMan);
        var influence = Math.Clamp(quantity.Float() * StrengthPerUnit / solution.Volume.Float(), 0f, 1f);
        solution.PHOverride = Math.Clamp(
            currentPH + (Math.Clamp(TargetPH, ChemistryPH.MinPH, ChemistryPH.MaxPH) - currentPH) * influence,
            ChemistryPH.MinPH,
            ChemistryPH.MaxPH);
    }

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        return null;
    }
}
