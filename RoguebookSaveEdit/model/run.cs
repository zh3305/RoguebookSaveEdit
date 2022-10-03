using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;

namespace Abrakam.Data.Runs
{
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
        private consumable[] _consumables = { null,null,null};
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
            set { _consumables = value;  OnPropertyChanged(); OnPropertyChanged(nameof(consumablesNoNull)); }
        }
        [JsonIgnore]
        public consumable[] consumablesNoNull
        {
            get => _consumables.Where(t=>t!=null).ToArray();
            // set { _consumables = value;  OnPropertyChanged(); }
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
}