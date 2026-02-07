using Evil_Riggs.Actions;
using Evil_Riggs.CardActions;
using Evil_Riggs.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Evil_Riggs.Cards;


public class ReadyOrNot : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.Evil_RiggsDeck.Deck,
                rarity = Rarity.uncommon,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ReadyOrNot", "name"]).Localize,
            Art = StableSpr.cards_Prepare
        });
    }

    public override CardData GetData(State state)
    {
        return new CardData
        {
            cost = 1,
            infinite = true
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        switch (this.upgrade)
        {
            case Upgrade.None:
                {
                    return new List<CardAction>
                    {
                        new AStatus
                        {
                            status = Status.drawNextTurn,
                            statusAmount = 1,
                            targetPlayer = true
                        },
                        new AStatus
                        {
                            statusAmount = 1,
                            targetPlayer = true,
                            status =  ModEntry.Instance.EvilRiggsDiscountNextTurn.Status
                        },
                    };
                }
            case Upgrade.A:
                {
                    return new List<CardAction>
                    {
                        new AStatus
                        {
                            status = Status.drawNextTurn,
                            statusAmount = 1,
                            targetPlayer = true
                        },
                        new AStatus
                        {
                            status = Status.energyNextTurn,
                            statusAmount = 1,
                            targetPlayer = true
                        },
                    };
                }
            case Upgrade.B:
                {
                    return new List<CardAction>
                    {
                        new AStatus
                        {
                            status = Status.drawNextTurn,
                            statusAmount = 1,
                            targetPlayer = true
                        },
                        new AStatus
                        {
                            statusAmount = 1,
                            targetPlayer = true,
                            status =  ModEntry.Instance.EvilRiggsDiscountNextTurn.Status
                        },
                        new AStatus
                        {
                            statusAmount = 1,
                            targetPlayer = true,
                            status =  ModEntry.Instance.EvilRiggsRage.Status
                        },
                    };
                }
            default:
                {
                    return new List<CardAction>
                    {
                    };
                }
        }
    }
}