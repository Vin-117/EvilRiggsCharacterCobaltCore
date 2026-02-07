using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Evil_Riggs.Midrow;
using Evil_Riggs.Actions;

namespace Evil_Riggs.Cards;


public class BurstFire : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BurstFire", "name"]).Localize,
            Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/cardart_rocketFactory.png")).Sprite,
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
                        cost = 2
                    };
                }
            case Upgrade.A:
                {
                    return new CardData
                    {
                        cost = 2
                    };
                }
            case Upgrade.B:
                {
                    return new CardData
                    {
                        cost = 3
                    };
                }
            default:
                {
                    return new CardData
                    {
                        cost = 1
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
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0
                            },
                            offset = -1
                        },
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0
                            },
                            offset = 1
                        },
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
                                yAnimation = 0.0
                            },
                            offset = -1
                        },
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0
                            }
                        },
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0
                            },
                            offset = 1
                        },
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
                                yAnimation = 0.0,
                                missileType = MissileType.heavy,
                            },
                            offset = -2
                        },
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0,
                                missileType = MissileType.heavy,
                            }
                        },
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0,
                                missileType = MissileType.heavy,
                            },
                            offset = 2
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