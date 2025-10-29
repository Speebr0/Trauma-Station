using Content.Shared.EntityEffects;
using Content.Shared.Trigger.Components.Effects;
using Robust.Shared.GameStates;

namespace Content.Trauma.Trigger.Effects;

/// <summary>
/// Runs entity effects on the entity or user.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(EffectsOnTriggerSystem))]
[AutoGenerateComponentState]
public sealed partial class EffectsOnTriggerComponent : BaseXOnTriggerComponent
{
    [DataField(required: true)]
    public EntityEffect[] Effects = default!;
}
