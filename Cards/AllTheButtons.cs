using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Evil_Riggs.Midrow;
using Evil_Riggs.Actions;

namespace Evil_Riggs.Cards;


public class AllTheButtons : Card, IRegisterable
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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "AllTheButtons", "name"]).Localize,
            Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/cardart_allTheButtons.png")).Sprite,
        });
    }

    public override CardData GetData(State state)
    {
        return new CardData
        {
            cost = 3
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
                        new AAttack
                        {
                            damage = GetDmg(s, 3)
                        },
                        new AMove
                        {
                            dir = -3,
                            targetPlayer = true,
                            isRandom = true
                        },
                        new AStatus
                        {
                            status = ModEntry.Instance.EvilRiggsRage.Status,
                            statusAmount = 3,
                            targetPlayer = true
                        },
                    };
                }
            case Upgrade.A:
                {
                    return new List<CardAction>
                    {
                        new AAttack
                        {
                            damage = GetDmg(s, 3)
                        },
                        new AMove
                        {
                            dir = -4,
                            targetPlayer = true,
                            isRandom = true
                        },
                        new AStatus
                        {
                            status = ModEntry.Instance.EvilRiggsRage.Status,
                            statusAmount = 5,
                            targetPlayer = true
                        },
                    };
                }
            case Upgrade.B:
                {
                    return new List<CardAction>
                    {
                        new AAttack
                        {
                            damage = GetDmg(s, 6)
                        },
                        new AMove
                        {
                            dir = -3,
                            targetPlayer = true,
                            isRandom = true
                        },
                        new AStatus
                        {
                            status = ModEntry.Instance.EvilRiggsRage.Status,
                            statusAmount = 3,
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