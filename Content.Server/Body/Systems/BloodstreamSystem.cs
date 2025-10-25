using Content.Server.Body.Components;
using Content.Server.Fluids.EntitySystems;
using Content.Server.Popups;
using Content.Shared.Alert;
using Content.Shared.Body.Events;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Drunk;
using Content.Shared.EntityEffects.Effects;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Forensics;

namespace Content.Server.Body.Systems;

public sealed class BloodstreamSystem : SharedBloodstreamSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BloodstreamComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<BloodstreamComponent, GenerateDnaEvent>(OnDnaGenerated);
        SubscribeLocalEvent<BloodstreamComponent, ReactionAttemptEvent>(OnReactionAttempt); // Goobstation - moved to Server
        SubscribeLocalEvent<BloodstreamComponent, SolutionRelayEvent<ReactionAttemptEvent>>(OnReactionAttempt); // Goobstation - moved to Server
    }

    // not sure if we can move this to shared or not
    // it would certainly help if SolutionContainer was documented
    // but since we usually don't add the component dynamically to entities we can keep this unpredicted for now
    private void OnComponentInit(Entity<BloodstreamComponent> entity, ref ComponentInit args)
    {
        if (!SolutionContainer.EnsureSolution(entity.Owner,
                entity.Comp.ChemicalSolutionName,
                out var chemicalSolution) ||
            !SolutionContainer.EnsureSolution(entity.Owner,
                entity.Comp.BloodSolutionName,
                out var bloodSolution) ||
            !SolutionContainer.EnsureSolution(entity.Owner,
                entity.Comp.BloodTemporarySolutionName,
                out var tempSolution))
            return;

        chemicalSolution.MaxVolume = entity.Comp.ChemicalMaxVolume;
        bloodSolution.MaxVolume = entity.Comp.BloodMaxVolume;
        tempSolution.MaxVolume = entity.Comp.BleedPuddleThreshold * 4; // give some leeway, for chemstream as well

        // Fill blood solution with BLOOD
        // The DNA string might not be initialized yet, but the reagent data gets updated in the GenerateDnaEvent subscription
        bloodSolution.AddReagent(new ReagentId(entity.Comp.BloodReagent, GetEntityBloodData(entity.Owner)), entity.Comp.BloodMaxVolume - bloodSolution.Volume);
    }

    // forensics is not predicted yet
    private void OnDnaGenerated(Entity<BloodstreamComponent> entity, ref GenerateDnaEvent args)
    {
        if (SolutionContainer.ResolveSolution(entity.Owner, entity.Comp.BloodSolutionName, ref entity.Comp.BloodSolution, out var bloodSolution))
        {
            foreach (var reagent in bloodSolution.Contents)
            {
                List<ReagentData> reagentData = reagent.Reagent.EnsureReagentData();
                reagentData.RemoveAll(x => x is DnaData);
                reagentData.AddRange(GetEntityBloodData(entity.Owner));
            }
        }
        else
            Log.Error("Unable to set bloodstream DNA, solution entity could not be resolved");
    }

    private void OnReactionAttempt(Entity<BloodstreamComponent> ent, ref ReactionAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        foreach (var effect in args.Reaction.Effects)
        {
            switch (effect)
            {
                case CreateEntityReactionEffect: // Prevent entities from spawning in the bloodstream
                case AreaReactionEffect: // No spontaneous smoke or foam leaking out of blood vessels.
                    args.Cancelled = true;
                    return;
            }
        }

        // The area-reaction effect canceling is part of avoiding smoke-fork-bombs (create two smoke bombs, that when
        // ingested by mobs create more smoke). This also used to act as a rapid chemical-purge, because all the
        // reagents would get carried away by the smoke/foam. This does still work for the stomach (I guess people vomit
        // up the smoke or spawned entities?).

        // TODO apply organ damage instead of just blocking the reaction?
        // Having cheese-clots form in your veins can't be good for you.
    }

    private void OnReactionAttempt(Entity<BloodstreamComponent> ent, ref SolutionRelayEvent<ReactionAttemptEvent> args)
    {
        if (args.Name != ent.Comp.BloodSolutionName
            && args.Name != ent.Comp.ChemicalSolutionName
            && args.Name != ent.Comp.BloodTemporarySolutionName)
        {
            return;
        }

        OnReactionAttempt(ent, ref args.Event);
    }
}
