using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Abrakam.Data.Runs;

namespace RoguebookSaveEdit
{
    /// <summary>
    /// AddGemsWindows.xaml 的交互逻辑
    /// </summary>
    public partial class AddCardWindows : Window
    {
        private readonly List<CardData> _cardDatas ;
        private baseCard selectdata = null;
        public AddCardWindows(List<CardData> cardDatas)
        {
            //加载数据
            _cardDatas = cardDatas;
            InitializeComponent();
        }


   
        //搜索按钮
        private void search_Click(object sender, RoutedEventArgs e)
        {
            var rainity = ((ComboBoxItem) RarityNames.SelectedValue).Tag?.ToString();


            GamData.ItemsSource = _cardDatas.AsEnumerable()
                    .WhereIf(!string.IsNullOrEmpty(rainity), t => t.rarity == rainity)
                    .WhereIf(!string.IsNullOrEmpty(NameFilter.Text), t => t.name.Contains(NameFilter.Text))
                    .ToList()
                ;

        }

        private void AddGemsWindows_OnLoaded(object sender, RoutedEventArgs e)
        {

            //显示数据
            search_Click(sender, e);
        }

        //显示窗口
        public baseCard ShowGetData()
        {
            this.ShowDialog();
            if (selectdata==null)
            {
                return null;
            }
            return new baseCard
            {
                cardId = selectdata.cardId,
                name = selectdata.name,
                text = selectdata.text,
                rarity = selectdata.rarity,
            };
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            selectdata = (baseCard)GamData.SelectedItem;
            this.DialogResult = true;
        }

    }
}
