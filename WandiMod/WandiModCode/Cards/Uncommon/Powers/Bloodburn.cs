using MegaCrit.Sts2.Core.Commands;                 // PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using WandiMod.WandiModCode.Powers;                 // BloodburnPower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 焚血 / Bloodburn（罕见 · 能力）
/// 因自身卡牌失血时，回复相同的血量，每回合一次。升级：费用 1→0。
/// —— 自伤引擎的反哺件：自伤产血仇的同时把血补回来，每回合白嫖一次血仇层数。
/// </summary>
public class Bloodburn : WandiModCard
{
    public Bloodburn() : base(
        cost: 1,
        type: CardType.Power,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    // 升级：降费 1→0（参考原生 Zap/WhiteNoise/WellLaidPlans 的 EnergyCost.UpgradeBy(-1)）
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature == null)
        {
            MainFile.Logger.Error("[焚血] OnPlay 时 Owner.Creature 为空，能力未授予");
            return;
        }
        // 授予焚血 Power（形态类，amount=1 仅用于建立实例；具体效果见 BloodburnPower）
        await PowerCmd.Apply<BloodburnPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        MainFile.Logger.Info("[焚血] 授予焚血能力");
    }
}
