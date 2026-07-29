using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                  // PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // StrifePower / VengeancePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 守誓 / Oathguard（普通 · 技能）
/// 获得 5 点【纷争】；消耗 1 层【血仇】额外获得 3 纷争（无血仇则不加）。升级：8 纷争。
/// —— 血仇·纷争联动：基础纷争 + 用血仇换额外纷争。没血仇时只给基础，不白送。
/// </summary>
public class Oathguard : WandiModCard
{
    public Oathguard() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Common,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Strife", 5).WithUpgradeTo(8),
        new IntVar("BloodBonus", 3),   // 消耗 1 血仇换的额外纷争（固定）
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife, WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[守誓] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }

        // ① 基础纷争
        await StrifePower.Grant(choiceContext, creature, DynamicVars["Strife"].IntValue, creature, this);

        // ② 消耗 1 血仇 → 额外纷争（无血仇则跳过）
        var vengeance = creature.GetPower<VengeancePower>();
        if (vengeance != null && vengeance.Amount > 0)
        {
            await PowerCmd.Apply<VengeancePower>(choiceContext, creature, -1, creature, null);
            await StrifePower.Grant(choiceContext, creature, DynamicVars["BloodBonus"].IntValue, creature, this);
            MainFile.Logger.Info("[守誓] 消耗 1 血仇，额外获得纷争");
        }
    }
}
