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

    private static Status Evil_Rage => ModEntry.Instance.EvilRiggsRage.Status;


    public IReadOnlyList<Tooltip> OverrideStatusTooltips(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusTooltipsArgs args)
        => args.Status == ModEntry.Instance.EvilRiggsRage.Status
            ?[
                .. args.Tooltips,
                new TTCard { card = new EvilRiggsCard() }
            ] : args.Tooltips;


    public int ModifyStatusChange(IModifyStatusChangeArgs args) 
    {
        //Check if the status is the Rage status
        if (args.Status == ModEntry.Instance.EvilRiggsRage.Status)
        {

            //if it is, check if its equal to or over 7
            
            //if (args.Ship.Get(ModEntry.Instance.EvilRiggsRage.Status) >= 7)
            if (args.NewAmount >= 7)
            {
                //if it is equal to or over 7, give the missile swarm card
                //and set it to 0
                args.Combat.QueueImmediate(
                new AAddCard()
                {
                    card = new EvilRiggsCard()
                    {
                        temporaryOverride = true,
                        exhaustOverride = true
                    },
                    destination = CardDestination.Hand,
                    amount = 1,
                });
                return 0;
            }
            else 
            {
                //the rage status is not over or equal to 7, so do nothing
                return args.NewAmount;
            }
        }
        else 
        {
            //if not the rage status, do nothing
            return args.NewAmount;
        }
    }
}

   


    
