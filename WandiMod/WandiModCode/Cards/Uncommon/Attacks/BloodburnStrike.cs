using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.Commands;                  // CardPileCmd（抽牌）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // VengeancePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 灼血击 / Bloodburn Strike（罕见 · 攻击）
/// 造成 6 点伤害；若没有【血仇】，抽 1 张牌。升级：抽 2 张牌（伤害不变）。
/// —— 0 费过牌件：奖励「未失血」状态（血仇 = 0）。血仇是失血计数器，简洁且贴主题。
///    与 BloodsipSpear（血仇 &gt; 0 时回血）互补：本卡奖励「干净」状态，鼓励早打。
/// </summary>
public class BloodburnStrike : WandiModCard
{
    public BloodburnStrike() : base(
        cost: 0,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6, ValueProp.Move),          // 伤害（升级不变）
        new IntVar("Draw", 1).WithUpgradeTo(2),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[灼血击] OnPlay 时 Owner.Creature 为空，抽牌判定未执行");
            // 攻击仍可继续（CommonActions.CardAttack 不依赖 creature）
        }

        // ① 造成伤害
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);

        // ② 血仇 = 0 = 本战斗未失血 → 抽牌（奖励「干净」状态）
        // 血仇真实层数保底1层（地板层不显示），因此「无有效血仇」= null 或 Amount<=1
        int blood = creature?.GetPower<VengeancePower>()?.Amount ?? 0;
        if (blood <= 1)
        {
            int draw = DynamicVars["Draw"].IntValue;
            await CardPileCmd.Draw(choiceContext, draw, Owner);
            MainFile.Logger.Info($"[灼血击] 血仇=0，抽 {draw} 张牌");
        }
    }
}
