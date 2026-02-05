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


public class EvilRiggsRageManager : IKokoroApi.IV2.IStatusLogicApi.IHook, IKokoroApi.IV2.IStatusRenderingApi.IHook
{

    public EvilRiggsRageManager()
    {
        ModEntry.Instance.KokoroApi.StatusLogic.RegisterHook(this, 0);
        ModEntry.Instance.KokoroApi.StatusRendering.RegisterHook(this, 0);
    }

    public IReadOnlyList<Tooltip> OverrideStatusTooltips(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusTooltipsArgs args)
        => args.Status == ModEntry.Instance.EvilRiggsRage.Status
            ?[
                .. args.Tooltips,
                new TTCard { card = new EvilRiggsCard() }
            ] : args.Tooltips;
        

    public bool HandleStatusTurnAutoStep(IHandleStatusTurnAutoStepArgs args)
    {

        if (args.Status != ModEntry.Instance.EvilRiggsRage.Status)
            return false;
        //if (args.Timing.)
        if (args.Amount >= 7)
        {
            args.Combat.QueueImmediate(
                new AAddCard()
                {
                    card = new EvilRiggsCard()
                    {
                    },
                    destination = CardDestination.Hand,
                    amount = 1,
                });
            args.Combat.Queue(
                new AStatus() 
                {
                    status = ModEntry.Instance.EvilRiggsRage.Status,
                    statusAmount = -1*args.Amount,
                    targetPlayer = true
                });
        }
        return false;
    }
}

   


    
