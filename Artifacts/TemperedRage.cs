using FMOD;
using JetBrains.Annotations;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Evil_Riggs.Artifacts;

public class TemperedRage : Artifact, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "TemperedRage", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "TemperedRage", "desc"]).Localize,
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Artifact/artifact_temperedRage.png")).Sprite
        });
    }

    public override List<Tooltip> GetExtraTooltips()
    {
        List<Tooltip> tempragetips = StatusMeta.GetTooltips(ModEntry.Instance.EvilRiggsRage.Status, 1);
        tempragetips.Insert(0, new TTGlossary("action.drawCard", 2));
        return tempragetips;
    }
}