using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Evil_Riggs.Midrow;
using Evil_Riggs.Actions;

namespace Evil_Riggs.Cards;


public class DoABarrelRoll : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.Evil_RiggsDeck.Deck,
                rarity = Rarity.rare,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "DoABarrelRoll", "name"]).Localize,
            Art = StableSpr.cards_Ace
        });
    }

    public override CardData GetData(State state)
    {

        return new CardData
        {
            cost = 2,
            exhaust = true
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
                            status = ModEntry.Instance.EvilRiggsBarrelRoll.Status,
                            statusAmount = 2,
                            targetPlayer = true
                        },
                    };
                }
            case Upgrade.A:
                {
                    return new List<CardAction>
                    {
                        new AMove
                        {
                            dir = -2,
                            targetPlayer = true,
                            isRandom = true
                        },
                        new AStatus
                        {
                            status = ModEntry.Instance.EvilRiggsBarrelRoll.Status,
                            statusAmount = 2,
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
                            status = ModEntry.Instance.EvilRiggsBarrelRoll.Status,
                            statusAmount = 4,
                            targetPlayer = true
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