using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard（基类，自带 [Pool] 自动入池）
using WandiMod.WandiModCode.Powers;                 // StrifePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 金焰斩 / Gilded Slash（普通 · 攻击）
/// 造成 14 点伤害，获得 3 点【纷争】。升级：18 伤害，4 纷争。
/// —— 攻防一体示例：DamageVar 走 CommonActions.CardAttack；纷争走 StrifePower.Grant。
/// </summary>
public class GildedSlash : WandiModCard
{
    public GildedSlash() : base(
        cost: 2,
        type: CardType.Attack,
        rarity: CardRarity.Common,
        target: TargetType.AnyEnemy)
    {
    }

    // 基础数值（真相源）。WithUpgrade 设定升级后的数值，逻辑代码无需改动。
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(14, ValueProp.Move).WithUpgrade(18),
        new IntVar("Strife", 3).WithUpgrade(4),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // ① 造成伤害：自动读 DamageVar，按 TargetType=AnyEnemy 选定目标，附带攻击 VFX。
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);

        // ② 获得纷争：读 Strife IntVar，抬临时上限 + 等量回血（见 StrifePower.Grant）。
        // Owner.Creature 战斗外 / 异常时序下可能为 null，记错误日志并跳过，避免空引用炸战斗
        if (Owner.Creature == null)
        {
            MainFile.Logger.Error("[金焰斩] OnPlay 时 Owner.Creature 为空（不在战斗中？），纷争未授予");
            return;
        }
        await StrifePower.Grant(choiceContext, Owner.Creature, DynamicVars["Strife"].IntValue, Owner.Creature, this);
    }
}
