using FMOD;
using JetBrains.Annotations;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;

namespace Evil_Riggs.Artifacts;

public class DreadnaughtShielding : Artifact, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact(new ArtifactConfiguration
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new ArtifactMeta
            {
                pools = [ArtifactPool.Common],
                owner = ModEntry.Instance.Evil_RiggsDeck.Deck,
                unremovable = false
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "DreadnaughtShielding", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "DreadnaughtShielding", "desc"]).Localize,
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Artifact/artifact_dreadnoughtShields.png")).Sprite
        });
    }

    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
        if (card.GetData(state).infinite)
        {
            combat.Queue(new AStatus { status = Status.tempShield, targetPlayer = true, statusAmount = 1, artifactPulse = Key() });
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> list = new List<Tooltip>();
        list.Add(new TTGlossary("status.tempShieldAlt"));
        return list;
    }
}