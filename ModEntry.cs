using Evil_Riggs.Actions;
using Evil_Riggs.CardActions;
using Evil_Riggs.Cards;
using Evil_Riggs.External;
using Evil_Riggs.Features;
using Evil_Riggs.Midrow;
using Evil_Riggs.Artifacts;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Evil_Riggs;

internal class ModEntry : SimpleMod
{

    //Setup modentry, harmony, and kokoro instances
    internal static ModEntry Instance { get; private set; } = null!;
    internal Harmony Harmony;
    internal IKokoroApi.IV2 KokoroApi;

    //Setup localization
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }

    //Register deck
    internal IDeckEntry Evil_RiggsDeck;

    //Register status variables
    internal IStatusEntry EvilRiggsRage;
    internal IStatusEntry EvilRiggsDiscountNextTurn;
    internal IStatusEntry EvilRiggsRocketFactory;
    internal IStatusEntry EvilRiggsBarrelRoll;
    internal IStatusEntry EvilRiggsEngineRedirect;
    internal IStatusEntry EvilRiggsEngineRedirectCounter;

    // Register cards
    private static List<Type> Evil_RiggsCommonCardTypes =
    [
        typeof(LightMissileCard),
        typeof(Skedaddle),
        typeof(Jostle),
        typeof(WildShot),
        typeof(Airburst),
        typeof(Bide),
        typeof(Scheme),
        typeof(JammedBarrel),
        typeof(FireAtWill)
    ];
    private static List<Type> Evil_RiggsUncommonCardTypes =
    [
        typeof(AllTheButtons),
        typeof(Brilliance),
        typeof(BurstFire),
        typeof(Swerve),
        typeof(SteamEngine),
        typeof(TargetLock),
        typeof(ReadyOrNot)
    ];
    private static List<Type> Evil_RiggsRareCardTypes = 
    [
        typeof(RiggsFinaleCard),
        typeof(NoEscape),
        typeof(RocketFactory),
        typeof(DoABarrelRoll),
        typeof(EngineRedirect)
    ];
    private static List<Type> Evil_RiggsSpecialCardTypes = 
    [
    ];

    private static IEnumerable<Type> Evil_RiggsCardTypes =
        Evil_RiggsCommonCardTypes
            .Concat(Evil_RiggsCommonCardTypes)
            .Concat(Evil_RiggsUncommonCardTypes)
            .Concat(Evil_RiggsRareCardTypes)
            .Concat(Evil_RiggsSpecialCardTypes);

    //Register artifacts
    private static List<Type> Evil_RiggsCommonArtifacts =
    [
        typeof(SpiltBoba),
        typeof(TemperedRage),
        typeof(DreadnaughtShielding)
    ];
    private static List<Type> Evil_RiggsBossArtifacts = 
    [
        typeof(HoldThatThought),
        typeof(SwarmPreloader)
    ];
    private static IEnumerable<Type> Evil_RiggsArtifactTypes =
        Evil_RiggsCommonArtifacts
            .Concat(Evil_RiggsBossArtifacts);

    private static IEnumerable<Type> AllRegisterableTypes =
        Evil_RiggsCardTypes
            .Concat(Evil_RiggsArtifactTypes);

    //Register sprite variables
    internal ISpriteEntry LightMissileMidrowIcon { get; }
    internal ISpriteEntry LightMissileActionIcon { get; }

    //internal ISpriteEntry MissileTurnActionIcon { get; }

    //Register Sequential trait and icon
    internal ICardTraitEntry SequentialTrait;
    internal ISpriteEntry SequentialIcon;


    //Setup modentry
    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {

        Instance = this;
        Harmony = new Harmony("Vintage.Evil_Riggs");
        
        //Register Kokoro
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!.V2;

        //Register localization
        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );

        //Define all custom sprites
        LightMissileMidrowIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/missile_mini.png"));
        LightMissileActionIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/midrow/icon_missile_light.png"));
        SequentialIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/CardTrait/icon_sequential.png"));
        AMissileTurn.AMissileTurnSpr = RegisterSprite(package, "assets/Action/evilRiggs_status_missileTurn.png").Sprite;
        ATargetLock.ATargetLockSpr = RegisterSprite(package, "assets/Action/evilRiggs_status_targetLock.png").Sprite;
        ADiscountFirstCards.ADiscountFirstCardSpr = RegisterSprite(package, "assets/Action/evilRiggs_status_discount.png").Sprite;


        //Define deck
        Evil_RiggsDeck = helper.Content.Decks.RegisterDeck("Evil_Riggs", new DeckConfiguration
        {
            Definition = new DeckDef
            {
                color = new Color("ff9c5f"),
                titleColor = new Color("000000")
            },
            DefaultCardArt = StableSpr.cards_colorless,
            BorderSprite = RegisterSprite(package, "assets/Card/evilRiggs_cardframe.png").Sprite,
            Name = AnyLocalizations.Bind(["character", "name"]).Localize
        });

        //Define sequential trait
        SequentialTrait = helper.Content.Cards.RegisterTrait("Symbiotic", new()
        {
            Name = this.AnyLocalizations.Bind(["trait", "Sequential", "name"]).Localize,
            Icon = (state, card) => SequentialIcon.Sprite,
            Tooltips = (state, card) => [
                new GlossaryTooltip($"trait.{Instance.Package.Manifest.UniqueName}::Sequential")
                {
                    Icon = SequentialIcon.Sprite,
                    TitleColor = Colors.cardtrait,
                    Title = Localizations.Localize(["trait", "Sequential", "name"]),
                    Description = Localizations.Localize(["trait", "Sequential", "desc"])
                },
            ]
        });

        //Define statuses
        EvilRiggsRage = helper.Content.Statuses.RegisterStatus("EvilRiggsRageStatus", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = false,
                color = new Color("ff9c5f"),
                icon = RegisterSprite(package, "assets/Status/evilRiggs_status_rage.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "EvilRiggsRageStatus", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "EvilRiggsRageStatus", "desc"]).Localize
        });
        _ = new EvilRiggsRageManager();
        EvilRiggsDiscountNextTurn = helper.Content.Statuses.RegisterStatus("EvilRiggsDiscountNextTurn", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = false,
                color = new Color("00478A"),
                icon = RegisterSprite(package, "assets/Status/evilRiggs_status_discount.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "EvilRiggsDiscountNextTurn", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "EvilRiggsDiscountNextTurn", "desc"]).Localize
        });
        _ = new EvilRiggsDiscountNextTurnManager();

        EvilRiggsRocketFactory = helper.Content.Statuses.RegisterStatus("EvilRiggsRocketFactory", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = false,
                color = new Color("AEAEAE"),
                icon = RegisterSprite(package, "assets/Status/evilRiggs_status_rocketFactory.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "EvilRiggsRocketFactory", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "EvilRiggsRocketFactory", "desc"]).Localize
        });
        _ = new EvilRiggsRocketFactoryManager();

        EvilRiggsBarrelRoll = helper.Content.Statuses.RegisterStatus("EvilRiggsBarrelRoll", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = false,
                color = new Color("AEAEAE"),
                icon = RegisterSprite(package, "assets/Status/evilRiggs_status_barrelRoll.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "EvilRiggsBarrelRoll", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "EvilRiggsBarrelRoll", "desc"]).Localize
        });
        _ = new EvilRiggsBarrelRollManager();

        EvilRiggsEngineRedirect = helper.Content.Statuses.RegisterStatus("EvilRiggsEngineRedirect", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = false,
                color = new Color("3FBFFF"),
                icon = RegisterSprite(package, "assets/Status/evilRiggs_status_engineRedirect.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "EvilRiggsEngineRedirect", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "EvilRiggsEngineRedirect", "desc"]).Localize
        });
        _ = new EvilRiggsEngineRedirectManager();

        EvilRiggsEngineRedirectCounter = helper.Content.Statuses.RegisterStatus("EvilRiggsEngineRedirectCounter", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = false,
                affectedByTimestop = false,
                color = new Color("A0A0A0"),
                icon = RegisterSprite(package, "assets/Status/evilRiggs_status_engineRedirectUsed.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "EvilRiggsEngineRedirectCounter", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "EvilRiggsEngineRedirectCounter", "desc"]).Localize
        });
        _ = new EvilRiggsEngineRedirectCounterManager();


        //Define a small patch for engine redirect
        var engine_redirect_patch_target = typeof(AMove).GetMethod("Begin", AccessTools.all);
        var engine_redirect_patch_insert = typeof(ModEntry).GetMethod("EvilRiggsEngineRedirectPatch", AccessTools.all);
        Harmony.Patch(engine_redirect_patch_target, postfix: new HarmonyMethod(engine_redirect_patch_insert));

        //Invoke all lists packaged with the helper
        foreach (var type in AllRegisterableTypes)
            AccessTools.DeclaredMethod(type, nameof(IRegisterable.Register))?.Invoke(null, [package, helper]);
        
        //Register animations
        RegisterAnimation(package, "neutral", "assets/Animation/Neutral/evilRiggsNeutral", 5);
        RegisterAnimation(package, "squint", "assets/Animation/Squint/evilRiggsSquint", 5);
        Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = Evil_RiggsDeck.Deck.Key(),
            LoopTag = "gameover",
            Frames = [
                RegisterSprite(package, "assets/Animation/GameOver/evilRiggsGameOver.png").Sprite,
            ]
        });
        Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = Evil_RiggsDeck.Deck.Key(),
            LoopTag = "mini",
            Frames = [
                RegisterSprite(package, "assets/evilRiggsMini.png").Sprite,
            ]
        });
        helper.Content.Characters.V2.RegisterPlayableCharacter("Evil_Riggs", new PlayableCharacterConfigurationV2
        {
            Deck = Evil_RiggsDeck.Deck,
            BorderSprite = RegisterSprite(package, "assets/evilRiggs_panel.png").Sprite,
            Starters = new StarterDeck
            {
                cards = 
                [
                    new LightMissileCard(),
                    new Skedaddle(),
                    new EngineRedirect()
                ],
            },
            Description = AnyLocalizations.Bind(["character", "desc"]).Localize
        });
    }


    //Setup patch for Engine Redirect
    private static void EvilRiggsEngineRedirectPatch(G g, State s, Combat c, ref AMove __instance)
    {
        Status statusRedirect = ModEntry.Instance.EvilRiggsEngineRedirect.Status;
        Status statusRedirectUsed = ModEntry.Instance.EvilRiggsEngineRedirectCounter.Status;
        Ship ship = (__instance.targetPlayer ? s.ship : c.otherShip);
        if (ship.Get(statusRedirect) > ship.Get(statusRedirectUsed))
        {
            c.QueueImmediate(new ADrawCard { count = 1 });
            c.QueueImmediate(new AStatus { targetPlayer = true, status = statusRedirectUsed, statusAmount = 1 });
        }
    }

    //Setup method for easy sprite registration
    public static ISpriteEntry RegisterSprite(IPluginPackage<IModManifest> package, string dir)
    {
        return Instance.Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile(dir));
    }

    //Setup method for easy animation registration
    public static void RegisterAnimation(IPluginPackage<IModManifest> package, string tag, string dir, int frames)
    {
        Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = Instance.Evil_RiggsDeck.Deck.Key(),
            LoopTag = tag,
            Frames = Enumerable.Range(0, frames)
                .Select(i => RegisterSprite(package, dir + i + ".png").Sprite)
                .ToImmutableList()
        });
    }
}

