using Evil_Riggs.Actions;
using Evil_Riggs.CardActions;
using Evil_Riggs.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Evil_Riggs.Cards;


public class TargetLock : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "TargetLock", "name"]).Localize,
            Art = StableSpr.cards_MultiBlast
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
                        cost = 0
                    };
                }
            case Upgrade.A:
                {
                    return new CardData
                    {
                        cost = 0,
                        retain = true
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
                        cost = 0
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
                        new ATargetLock{}
                    };
                }
            case Upgrade.A:
                {
                    return new List<CardAction>
                    {
                        new ATargetLock{}
                    };
                }
            case Upgrade.B:
                {
                    return new List<CardAction>
                    {
                        new ATargetLock{},
                        new ASpawn
                        {
                            thing = new Missile
                            {
                                missileType = MissileType.seeker,
                                yAnimation = 0.0
                            }
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