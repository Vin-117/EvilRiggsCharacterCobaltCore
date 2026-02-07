using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Evil_Riggs.Midrow;
using Evil_Riggs.Actions;

namespace Evil_Riggs.Cards;


public class Jostle : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Jostle", "name"]).Localize,
            Art = StableSpr.cards_SeekerMissileCard
        });
    }

    public override CardData GetData(State state)
    {
        return new CardData
        {
            cost = 1,
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
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0
                            }
                        },
                        new ADroneRandomMove
                        {
                            dir = 2,
                        }
                    };
                }
            case Upgrade.A:
                {
                    return new List<CardAction>
                    {
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                missileType = MissileType.seeker,
                                yAnimation = 0.0
                            }
                        },
                        new ADroneRandomMove
                        {
                            dir = 2,
                        }
                    };
                }
            case Upgrade.B:
                {
                    return new List<CardAction>
                    {
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0
                            }
                        },
                        new ADroneRandomMove
                        {
                            dir = 3,
                        },
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0
                            }
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