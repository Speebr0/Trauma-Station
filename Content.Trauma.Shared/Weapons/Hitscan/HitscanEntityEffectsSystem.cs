using Content.Shared.EntityEffects;
using Content.Shared.Weapons.Hitscan;

namespace Content.Trauma.Shared.Weapons.Hitscan;

public sealed class HitscanEntityEffectsSystem : EntitySystem
{
    [Dependency] private readonly SharedEntityEffectsSystem _effects = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HitscanEntityEffectsComponent, HitscanRaycastFiredEvent>(OnFired);
    }

    private void OnFired(Entity<HitscanEntityEffectsComponent> ent, ref HitscanRaycastFiredEvent args)
    {
        if (args.HitEntity is not {} target)
            return;

        _effects.ApplyEffects(target, ent.Comp.Effects);
    }
}
