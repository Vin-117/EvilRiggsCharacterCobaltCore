using Evil_Riggs.Artifacts;
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
            if (args.NewAmount >= 7)
            {

                //Check if we have swarm preloader. If we do, make the 
                //missile swarm cost 0
                if ((from r in args.State.EnumerateAllArtifacts()
                     where r is SwarmPreloader
                     select r).ToList().Count > 0)
                {
                    args.Combat.QueueImmediate(
                        new AAddCard()
                        {
                            card = new EvilRiggsCard()
                            {
                                temporaryOverride = true,
                                exhaustOverride = true,
                                discount = -2
                            },
                            destination = CardDestination.Hand,
                            amount = 1,
                        });
                }
                //Otherwise just give it at its base cost
                else 
                {
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
                }
                //Regardless of what happens, set the status to 0
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

    //This block of code checks if the player has tempered rage, and
    //if they do, draws them extra cards if they are over 4 rage
    public bool HandleStatusTurnAutoStep(IHandleStatusTurnAutoStepArgs args)
    {

        //Do nothing if not start of turn
        if (args.Timing != StatusTurnTriggerTiming.TurnStart) 
        {
            return false;
        }

        //Do nothing if the player does not have the artifact
        if (!((from r in args.State.EnumerateAllArtifacts()
             where r is TemperedRage
             select r).ToList().Count > 0))
        {
            return false;
        }

        //Do nothing if this is not the rage status
        if (args.Status != ModEntry.Instance.EvilRiggsRage.Status) 
        {
            return false;
        }

        //If the code got this far, it has to have been the start of the turn
        //the player must have the artifact, and they must have the rage 
        //status
        if (args.Amount >= 4) 
        {
            args.Combat.QueueImmediate(
                new ADrawCard()
                {
                    count = 2
                });
        }
        return false;
    }
}

   


    
