namespace MonoGameCore
{
    public static class Constants
    {
        public const string GAME_NAME = "MatchThree";

        public const int SCREEN_WIDTH = 480;
        public const int SCREEN_HEIGHT = 800;

        public const int SYMBOLS_TYPES_AMOUNT = 10;
        public const int SYMBOL_GFX_SIZE = 32;

        public const int BOARD_SIZE = 12;
        public const int BOARD_LENGTH = 394;
        public const int BOARD_POS_X = (SCREEN_WIDTH - BOARD_LENGTH) / 2;
        public const int BOARD_POS_Y = 50;

        public const string SWIPE_SOUND = "sfx/swipeEffect";
        public const string SWIPE_BACK_SOUND = "sfx/swipeBackEffect";
        public const string MATCHING_SOUND = "sfx/matchingEffect";
        public const string INGAME_BG = "gfx/bg";
        public static readonly string[] SYMBOLS_PATH = {
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