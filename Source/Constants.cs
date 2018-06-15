namespace MonoGameCore
{
    public static class Constants
    {
        public const string GameName = "MatchThree";

        public const int ScreenWidth = 480;
        public const int ScreenHeight = 800;

        public const int SymbolsTypesAmount = 10;
        public const int SymbolGfxSize = 32;

        public const int BoardSize = 12;
        public const int BoardLength = 394;
        public const int BoardPosX = (ScreenWidth - BoardLength) / 2;
        public const int BoardPosY = 50;

        public const string HighScoreXml = "myFileName.xml";

        public const string SwipeSound = "sfx/swipeEffect";
        public const string SwipeBackSound = "sfx/swipeBackEffect";
        public const string MatchingSound = "sfx/matchingEffect";
        public const string IngameBg = "gfx/bg";
        public const string MenuLogo = "gfx/logo";
        public static readonly string[] SymbolsPath = {
            "gfx/symbol_0",
            "gfx/symbol_1",
            "gfx/symbol_2",
            "gfx/symbol_3",
            "gfx/symbol_4",
            "gfx/symbol_5",
            "gfx/symbol_6",
            "gfx/symbol_7",
            "gfx/symbol_8",
            "gfx/symbol_9",
            "gfx/symbol_10",
            "gfx/symbol_11",
            "gfx/symbol_12",
            "gfx/symbol_13",
            "gfx/symbol_14",
            "gfx/symbol_15"
        };
    }
}