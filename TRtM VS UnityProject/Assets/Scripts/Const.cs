using UnityEngine;

public class Const
{
    public const string SavePath = @"G:\GitHub\The Road to Moscow\saves\save.xml";

    public enum LogEvent
    {
        LogPhrase,
        LogButtonGroup,
        LogButton,
        LogButtonPressed
    }

    public class XmlAliases
    {
        public const string Phrase = "phrase";
        public const string ButtonGroup = "buttonGroup";
        public const string Button = "button";
        public const string ButtonPressedAttributte = "pressed";

        public const string ExecuteId = "tname";
        public const string ExecuteTime = "at";

    }
}
