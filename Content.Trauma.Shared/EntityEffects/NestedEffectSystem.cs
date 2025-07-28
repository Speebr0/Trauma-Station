using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Trauma.Shared.EntityEffects;

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
