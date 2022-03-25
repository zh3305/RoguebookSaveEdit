using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using Newtonsoft.Json;

namespace Abrakam.Data.Runs
{
    public class NotifierBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            propertyName ??= new StackTrace().GetFrame(1)?.GetMethod()?.Name.Substring(4);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class run : NotifierBase //, ISingletonDependency
    {
        private string _id=null;
        private int _gold;
        private int _brushes;
        private int _brushRadius;
        private int _battlesWonCount;
        private string _gameVersion;
        private heroe[] _heroes = { };
        private List<card> _cards = new();
        private List<baseCard> _gems = new();
        private consumable[] _consumables = { };
        private consumable _activePigment ;
        private List<baseCard> _treasures = new();

        [JsonIgnore]
        public Visibility IsLoad
        {
            get
            {
                return string.IsNullOrEmpty(id) ?  Visibility.Hidden: Visibility.Visible ;
            }
        }
        // public bool IsLoad => !string.IsNullOrEmpty(id);

        /// <summary>
        /// 金币
        /// </summary>
        public int gold
        {
            get => _gold;
            set { _gold = value; OnPropertyChanged(); }

        }

        /// <summary>
        /// 游戏ID
        /// </summary>
        public string id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsLoad)); }
        }

        /// <summary>
        /// 笔刷数量
        /// </summary>
        public int brushes
        {
            get => _brushes;
            set { _brushes = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 笔刷宽度
        /// </summary>
        public int brushRadius
        {
            get => _brushRadius;
            set { _brushRadius = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 英雄数据
        /// </summary>
        public heroe[] heroes
        {
            get => _heroes;
            set { _heroes = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 卡片数据
        /// </summary>
        public List<card> cards
        {
            get => _cards;
            set { _cards = value;  OnPropertyChanged(); }
        }

        /// <summary>
        /// 宝石列表
        /// </summary>
        public List<baseCard>  gems
        {
            get => _gems;
            set { _gems = value; ; OnPropertyChanged(); }
        }
        /// <summary>
        /// 公用宝物
        /// </summary>
     public List<baseCard >treasures
        {
            get => _treasures;
            set { _treasures = value; ; OnPropertyChanged(); }
        }
        /// <summary>
        /// 消耗品
        /// </summary>
        public consumable[] consumables
        {
            get => _consumables;
            set { _consumables = value; ; OnPropertyChanged(); }
        }
        [JsonIgnore]
        public consumable[] consumablesNoNull
        {
            get => _consumables.Where(t=>t!=null).ToArray();
            set { _consumables = value; ; OnPropertyChanged(); }
        }

        /// <summary>
        /// 活性颜料
        /// </summary>
        public consumable activePigment
        {
            get => _activePigment;
            set { _activePigment = value; OnPropertyChanged(); }
        }

        // public int brushRadius { get; set; }

        /// <summary>
        /// 战斗胜利计数
        /// </summary>
        public int battlesWonCount
        {
            get => _battlesWonCount;
            set { _battlesWonCount = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 游戏版本
        /// </summary>
        public string gameVersion
        {
            get => _gameVersion;
            set { _gameVersion = value; OnPropertyChanged(); }
        }
    }
    //消耗品
    public class consumable: baseCard
    {
        /// <summary>
        /// 物品数量
        /// </summary>
        public int count { get; set; } = 1;
    }
    //卡牌数据
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
    //英雄数据
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
                    6001 => "索洛科",
                    6002 => "奥罗拉",
                    6000 => "Sharra, Dragonslayer",
                    6003 => "Seifer, Blood Tyrant",
                    6004 => "FUGORO",
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
    //数据基类 
    public class baseCard
    {
        public baseCard()
        {
        }


        public int cardId { get; set; }
        [JsonIgnore]
        public string  name { get; set; }
        [JsonIgnore]
        public string text { get; set; }
        [JsonIgnore]
        public string rarity { get; set; }

        [JsonIgnore]
        public string rarityName
        {
            get
            {
                return rarity switch
                {
                    "COMMON" => "普通",
                    "RARE" => "稀有",
                    "EPIC" => "史诗",
                    "LEGENDARY" => "传奇",
                    _ => ""
                };
            }
        
        }
        [JsonIgnore]
        public string heroType { get; set; }
    }

    public enum EnvironmentType
    {
        // Token: 0x04000098 RID: 152
        NONE,
        // Token: 0x04000099 RID: 153
        OVERWORLD,
        // Token: 0x0400009A RID: 154
        GEM_MINE
    }


}

namespace Abrakam.Data.Heroes
{
    // Token: 0x0200016B RID: 363
    public enum HeroType
    {
        // Token: 0x0400060C RID: 1548
        NONE = -1,
        // Token: 0x0400060D RID: 1549
        NEUTRAL,
        // Token: 0x0400060E RID: 1550
        ASSIGNABLE,
        // Token: 0x0400060F RID: 1551
        AURORA,
        // Token: 0x04000610 RID: 1552
        SHARRA,
        // Token: 0x04000611 RID: 1553
        SEIFER,
        // Token: 0x04000612 RID: 1554
        SOROCCO,
        // Token: 0x04000613 RID: 1555
        ENEMY
    }
}