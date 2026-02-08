using FMOD;
using JetBrains.Annotations;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Evil_Riggs.Artifacts;

public class SpiltBoba : Artifact, IRegisterable
{

    private static Spr SpiltSpr;

    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {

        //UnSpiltSpr = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Artifact/artifact_spiltBoba.png")).Sprite;
        SpiltSpr = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Artifact/artifact_spiltBobaUsed.png")).Sprite;


        helper.Content.Artifacts.RegisterArtifact(new ArtifactConfiguration
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new ArtifactMeta
            {
                pools = [ArtifactPool.Common],
                owner = ModEntry.Instance.Evil_RiggsDeck.Deck,
                unremovable = false
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "SpiltBoba", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "SpiltBoba", "desc"]).Localize,
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Artifact/artifact_spiltBoba.png")).Sprite
        });
    }

    public override Spr GetSprite()
    {
        if (YouSpilledRiggsBobaNowYoureInForIt)
        {
            return SpiltSpr;
        }
        else 
        {
            return base.GetSprite();
        }
    }

    public bool YouSpilledRiggsBobaNowYoureInForIt = false;

    public override void OnPlayerTakeNormalDamage(State state, Combat combat, int rawAmount, Part? part)
    {

        if (!YouSpilledRiggsBobaNowYoureInForIt) 
        {
            Pulse();
            YouSpilledRiggsBobaNowYoureInForIt = true;
            combat.QueueImmediate
            (
                new AStatus
                {
                    targetPlayer = true,
                    statusAmount = 1,
                    status = ModEntry.Instance.EvilRiggsRage.Status
                }
            );
            combat.QueueImmediate
            (
                new AStatus
                {
                    targetPlayer = true,
                    statusAmount = 1,
                    status = Status.drawLessNextTurn
                }
            );
        }
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        YouSpilledRiggsBobaNowYoureInForIt = false;
    }

    public override void OnCombatEnd(State state)
    {
        YouSpilledRiggsBobaNowYoureInForIt = false;
    }

    public override List<Tooltip> GetExtraTooltips()
    {
        List<Tooltip> bobatips = StatusMeta.GetTooltips(ModEntry.Instance.EvilRiggsRage.Status, 1);
        return bobatips;
    }

}