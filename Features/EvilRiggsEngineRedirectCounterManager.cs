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
using System.Linq.Expressions;
using static Evil_Riggs.External.IKokoroApi.IV2;
using static Evil_Riggs.External.IKokoroApi.IV2.IStatusLogicApi;
using static Evil_Riggs.External.IKokoroApi.IV2.IStatusLogicApi.IHook;

namespace Evil_Riggs.Features;


public class EvilRiggsEngineRedirectCounterManager : IKokoroApi.IV2.IStatusLogicApi.IHook, IKokoroApi.IV2.IStatusRenderingApi.IHook
{

    public EvilRiggsEngineRedirectCounterManager()
    {
        ModEntry.Instance.KokoroApi.StatusLogic.RegisterHook(this, 0);
        ModEntry.Instance.KokoroApi.StatusRendering.RegisterHook(this, 0);
    }

    private static Status EvilRiggsEngineRedirectCounter => ModEntry.Instance.EvilRiggsEngineRedirectCounter.Status;


    /*public IReadOnlyList<Tooltip> OverrideStatusTooltips(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusTooltipsArgs args)
        => args.Status == ModEntry.Instance.EvilRiggsRage.Status
            ?[
                .. args.Tooltips,
                new TTCard { card = new EvilRiggsCard() }
            ] : args.Tooltips;*/


    public bool HandleStatusTurnAutoStep(IHandleStatusTurnAutoStepArgs args)
    {

        if (args.Status != EvilRiggsEngineRedirectCounter)
            return false;
        if (args.Timing != StatusTurnTriggerTiming.TurnEnd)
            return false;
        if (args.Amount > 0)
        {
            args.Combat.QueueImmediate(
                new AStatus()
                {
                    status = EvilRiggsEngineRedirectCounter,
                    statusAmount = -1 * args.Amount,
                    targetPlayer = true
                });
        }
        return false;
    }
}

   


    
