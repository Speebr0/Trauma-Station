using Content.Shared.DoAfter;
using Content.Shared.Ensnaring;
using Content.Shared.Ensnaring.Components;
using Content.Shared.EntityEffects;

namespace Content.Trauma.Shared.EntityEffects.Effects;

public sealed class RemoveSnaresEffectSystem : EntityEffectSystem<EnsnareableComponent, RemoveSnares>
{
    [Dependency] private readonly DoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    protected override void Effect(Entity<EnsnareableComponent> ent, ref EntityEffectEvent<RemoveSnares> args)
    {
        var user = ent.Owner;

        // snare api is dogshit and i cbf to improve it
        foreach (var bola in ent.Comp.Container.ContainedEntities)
        {
            _doAfter.TryStartDoAfter(new DoAfterArgs(EntityManager, user, 0, new EnsnareableDoAfterEvent(), user, user, bola));
            _transform.DropNextTo(bola, user);
        }
    }
}
