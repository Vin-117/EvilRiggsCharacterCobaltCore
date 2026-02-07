using Evil_Riggs.Midrow;
using Evil_Riggs.CardActions;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Evil_Riggs.Cards;


public class FireAtWill : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "FireAtWill", "name"]).Localize,
            Art = StableSpr.cards_SeekerMissileCard
        });
    }

    public override CardData GetData(State state)
    {
        switch(this.upgrade)
        {
            case Upgrade.None: 
                {
                    return new CardData
                    {
                        cost = 1,
                        infinite = true
                    };
                }
            case Upgrade.A:
                {
                    return new CardData
                    {
                        cost = 1,
                        infinite = true
                    };
                }
            case Upgrade.B:
                {
                    return new CardData
                    {
                        cost = 0
                    };
                }
            default:
                {
                    return new CardData
                    {
                        cost = 1,
                        infinite = true
                    };
                }
        }
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        switch (this.upgrade)
        {
            case Upgrade.None:
                {
                    return new List<CardAction>
                    {
                        new AMissileTurn
                        {
                        },
                        new ASpawn
                        {
                            thing = new LightMissile
                            {
                                yAnimation = 0.0
                            },
                        }
                    };
                }
            case Upgrade.A:
                {
                    return new List<CardAction>
                    {
                        new AMissileTurn
                        {
                        },
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0
                            },
                        }
                    };
                }
            case Upgrade.B:
                {
                    return new List<CardAction>
                    {
                        new AMissileTurn
                        {
                        },
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0
                            },
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