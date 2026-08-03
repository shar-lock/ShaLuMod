using MegaCrit.Sts2.Core.Commands;                 // PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using WandiMod.WandiModCode.Powers;                 // BerserkerHeartPower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 狂战之心 / Berserker Heart（罕见 · 能力）
/// 每打出一张攻击：获得 1 层【血仇】。升级：费用 2→1。
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

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature == null)
        {
            MainFile.Logger.Error("[狂战之心] OnPlay 时 Owner.Creature 为空，能力未授予");
            return;
        }
        await PowerCmd.Apply<BerserkerHeartPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        MainFile.Logger.Info("[狂战之心] 授予：每打攻击 +1 血仇");
    }
}
