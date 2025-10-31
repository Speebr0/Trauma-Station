using Content.Shared.Trigger.Systems;

namespace Content.Trauma.Shared.Trigger;

public sealed class AutoStartTimerSystem : EntitySystem
{
    [Dependency] private readonly TriggerSystem _trigger = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AutoStartTimerComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(Entity<AutoStartTimerComponent> ent, ref MapInitEvent args)
    {
        _trigger.ActivateTimerTrigger(ent.Owner);
    }
}
