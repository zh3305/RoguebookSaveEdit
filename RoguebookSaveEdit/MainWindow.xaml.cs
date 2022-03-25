using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Abrakam.Data.Runs;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Path = System.IO.Path;

namespace RoguebookSaveEdit
{


    public class CardData : baseCard
    {
        public string orderId { get; set; }
        public string heroType { get; set; }

        /**
status
event
ally
creature
talent
gem
treasure
perk
positionStatus
hero
consumable
globalStatus
tournamentSeal
         */
        public string type { get; set; }
        public string faeria { get; set; }
        public string scope { get; set; }
        public string price { get; set; }
        public string tags { get; set; }
        // public string name_cn { get; set; }
        // public string text_cn { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //语言文件位于资源文件Cards 中
        public const string GameDataConn = "data source=gamedata.db";


        /// <summary>
        /// 缓存卡片数据列表
        /// </summary>
        public static List<CardData> CardDatas;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //加载卡片数据
            using (IDbConnection cnn = new SqliteConnection(GameDataConn))
            {
                cnn.Open();
                CardDatas = cnn.Query<CardData>($@"
SELECT
	cards.cardId, 
	cards.orderId, 
	cards.heroType, 
	cards.type, 
	cards.faeria, 
	cards.scope, 
	cards.rarity, 
	cards.price, 
	cards.tags, 
	IFNULL(cards.name_cn , name)	`name`, 
	IFNULL(cards.text_cn , text)	 text
FROM
	cards", new DynamicParameters()).ToList();
            }

            //颜料下拉列表
            var activePigmentItems = CardDatas.Where(t => new[] { 8503, 8508 }.Contains(t.cardId))
                .Select(t => new consumable
                {
                    cardId = t.cardId,
                    count = 1,
                    name = t.name,
                    text = t.text

                })
                .ToList();

            activePigment.ItemsSource = activePigmentItems;
            //这里的Name和Value不能乱填哦
            activePigment.DisplayMemberPath = "name";//显示出来的值


        }
        string runjsonfilepath;

        run _runConfig = new run();//初始化避免绑定出错

        private static readonly string ApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("Roaming", "LocalLow");
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //#if DEBUG
            //            var path = Path.Combine(System.Environment.CurrentDirectory, @"");
            //#else
            var path = Path.Combine(ApplicationData, @"Abrakam Entertainment SA\Roguebook\Saves\");

            //#endif

            if (!Directory.Exists(path))
            {
                MessageBox.Show("未找到存档目录,请先运行游戏!");
            }
            if (!string.IsNullOrEmpty(runjsonfilepath))
            {
                if (MessageBox.Show("已加载数据,是否重新加载?", "提示", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
                runjsonfilepath = "";
                _runConfig.id = null;
                LoadInfoTxt.Content = "未载入存档...";
            }

            DirectoryInfo Dir = new DirectoryInfo(path);
            DirectoryInfo[] DirSub = Dir.GetDirectories();
            //TODO 多个存档选择,目前只加载第一个
            foreach (DirectoryInfo f in DirSub)
            {
                string osb_sub = f.FullName;
                osb_sub += $@"\1\run\run.json";
                if (!File.Exists(osb_sub))
                {
                    continue;
                }
                //加载数据
                loadRunConfig(osb_sub);
                break;
            }

            if (string.IsNullOrEmpty(runjsonfilepath))
            {
                MessageBox.Show("未找到运行数据,请先运行游戏!");
                return;
            }
        }
        //载入存档数据
        private void loadRunConfig(string runjisnpath)
        {
            runjsonfilepath = runjisnpath;
            _runConfig = LoadSettings(runjsonfilepath).Get<run>();

            //添加名称
            BandCardName(_runConfig.consumables);//消耗品
            BandCardName(_runConfig.gems);//宝石
            BandCardName(_runConfig.cards.Cast<baseCard>().ToList());//卡牌
            BandCardName(_runConfig.heroes[0].treasure);//宝物
            BandCardName(_runConfig.heroes[1].treasure);//宝物
            BandCardName(_runConfig.treasures);//宝物

            LoadInfoTxt.Content = "已载入存档" + _runConfig.id + "\r\n游戏版本：" + _runConfig.gameVersion;
            MainGrid.DataContext = _runConfig;
        }

        //绑定翻译记录
        private void BandCardName(IList<baseCard> cards)
        {
            foreach (var runConfigConsumable in cards)
            {
                var trdata = CardDatas.FirstOrDefault(t => t.cardId == runConfigConsumable.cardId);
                if (trdata != null)
                {
                    runConfigConsumable.name = trdata.name;

                    runConfigConsumable.text = trdata.text;
                    runConfigConsumable.rarity = trdata.rarity;
                    runConfigConsumable.heroType = trdata.heroType;
                }
            }
        }

        internal IConfiguration LoadSettings(string ConfigPath)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(runjsonfilepath, optional: true, reloadOnChange: true)
                .Build();


            return config;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(runjsonfilepath))
            {
                SaveConfiguration(_runConfig);
                LoadInfoTxt.Content = DateTime.Now.Ticks + "已保存存档" + _runConfig.id + "\r\n游戏版本：" + _runConfig.gameVersion;
            }

        }

        #region 保存配置文件


        public void SaveConfiguration(run configuration)
        {
            SetAppSettingValue(configuration);

        }

        private void SetAppSettingValue(object atype, string appSettingsJsonFilePath = null)
        {

            var fromObject = JToken.FromObject(atype);

            if (appSettingsJsonFilePath == null)
            {
                appSettingsJsonFilePath = runjsonfilepath;
            }

            var json = System.IO.File.ReadAllText(appSettingsJsonFilePath);
            JObject jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);
            // jsonObj.SelectToken(prop.Path).Replace(prop);

            UpdataJobject(fromObject, jsonObj);


            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                Converters =
                {
                    new StringEnumConverter()
                }
            };

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, settings);
            //输出文件
            System.IO.File.WriteAllText(appSettingsJsonFilePath, output);
            //备份原始文件
            System.IO.File.WriteAllText(appSettingsJsonFilePath + ".back", json);
        }

        private static readonly string[] ReplacePropertyNames = new string[] { "treasure", "talents", "generatedTalents", "gems", "cards" };
        //更新json对象
        private static void UpdataJobject(JToken fromObject, JObject jsonObj)
        {
            foreach (var child in fromObject.Children())
            {

                if (!ReplacePropertyNames.Contains(child.Path.Split(".").Last()) && child.Children().Any())
                {
                    UpdataJobject(child, jsonObj);
                }
                else
                {
                    jsonObj.SelectToken(child.Path)?.Replace(fromObject.Root.SelectToken(child.Path));
                }

            }
        }

        #endregion

        //手动载入文件
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择运行存档文件.";
            dialog.Filter = "运行存档(run.json)|run.json";
            if (dialog.ShowDialog() ?? false)
            {
                LoadSettings(dialog.FileName);
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        //添加消耗品
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (_runConfig.consumablesNoNull.Length >= 3)
            {
                MessageBox.Show("消耗品只能存放3个,请删除后在添加!");
                return;
            }



        }
        //添加宝石
        private void Addgems_Click(object sender, RoutedEventArgs e)
        {
            if (_runConfig.gems.Count >= 6)
            {
                MessageBox.Show("宝石只能存放6个,请删除后在添加!");
                return;
            }
            var showGetData = new AddCardWindows(CardDatas.Where(t => t.type == "gem").ToList())
                .ShowGetData();
            if (showGetData != null)
            {
                _runConfig.gems.Add(showGetData);
                CollectionViewSource.GetDefaultView(gems.ItemsSource).Refresh();
            }
        }

        private void AddCard_Click(object sender, RoutedEventArgs e)
        {
            var herostypes = new[] { _runConfig.heroes[1].En_name, _runConfig.heroes[0].En_name };

            var showGetData = new AddCardWindows(CardDatas.Where(t => t.type == "event" &&
                                                                      herostypes
                                                                          .Contains(t.heroType)).ToList())
                .ShowGetData();
            if (showGetData != null)
            {
                _runConfig.cards.Add(new card
                {
                    cardId = showGetData.cardId,
                    name = showGetData.name,
                    text = showGetData.text,
                    rarity = showGetData.rarity,
                    socketsCount = 2,
                    gems = new baseCard[]
                    {
                    }
                });
                CollectionViewSource.GetDefaultView(usercards.ItemsSource).Refresh();
            }

        }

        private void Addtreasure1_Click(object sender, RoutedEventArgs e)
        {
            var herostypes = new[] { "ASSIGNABLE", _runConfig.heroes[0].En_name };
            var showGetData = new AddCardWindows(CardDatas.Where(t => t.type == "treasure" &&
                                                                      herostypes
                                                                          .Contains(t.heroType)).ToList())
                .ShowGetData();
            if (showGetData != null)
            {
                _runConfig.heroes[0].treasure.Add(showGetData);
                CollectionViewSource.GetDefaultView(treasure1.ItemsSource).Refresh();
            }


        }

        private void Addtreasure2_Click(object sender, RoutedEventArgs e)
        {
            var herostypes = new[] { "ASSIGNABLE", _runConfig.heroes[1].En_name };
            var showGetData = new AddCardWindows(CardDatas.Where(t => t.type == "treasure" &&
                                                                      herostypes
                                                                          .Contains(t.heroType)).ToList())
                .ShowGetData();
            if (showGetData != null)
            {
                _runConfig.heroes[1].treasure.Add(showGetData);
                CollectionViewSource.GetDefaultView(treasure2.ItemsSource).Refresh();
            }

        }

        private void Addtreasure_Click(object sender, RoutedEventArgs e)
        {
            var herostypes = new[] { "NEUTRAL"};
            var showGetData = new AddCardWindows(CardDatas.Where(t => t.type == "treasure" &&
                                                                      herostypes
                                                                          .Contains(t.heroType)).ToList())
                .ShowGetData();
            if (showGetData != null)
            {
                _runConfig.treasures.Add(showGetData);
                CollectionViewSource.GetDefaultView(treasure.ItemsSource).Refresh();
            }
        }
    }
}
