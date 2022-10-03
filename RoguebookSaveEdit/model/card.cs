using Newtonsoft.Json;

namespace Abrakam.Data.Runs
{
    /// <summary>
    /// 卡牌数据
    /// </summary>
    public class card: baseCard
    {

        /// <summary>
        /// 卡牌孔数
        /// </summary>
        public int socketsCount { get; set; }

        /// <summary>
        /// 镶嵌的宝石
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public baseCard[] gems { get; set; } = { };

    }
}