using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.Commands;                 // CardPileCmd（抽牌）/ CreatureCmd（自伤）
using System.Linq;                                  // Hand.Cards.Count()
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Powers;                 // VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 血契 / Blood Pact（罕见 · 技能 · 消耗）
/// 失去「手牌数」点生命，抽 3 张牌，获得 1 层【血仇】。升级：抽 4 张。
/// —— 0 费过牌 + 喂养血仇：自伤量随手牌数动态结算（手牌越多越痛），但血仇引擎也会因自伤自动 +1。
/// </summary>
public class BloodPact : WandiModCard
{
    public BloodPact() : base(
        cost: 0,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Draw", 3).WithUpgrade(4),
        new IntVar("Vengeance", 1),
    ];

    // 消耗：防囤积循环（0 费过牌件的标准约束）
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance, CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[血契] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }

        // 失去「手牌数」点生命（动态结算：手牌越多越痛）
        // PileType.Hand.GetPile(Owner).Cards.Count——参考原生 Anointed
        int handCount = PileType.Hand.GetPile(Owner).Cards.Count;
        if (handCount > 0)
        {
            // Unblockable|Unpowered|Move：全额计入失血，不走格挡/不吃力量，触发血仇自动 +1
            await CreatureCmd.Damage(choiceContext, creature, handCount,
                ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        }

        await CardPileCmd.Draw(choiceContext, DynamicVars["Draw"].IntValue, Owner);
        await VengeancePower.Grant(choiceContext, creature, DynamicVars["Vengeance"].IntValue, this);

        MainFile.Logger.Info($"[血契] 失 {handCount} 血（手牌数），抽 {DynamicVars["Draw"].IntValue} 牌，获得 {DynamicVars["Vengeance"].IntValue} 血仇");
    }
}
