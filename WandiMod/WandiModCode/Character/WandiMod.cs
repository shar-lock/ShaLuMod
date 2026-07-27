using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using WandiMod.WandiModCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace WandiMod.WandiModCode.Character;

public class WandiMod : PlaceholderCharacterModel
{
    public const string CharacterId = "WandiMod";

    // 万敌主色调：虚数金（Imaginary / 虚数属性）
    public static readonly Color Color = new("D4AF37");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Male;
    public override int StartingHp => 75;

    // TODO(M3): 替换为万敌专属起手牌（5×横扫突击 + 4×御阵 + 1×血祭）
    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendIronclad>()
    ];

    // TODO(M2): 替换为「弑亲血脉」遗物（4 次免死）
    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<BurningBlood>()
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
}