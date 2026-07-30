using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Relics;   // 起手遗物 BloodOfTheKinslayer
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using WandiMod.WandiModCode.Cards;                  // 起手牌 HoldTheLine

namespace WandiMod.WandiModCode.Character;

public class WandiMod : PlaceholderCharacterModel
{
    public const string CharacterId = "WandiMod";

    // 万敌主色调：血红色（卖血 / 血仇主题）。NameColor / 能量描边 / 牌组图标统一用此色。
    public static readonly Color Color = new("B71C1C");

    public override Color NameColor => Color;
    // 能量数字描边色——同步血红色（默认近透明黑 0000000D）
    public override Color EnergyLabelOutlineColor => Color;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 80;

        // 起手牌组（设计定稿）：5×打击 + 3×御敌 + 1×血祭之枪 + 1×残影。
        // 打击显式限定命名空间——嵌套命名空间解析规则下 Cards.Strike 本就指向万敌专属 Strike，
        // 写全消除与原版 MegaCrit...Cards.Strike 的歧义疑虑。
        public override IEnumerable<CardModel> StartingDeck => [
            ModelDb.Card<WandiModCode.Cards.Strike>(),
            ModelDb.Card<WandiModCode.Cards.Strike>(),
            ModelDb.Card<WandiModCode.Cards.Strike>(),
            ModelDb.Card<WandiModCode.Cards.Strike>(),
            ModelDb.Card<WandiModCode.Cards.Strike>(),
            ModelDb.Card<HoldTheLine>(),
            ModelDb.Card<HoldTheLine>(),
            ModelDb.Card<HoldTheLine>(),
            ModelDb.Card<BloodriteStrike>(),
            ModelDb.Card<Cards.Afterimage>()
        ];

    // 起手遗物：弑亲血脉（免死 ×2 + 战斗开始赋予血仇 Power）——角色灵魂，M2 已实装
    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<BloodOfTheKinslayer>()
    ];
    
    public override CardPoolModel CardPool => ModelDb.CardPool<WandiModCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<WandiModRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<WandiModPotionPool>();
    
    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets. 
        These are just some of the simplest assets, given some placeholders to differentiate your character with. 
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    public override string CustomIconTexturePath => "character_icon_wandi.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_wandi.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_wandi_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_wandi.png".CharacterUiPath();

    // 选角界面大立绘：静态场景（Control + TextureRect），替代 PlaceholderCharacterModel 借用的铁甲战士动画背景
    public override string CustomCharacterSelectBg => "res://WandiMod/scenes/char_select_bg_wandimod.tscn";
}