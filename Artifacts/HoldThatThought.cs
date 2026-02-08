using FMOD;
using JetBrains.Annotations;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Evil_Riggs.Artifacts;

public class HoldThatThought : Artifact, IRegisterable
{

    private static Spr HoldThatThoughtUsedSprite;
    public bool HoldThatThoughtUsed = false;

    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {

        HoldThatThoughtUsedSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Artifact/artifact_holdThatThoughtUsed.png")).Sprite;


        helper.Content.Artifacts.RegisterArtifact(new ArtifactConfiguration
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new ArtifactMeta
            {
                pools = [ArtifactPool.Boss],
                owner = ModEntry.Instance.Evil_RiggsDeck.Deck,
                unremovable = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "HoldThatThought", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "HoldThatThought", "desc"]).Localize,
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Artifact/artifact_holdThatThought.png")).Sprite
        });
    }

    public override Spr GetSprite()
    {
        if (HoldThatThoughtUsed)
        {
            return HoldThatThoughtUsedSprite;
        }
        else 
        {
            return base.GetSprite();
        }
    }

    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
        if (card.GetData(state).infinite && card.GetData(state).cost > 0 && !card.GetData(state).retain && !HoldThatThoughtUsed)
        {
            HoldThatThoughtUsed = true;
            card.retainOverride = true;
            Pulse();
        }
    }

    public override void OnCombatEnd(State state)
    {
       HoldThatThoughtUsed = false;
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> list = new List<Tooltip>();
        list.Add(new TTGlossary("cardtrait.infinite"));
        list.Add(new TTGlossary("cardtrait.retain"));
        return list;
    }

}