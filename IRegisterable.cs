using Nanoray.PluginManager;
using Nickel;

namespace Evil_Riggs;

internal interface IRegisterable
{
    static abstract void Register(IPluginPackage<IModManifest> package, IModHelper helper);
}