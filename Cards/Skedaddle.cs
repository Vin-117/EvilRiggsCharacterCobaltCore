using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Evil_Riggs.Midrow;
using Evil_Riggs.Actions;

namespace Evil_Riggs.Cards;


public class Skedaddle : SequentialCard, IRegisterable, IHasCustomCardTraits//Card, IRegisterable
{

    private static ISpriteEntry TopHalfArt = null!;
    private static ISpriteEntry BottomHalfArt = null!;

    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {

        TopHalfArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/cardart_seq_offset_top.png"));
        BottomHalfArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/cardart_seq_offset_bottom.png"));

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
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Skedaddle", "name"]).Localize,
            //Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/VicbasicMissile.png")).Sprite,
        });
    }

    public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state)
    {
        switch (this.upgrade)
        {
            case Upgrade.None:
                {
                    return new HashSet<ICardTraitEntry> { ModEntry.Instance.SequentialTrait };
                }
            case Upgrade.A:
                {
                    return new HashSet<ICardTraitEntry> { ModEntry.Instance.SequentialTrait };
                }
            case Upgrade.B:
                {
                    return new HashSet<ICardTraitEntry> { ModEntry.Instance.SequentialTrait };
                }
            default:
                {
                    return new HashSet<ICardTraitEntry> { ModEntry.Instance.SequentialTrait };
                }
        }

    }

    public override CardData GetData(State state)
    {
        switch(this.upgrade)
        {
            case Upgrade.None: 
                {
                    return new CardData
                    {
                        art = (SequenceInitiated ? BottomHalfArt : TopHalfArt).Sprite,
                        cost = 1,
                        infinite = true
                    };
                }
            case Upgrade.A:
                {
                    return new CardData
                    {
                        art = (SequenceInitiated ? BottomHalfArt : TopHalfArt).Sprite,
                        cost = 1,
                        infinite = true
                    };
                }
            case Upgrade.B:
                {
                    return new CardData
                    {
                        art = (SequenceInitiated ? BottomHalfArt : TopHalfArt).Sprite,
                        cost = 1,
                        infinite = true
                    };
                }
            default:
                {
                    return new CardData
                    {
                        art = (SequenceInitiated ? BottomHalfArt : TopHalfArt).Sprite,
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
                        new AMove
                        {
                            dir = -1,
                            isRandom = true,
                            targetPlayer = true,
                            disabled = SequenceInitiated
                        },
                        new AStatus
                        {
                            status = Status.hermes,
                            statusAmount = 1,
                            targetPlayer = true,
                            disabled = SequenceInitiated
                        },
                        new ASequential
                        {
                            CardId = uuid
                        },
                        new AMove
                        {
                            dir = -1,
                            targetPlayer = true,
                            isRandom = true,
                            disabled = !SequenceInitiated
                        }
                        
                    };
                }
            case Upgrade.A:
                {
                    return new List<CardAction>
                    {
                        new AStatus
                        {
                            status = Status.evade,
                            statusAmount = 1,
                            targetPlayer = true,
                            disabled = SequenceInitiated
                        },
                        new AStatus
                        {
                            status = Status.hermes,
                            statusAmount = 1,
                            targetPlayer = true,
                            disabled = SequenceInitiated
                        },
                        new ASequential
                        {
                            CardId = uuid
                        },
                        new AMove
                        {
                            dir = -1,
                            targetPlayer = true,
                            isRandom = true,
                            disabled = !SequenceInitiated
                        }
                    };
                }
            case Upgrade.B:
                {
                    return new List<CardAction>
                    {
                        new AMove
                        {
                            dir = -2,
                            isRandom = true,
                            targetPlayer = true,
                            disabled = SequenceInitiated
                        },
                        new AStatus
                        {
                            status = Status.hermes,
                            statusAmount = 1,
                            targetPlayer = true,
                            disabled = SequenceInitiated
                        },
                        new ASequential
                        {
                            CardId = uuid
                        },
                        new AMove
                        {
                            dir = -2,
                            targetPlayer = true,
                            isRandom = true,
                            disabled = !SequenceInitiated
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