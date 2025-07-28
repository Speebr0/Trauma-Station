using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Trauma.Shared.EntityEffects;

/// <summary>
/// Applies the effect of an <see cref="EntityEffectPrototype"/>.
/// </summary>
public sealed partial class NestedEffect : EntityEffectBase<NestedEffect>
{
    /// <summary>
    /// The effect prototype to use.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<EntityEffectPrototype> Proto;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => prototype.Index(Proto).Effect.ReagentEffectGuidebookText(prototype, entSys);
}
