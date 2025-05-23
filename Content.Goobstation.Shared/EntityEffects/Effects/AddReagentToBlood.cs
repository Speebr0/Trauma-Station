using Content.Shared.Chemistry.Reagent;
using Content.Shared.EntityEffects;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Body.Components;
using Content.Shared.Body.Systems;
using Robust.Shared.Prototypes;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;

namespace Content.Goobstation.Shared.EntityEffects.Effects;

/// <summary>
/// Trauma - Rewrote this shitcode and put it here instead of core files
/// </summary>
public sealed partial class AddReagentToBlood : EntityEffect
{
    private readonly SharedSolutionContainerSystem _solutionContainers;

    [DataField(required: true)]
    public ProtoId<ReagentPrototype> Reagent;

    [DataField(required: true)]
    public FixedPoint2 Amount = default!;

    public override void Effect(EntityEffectBaseArgs args)
    {
        if (args is not EntityEffectReagentArgs reagentArgs)
            return;

        if (!args.EntityManager.TryGetComponent<BloodstreamComponent>(args.TargetEntity, out var blood))
            return;

        var sys = args.EntityManager.System<SharedBloodstreamSystem>();
        var amt = Amount;
        var solution = new Solution();
        solution.AddReagent(Reagent, amt);
        sys.TryAddToChemicals((args.TargetEntity, blood), solution);
    }

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        var proto = prototype.Index(Reagent);
        return Loc.GetString("reagent-effect-guidebook-add-to-chemicals",
            ("chance", Probability),
            ("deltasign", MathF.Sign(Amount.Float())),
            ("reagent", proto.LocalizedName),
            ("amount", MathF.Abs(Amount.Float())));
    }
}
