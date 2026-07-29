using MegaCrit.Sts2.Core.Commands;                 // PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using WandiMod.WandiModCode.Powers;                 // BerserkerHeartPower / VengeancePower（关键词）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 狂战之心 / Berserker Heart（罕见 · 能力）
/// 每打出一张攻击：失 1 血，获得 1 层【血仇】。升级：费用 2→1。
/// —— 攻击流引擎件：每张攻击自伤换血仇，配合多段攻击快速堆叠。
/// 注意：血仇由自伤自动触发（VengeancePower.AfterDamageReceived），BerserkerHeartPower 只负责自伤。
/// </summary>
public class BerserkerHeart : WandiModCard
{
    public BerserkerHeart() : base(
        cost: 2,
        type: CardType.Power,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    // 升级：降费 2→1
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature == null)
        {
            MainFile.Logger.Error("[狂战之心] OnPlay 时 Owner.Creature 为空，能力未授予");
            return;
        }
        // 授予狂战之心 Power（形态类，amount=1 仅用于建立实例；具体效果见 BerserkerHeartPower）
        await PowerCmd.Apply<BerserkerHeartPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        MainFile.Logger.Info("[狂战之心] 授予狂战之心：每打攻击自伤1（自动+1血仇）");
    }
}
