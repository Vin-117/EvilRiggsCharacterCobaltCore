using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Evil_Riggs.Midrow;
using Evil_Riggs.Actions;

namespace Evil_Riggs.Cards;


public class WildShot : Card, IRegisterable
{

    //private static ISpriteEntry DoubleMissileArt = null!;
    //private static ISpriteEntry SingleMissileArt = null!;

    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {

        //DoubleMissileArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/VicSeekerSwarm.png"));
        //SingleMissileArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/VicbasicMissile.png"));

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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "WildShot", "name"]).Localize,
            //Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/VicbasicMissile.png")).Sprite,
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
                        cost = 2,
                    };
                }
            case Upgrade.A:
                {
                    return new CardData
                    {
                        cost = 2,
                    };
                }
            case Upgrade.B:
                {
                    return new CardData
                    {
                        cost = 2
                    };
                }
            default:
                {
                    return new CardData
                    {
                        cost = 2
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
                            }
                        },
                        new AMove
                        {
                            isRandom = true,
                            dir = 2,
                            targetPlayer = true
                        },
                        new ADroneRandomMove
                        {
                            dir = 1,
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
            case Upgrade.A:
                {
                    return new List<CardAction>
                    {
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0,
                                missileType = MissileType.heavy
                            }
                        },
                        new AMove
                        {
                            isRandom = true,
                            dir = 2,
                            targetPlayer = true
                        },
                        new ADroneRandomMove
                        {
                            dir = 1,
                        },
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0,
                                missileType = MissileType.seeker
                            }
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
                                yAnimation = 0.0
                            }
                        },
                        new AMove
                        {
                            isRandom = true,
                            dir = 2,
                            targetPlayer = true
                        },
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0
                            }
                        },
                        new ADroneRandomMove
                        {
                            dir = 1,
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