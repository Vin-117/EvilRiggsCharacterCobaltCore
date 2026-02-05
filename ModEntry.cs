using Evil_Riggs.Actions;
using Evil_Riggs.Cards;
using Evil_Riggs.External;
using Evil_Riggs.Features;
using Evil_Riggs.Midrow;
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

    // Register cards
    private static List<Type> Evil_RiggsCommonCardTypes =
    [
        typeof(LightMissileCard),
        typeof(Skedaddle),
        typeof(Jostle),
        typeof(WildShot),
        typeof(Airburst),
        typeof(Bide)
    ];
    private static List<Type> Evil_RiggsUncommonCardTypes =
    [
    ];
    private static List<Type> Evil_RiggsRareCardTypes = 
    [
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
    ];
    private static List<Type> Evil_RiggsBossArtifacts = 
    [
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
                    new Bide()  
                ],
            },
            Description = AnyLocalizations.Bind(["character", "desc"]).Localize
        });
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

