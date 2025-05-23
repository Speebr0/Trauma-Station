using Content.Shared.Polymorph;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.EntityEffects.Effects;

public sealed partial class Polymorph : EventEntityEffect<Polymorph>
{
    /// <summary>
    ///     What polymorph prototype is used on effect
    /// </summary>
    [DataField("prototype", customTypeSerializer:typeof(PrototypeIdSerializer<PolymorphPrototype>))]
    public string PolymorphPrototype { get; set; }

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    // <Trauma> rewrite for goob optional polymorph.Entity
    => prototype.Index<PolymorphPrototype>(PolymorphPrototype).Configuration.Entity is {} entProto
        ? Loc.GetString("reagent-effect-guidebook-make-polymorph",
            ("chance", Probability), ("entityname", prototype.Index(entProto).Name))
        : null;
    // </Trauma>
}
