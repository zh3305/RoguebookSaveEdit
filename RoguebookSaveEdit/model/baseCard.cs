using System;
using System.Linq;
using Newtonsoft.Json;
using RoguebookSaveEdit;

namespace Abrakam.Data.Runs
{
    /// <summary>
    /// 数据基类
    /// </summary>
    public class baseCard
    {
        private string _name;
        private string _text;

        public baseCard()
        {
        }


        public int cardId { get; set; }

        public string name
        {
            internal get
            {
                return Const.Localizations?.FirstOrDefault(t => t.id == $"{cardId:D5}_CARD_NAME").translation ?? _name;
            }
            set { _name = value; }
        }

        public string text
        {
            internal get
            {
                return Const.Localizations?.FirstOrDefault(t => t.id == $"{cardId:D5}_CARD_TEXT").translation
                    .Format(placeholders =>
                      {
                          var strings = placeholders
                              .Replace("dynamic_number|", "")
                              .Split("|");
                          switch (strings.Length)
                          {
                              case 1:
                                  switch (strings[0])
                                  {
                                      case "reap":
                                          return "收获";
                                      case "life":
                                          return "生命";
                                      case "protection":
                                          return "保护";
                                      case "power":
                                          return "力量";
                                      // "dynamic_number" => "",
                                      case "ally":
                                          return "盟友";  
                                      case "retain":
                                          return "保留";
                                      case "ranged":
                                          return "远程";
                                      case "melee":
                                          return "近战";
                                      case "courage":
                                          return "勇气";
                                      case "criticalhit":
                                          return "暴击";
                                      case "spiritless":
                                          return "无精打采";
                                      case "block":
                                      case "blocknumberless":
                                          return "格挡";
                                      case "heroic":
                                          return "冲锋";
                                      case "combo":
                                          return "连携技";
                                      case "exhaust":
                                          return "解除";
                                      case "activate":
                                          return "激活";
                                      case "rage":
                                          return "愤怒";
                                      case "spirit":
                                          return "精神力";
                                      case "weak":
                                          return "虚弱";
                                      case "bleednumberless":
                                          return "流血";
                                      case "flash":
                                          return "一闪";
                                      case "swap":
                                          return "交换";
                                      default:
                                          Console.WriteLine(strings[0]);
                                          return strings[0];
                                  }
                                  break;
                              case 2:
                              {
                                  string prefix ;
                                      switch (strings[0])
                                      {
                                          case "life":
                                              prefix= "生命";
                                              break;
                                          case "power":
                                              prefix = "力量";
                                              break;
                                          case "block":
                                              prefix = "格挡";
                                              break;
                                          case "faeria":
                                              prefix = "精力";
                                              break;
                                          case "bleed":
                                              prefix = "流血";
                                              break;
                                          case "refer":
                                              return "["+( Const.Localizations?.FirstOrDefault(t =>
                                                  t.id == $"{int.Parse(strings[1]):D5}_CARD_NAME").translation ?? ("引用牌:"+ strings[1]))+"]";
                                           
                                          case "deckSize"://对战时计算的状态值
                                          case "soroccoPower":
                                          case "reviveCounter":
                                          case "drawPile":
                                          case "storm":
                                              prefix = "";
                                              break;

                                          default:
                                              Console.WriteLine("prefix :"+ strings[0]);
                                              return strings[0]+"|" + strings[1];
                                      }
                                      return strings[1] + prefix;
                                  }
                                  break;
                          }

                          Console.WriteLine("lost:"+placeholders);
                          return placeholders;
                      })
                      ?? _text;
            }
            set { _text = value; }
        }

        public string rarity { internal get; set; }

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
        public string heroType { internal get; set; }
    }
}