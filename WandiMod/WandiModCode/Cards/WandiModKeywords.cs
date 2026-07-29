using BaseLib.Patches.Content;                      // [CustomEnum] / [KeywordProperties]
using MegaCrit.Sts2.Core.Entities.Cards;            // CardKeyword

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 万敌 Mod 自定义卡牌关键词。注册方式参考 BaseLib 自带的 BaseLibKeywords（Purge）：
/// 静态字段 + [CustomEnum]，字段名决定本地化键 —— Strife → WANDIMOD-STRIFE（card_keywords.json）。
///
/// 高亮方式：AutoKeywordPosition.None —— 不在卡牌下方追加独立的关键词行（避免与正文里的
/// 「纷争/血仇」重复）。改为在卡牌本地化描述里用 [gold]纷争[/gold] 富文本标签内联高亮；
/// tooltip 由 CardModel.HoverTips 自动提供（卡牌只要带 CanonicalKeywords，hover 卡牌即弹词条说明）。
///
/// 排查提示：注册在 Mod 加载时由 BaseLib 的 [CustomEnum] 补丁完成，若注册失败会在启动日志
/// 报枚举/本地化相关错误；词条不显示时先检查 card_keywords.json 的键名是否为 WANDIMOD-STRIFE.*。
/// </summary>
public class WandiModKeywords
{
    /// <summary>纷争：万敌的临时生命上限（tooltip 自动附在卡牌描述后）。</summary>
    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Strife;

    /// <summary>血仇：万敌的失血计数器（tooltip 自动附在卡牌描述后）。</summary>
    [CustomEnum]
    [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Vengeance;
}
