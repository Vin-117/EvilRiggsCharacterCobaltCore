using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Evil_Riggs.Midrow;
using Evil_Riggs.Actions;

namespace Evil_Riggs.Cards;


public class Scheme : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = ModEntry.Instance.Evil_RiggsDeck.Deck,
                rarity = Rarity.common,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Scheme", "name"]).Localize,
            Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/cardart_ragedraw.png")).Sprite,
        });
    }

    public override CardData GetData(State state)
    {
        return new CardData
        {
            cost = 1
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
                            status = ModEntry.Instance.EvilRiggsRage.Status,
                            statusAmount = 2,
                            targetPlayer = true
                        },
                        new ADrawCard
                        {
                            count = 2
                        }
                    };
                }
            case Upgrade.A:
                {
                    return new List<CardAction>
                    {
                        new AStatus
                        {
                            status = ModEntry.Instance.EvilRiggsRage.Status,
                            statusAmount = 2,
                            targetPlayer = true
                        },
                        new ADrawCard
                        {
                            count = 3
                        }
                    };
                }
            case Upgrade.B:
                {
                    return new List<CardAction>
                    {
                        new AStatus
                        {
                            status = ModEntry.Instance.EvilRiggsRage.Status,
                            statusAmount = 3,
                            targetPlayer = true
                        },
                        new ADrawCard
                        {
                            count = 2
                        }
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