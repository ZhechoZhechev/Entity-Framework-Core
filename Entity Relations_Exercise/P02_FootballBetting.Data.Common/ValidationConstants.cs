namespace P02_FootballBetting.Data.Common
{
    public static class ValidationConstants
    {
        //team
        public const int NameMaxLength = 25;
        public const int LogoMaxLength = 2048;
        public const int InitialsMaxLength = 5;

        //color
        public const int ColorNameMaxLength = 15;

        //town
        public const int TownNameMaxLength = 60;

        //country
        public const int CountryNameMaxLength = 60;

        //player
        public const int PlayerNameMaxLength = 100;

        //game
        public const int GameResultMaxLength = 7;

        //user
        public const int UserUserNameMaxLength = 50;
        public const int UserPasswordMaxLength = 256;
        public const int UserEmailMaxLength = 256;
        public const int UserNameMaxLength = 100;

    }
}
