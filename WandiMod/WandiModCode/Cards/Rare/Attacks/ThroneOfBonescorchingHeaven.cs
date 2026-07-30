using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 诛天焚骨的王座 / Throne of Bonescorching Heaven ⭐（稀有 · 攻击 · 全体）
/// 吞噬所有血仇，每层全体 8 伤；HP≤50% 每层额外+4。升级：10/+5。
/// —— 万敌的终结技：把积攒的血仇一次性转化为全体核弹。致敬星铁终结技。
/// </summary>
public class ThroneOfBonescorchingHeaven : WandiModCard
{
    public ThroneOfBonescorchingHeaven() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) { }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("DmgPerStack", 8).WithUpgradeTo(10),
        new IntVar("BonusPerStack", 4).WithUpgradeTo(5),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || CombatState == null) { MainFile.Logger.Error("[诛天焚骨的王座] Creature/CombatState 为空"); return; }

        int blood = creature.GetPower<VengeancePower>()?.Amount ?? 0;
        if (blood <= 0) { MainFile.Logger.Warn("[诛天焚骨的王座] 无血仇，效果落空"); return; }

        // 吞噬所有血仇（留 1 层防 Power 移除）
        await PowerCmd.Apply<VengeancePower>(choiceContext, creature, -(blood - 1), creature, null);

        // 每层基础伤害 + HP≤50% 额外
        int perStack = DynamicVars["DmgPerStack"].IntValue;
        int bonus = DynamicVars["BonusPerStack"].IntValue;
        decimal dmg = blood * perStack;
        bool lowHp = creature.CurrentHp <= creature.MaxHp * 0.5m;
        if (lowHp) dmg += blood * bonus;

        await DamageCmd.Attack(dmg).FromCard(this, cardPlay).TargetingAllOpponents(CombatState).WithValueProp(ValueProp.Move).Execute(choiceContext);
        MainFile.Logger.Info($"[诛天焚骨的王座] 吞噬 {blood} 血仇，全体 {dmg} 伤（残血加成={lowHp}）");
    }
}
