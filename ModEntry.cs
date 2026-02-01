using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Evil_Riggs.External;

namespace Evil_Riggs;

internal class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal Harmony Harmony;
    internal IKokoroApi.IV2 KokoroApi;
    internal IDeckEntry Evil_RiggsDeck;
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }

    // Register cards
    private static List<Type> Evil_RiggsCommonCardTypes =
    [
        
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

        //Register deck
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
                ],
            },
            Description = AnyLocalizations.Bind(["character", "desc"]).Localize
        });

        
        /*KnowledgeStatus = helper.Content.Statuses.RegisterStatus("Knowledge", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = false,
                color = new Color("fbb954"),
                icon = RegisterSprite(package, "assets/knowledge.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "knowledge", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "knowledge", "desc"]).Localize
        });
        LessonStatus = helper.Content.Statuses.RegisterStatus("Lesson", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = false,
                color = new Color("c7dcd0"),
                icon = RegisterSprite(package, "assets/lesson.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "lesson", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "lesson", "desc"]).Localize
        });

        /*
         * Managers are typically made to register themselves when constructed.
         * _ = makes the compiler not complain about the fact that you are constructing something for seemingly no reason.
         
        _ = new KnowledgeManager(package, helper);
        _ = new SilentStatusManager();*/

    }

    /*
     * assets must also be registered before they may be used.
     * Unlike cards and artifacts, however, they are very simple to register, and often do not need to be referenced in more than one place.
     * This utility method exists to easily register a sprite, but nothing prevents you from calling the method used yourself.
     */
    public static ISpriteEntry RegisterSprite(IPluginPackage<IModManifest> package, string dir)
    {
        return Instance.Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile(dir));
    }

    /*
     * Animation frames are typically named very similarly, only differing by the number of the frame itself.
     * This utility method exists to easily register an animation.
     * It expects the animation to start at frame 0, up to frames - 1.
     * TODO It is advised to avoid animations consisting of 2 or 3 frames.
     */
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

