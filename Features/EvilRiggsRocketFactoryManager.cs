using Evil_Riggs.Actions;
using Evil_Riggs.CardActions;
using Evil_Riggs.External;
using HarmonyLib;
using JetBrains.Annotations;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static Evil_Riggs.External.IKokoroApi.IV2;
using static Evil_Riggs.External.IKokoroApi.IV2.IStatusLogicApi;
using static Evil_Riggs.External.IKokoroApi.IV2.IStatusLogicApi.IHook;

namespace Evil_Riggs.Features;


public class EvilRiggsRocketFactoryManager : IKokoroApi.IV2.IStatusLogicApi.IHook, IKokoroApi.IV2.IStatusRenderingApi.IHook
{

    public EvilRiggsRocketFactoryManager()
    {
        ModEntry.Instance.KokoroApi.StatusLogic.RegisterHook(this, 0);
        ModEntry.Instance.KokoroApi.StatusRendering.RegisterHook(this, 0);
    }

    private static Status EvilRiggsRocketFactory => ModEntry.Instance.EvilRiggsRocketFactory.Status;


    /*public IReadOnlyList<Tooltip> OverrideStatusTooltips(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusTooltipsArgs args)
        => args.Status == ModEntry.Instance.EvilRiggsRage.Status
            ?[
                .. args.Tooltips,
                new TTCard { card = new EvilRiggsCard() }
            ] : args.Tooltips;*/


    public bool HandleStatusTurnAutoStep(IHandleStatusTurnAutoStepArgs args)
    {

        if (args.Status != EvilRiggsRocketFactory)
            return false;
        if (args.Timing != StatusTurnTriggerTiming.TurnStart)
            return false;
        if (args.Amount > 0)
        {
            for (int i = 0; i < args.Amount; i++)
            {
                args.Combat.QueueImmediate(
                new ASpawn()
                {
                    thing = new Missile
                    {
                        yAnimation = 0.0
                    }
                });
            }
        }
        return false;
    }
}

   


    
