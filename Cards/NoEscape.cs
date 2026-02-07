using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Evil_Riggs.Midrow;
using Evil_Riggs.Actions;

namespace Evil_Riggs.Cards;


public class NoEscape : Card, IRegisterable
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
                rarity = Rarity.rare,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "NoEscape", "name"]).Localize,
            //Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/VicbasicMissile.png")).Sprite,
        });
    }

    public override CardData GetData(State state)
    {
        switch (this.upgrade)
        {
            case Upgrade.None: 
                {
                    return new CardData
                    {
                        cost = 1
                    };
                }
            case Upgrade.A:
                {
                    return new CardData
                    {
                        cost = 1,
                        exhaust = true
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
                        cost = 1
                    };
                }
        }
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        int GetNoEscapeTotal(State s)
        {
            int num = 0;
            if (s.route is Combat combat)
            {
                num = combat.hand.Count - 1;
            }
            return num;
        }
        switch (this.upgrade)
        {
            case Upgrade.None:
                {
                    return new List<CardAction>
                    {
                        new AVariableHint
                        {
                            hand = true,
                            handAmount = GetNoEscapeTotal(s)
                        },
                        new AStatus
                        {
                            status = Status.evade,
                            targetPlayer = true,
                            statusAmount = GetNoEscapeTotal(s),
                            xHint = 1
                        },
                        new AStatus
                        {
                            status = Status.heat,
                            targetPlayer = true,
                            statusAmount = GetNoEscapeTotal(s),
                            xHint = 1
                        },
                    };
                }
            case Upgrade.A:
                {
                    return new List<CardAction>
                    {
                        new AVariableHint
                        {
                            hand = true,
                            handAmount = GetNoEscapeTotal(s)
                        },
                        new AStatus
                        {
                            status = Status.evade,
                            targetPlayer = true,
                            statusAmount = GetNoEscapeTotal(s),
                            xHint = 1
                        }
                    };
                }
            case Upgrade.B:
                {
                    return new List<CardAction>
                    {
                        new AVariableHint
                        {
                            hand = true,
                            handAmount = GetNoEscapeTotal(s)
                        },
                        new AStatus
                        {
                            status = Status.evade,
                            targetPlayer = true,
                            statusAmount = GetNoEscapeTotal(s),
                            xHint = 1
                        },
                        new AStatus
                        {
                            status = ModEntry.Instance.EvilRiggsRage.Status,
                            statusAmount = GetNoEscapeTotal(s),
                            targetPlayer = true,
                            xHint = 1
                        },
                        new AStatus
                        {
                            status = Status.heat,
                            targetPlayer = true,
                            statusAmount = GetNoEscapeTotal(s),
                            xHint = 1
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