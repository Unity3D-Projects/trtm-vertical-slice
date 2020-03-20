using UnityEngine;

public class Const
{
    public const string SavePath = @"G:\GitHub\The Road to Moscow\saves\save.xml";

    public enum LogEvent
    {
        LogPhrase,
        LogButtonGroup,
        LogButton,
        LogButtonPressed,
        LogEndGameWin,
        LogEndGameLose
    }

    public class XmlAliases
    {
        public const string Phrase = "phrase";
        public const string ButtonGroup = "buttonGroup";
        public const string Button = "button";
        public const string EndGame = "endGame";
        public const string ButtonPressedAttributte = "pressed";
        public const string EndGameWinAttributte = "win";
        public const string ExecuteId = "tname";
        public const string ExecuteTime = "at";
        public const string StartTime = "started";
    }

    public class NotificationKeys
    {
        public const string DalayReminder = "delay";
    }
}
