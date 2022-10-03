using System.Collections.Generic;
using Newtonsoft.Json;

namespace Abrakam.Data.Runs
{
    /// <summary>
    /// 英雄数据
    /// </summary>
    public class heroe : NotifierBase
    {
        private int _order;
        private int _id;
        private int _maxLife;
        private int _currentLife;
        private int _rage;
        private int _bonusPower;
        private List<baseCard> _treasure=new();
        private List<baseCard> _talents =new();
        private List<baseCard> _generatedTalents =new();

        public int id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        
        } //CardId

        /// <summary>
        /// 名称
        /// </summary>
        [JsonIgnore]
        public string name
        {
            get
            {
                return id switch
                {
                    6001 => "索洛科，第一副官",
                    6002 => "奥罗拉，神话撰师",
                    6000 => "莎拉，屠龙少女",
                    6003 => "塞弗，血色暴君",
                    6004 => "福咕噜，不可思议的商人",
                    _ => "其他英雄：" + id
                };
            }
        }
        [JsonIgnore]
        public string En_name
        {
            get
            {
                return id switch
                {
                    6000 => "SHARRA",
                    6001 => "SOROCCO",
                    6002 => "AURORA",
                    6003 => "SEIFER",
                    6004 => "FUGORO",
                    _ => "其他英雄：" + id
                };
            }
        }


        /// <summary>
        /// 位置
        /// </summary>
        public int order
        {
            get => _order;
            set { _order = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 最大生命值
        /// </summary>
        public int maxLife
        {
            get => _maxLife;
            set { _maxLife = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 当前生命值
        /// </summary>
        public int currentLife
        {
            get => _currentLife;
            set { _currentLife = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 愤怒值
        /// </summary>
        public int rage
        {
            get => _rage;
            set { _rage = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 额外力量
        /// </summary>
        public int bonusPower
        {
            get => _bonusPower;
            set { _bonusPower = value; OnPropertyChanged(); }
        }

        // public int forcedSkin { get; set; } //强制换肤 皮肤
        /// <summary>
        /// 宝物列表
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<baseCard> treasure
        {
            get => _treasure;
            set { _treasure = value;  OnPropertyChanged(); }
        }

        /// <summary>
        /// 天赋
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<baseCard> talents
        {
            get => _talents;
            set
            {
                _talents = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 生成的天赋
        /// </summary>

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<baseCard> generatedTalents
        {
            get => _generatedTalents;
            set { _generatedTalents = value; OnPropertyChanged(); }
        }
    }
}