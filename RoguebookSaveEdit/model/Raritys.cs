using System.ComponentModel;

namespace RoguebookSaveEdit.model
{
    public enum Raritys:int
    {
        //未知
        [Description("未知")]
        Unknown =0,
        [Description("普通")]
        COMMON=1,
        [Description("稀有")]
        RARE,
        [Description("传奇")]
        LEGENDARY,
        [Description("史诗")]
        EPIC,
    }
}