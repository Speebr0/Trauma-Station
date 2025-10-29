using Content.Shared.EntityEffects;
using Content.Shared.Trigger;

namespace Content.Trauma.Trigger.Effects;

public sealed class EffectsOnTriggerSystem : EntitySystem
{
    [Dependency] private readonly SharedEntityEffectsSystem _effects = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<EffectsOnTriggerComponent, TriggerEvent>(OnTrigger);
    }

    private void OnTrigger(Entity<EffectsOnTriggerComponent> ent, ref TriggerEvent args)
    {
        if (args.Key != null && !ent.Comp.KeysIn.Contains(args.Key))
            return;

        var target = ent.Owner;
        if (ent.Comp.TargetUser)
        {
            if (args.User is not {} user)
                return;

            target = user;
        }

        _effects.ApplyEffects(target, ent.Comp.Effects);
    }
}
