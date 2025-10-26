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

    public override string? EntityEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => prototype.Index(Proto).Effect.EntityEffectGuidebookText(prototype, entSys);
}

/// <summary>
/// Handles <see cref="NestedEffect"/> and provides API for applying one directly in code.
/// </summary>
public sealed class NestedEffectSystem : EntityEffectSystem<TransformComponent, NestedEffect>
{
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly SharedEntityEffectsSystem _effects = default!;

    protected override void Effect(Entity<TransformComponent> ent, ref EntityEffectEvent<NestedEffect> args)
    {
        ApplyNestedEffect(ent, args.Effect.Proto, args.Scale);
    }

    public void ApplyNestedEffect(EntityUid target, ProtoId<EntityEffectPrototype> id, float scale = 1f)
    {
        var effect = _proto.Index(id).Effect;
        _effects.TryApplyEffect(target, effect, scale);
    }
}
