using Content.Goobstation.Shared.EntityConditions;
using Content.Server.Atmos.EntitySystems;
using Content.Shared._Lavaland.Procedural.Components;

namespace Content.Goobstation.Server.EntityConditions;

public sealed class PressureConditionSystem : EntityConditionSystem<TransformComponent, PressureCondition>
{
    [Dependency] private readonly AtmosphereSystem _atmosphere;

    protected override bool Condition(Entity<TransformComponent> ent, ref EntityConditionEvent<PressureCondition> args)
    {
        if (WorksOnLavaland && HasComp<LavalandMapComponent>(ent.Comp.MapUid))
        {
            args.Result = true;
            return;
        }

        var mix = _atmos.GetTileMixture(ent);
        var pressure = mix?.Pressure ?? 0f;
        args.Result = pressure >= Min && pressure <= Max;
    }
}
