namespace Abrakam.Data.Runs
{
    /// <summary>
    /// 消耗品
    /// </summary>
    public class consumable: baseCard
    {
        /// <summary>
        /// 物品数量
        /// </summary>
        public int count { get; set; } = 1;
    }
}