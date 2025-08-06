using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Trauma.Shared.EntityEffects.Effects;

/// <summary>
/// Removes bolas from the target entity.
/// </summary>
public sealed partial class RemoveSnares : EntityEffectBase<RemoveSnares>
{
    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("entity-effect-guidebook-remove-snares");
}
