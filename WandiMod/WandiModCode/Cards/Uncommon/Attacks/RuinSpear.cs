using BaseLib.Extensions;                           // WithUpgrade / WithValueProp
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd / PlayerCmd（获得能量）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using System.Linq;                                  // SelectMany / Any（击杀结果判定）
using WandiMod.WandiModCode.Character;              // WandiModCard

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 破灭之枪 / Ruin Spear（罕见 · 攻击）
/// 造成 16 点伤害；若击杀目标，获得 3 点能量。升级：22 伤害。
/// —— 击杀回能件：参考原版 Feed / HandOfGreed 的「击杀奖励」范式——
///    用 DamageCmd 建造器拿到 AttackCommand.Results，判 WasTargetKilled 决定是否回能。
///    （Creature.IsDead 也能粗判目标是否死亡，但 WasTargetKilled 能过滤掉死亡被触发器阻止的情况，更贴近原版。）
/// </summary>
public class RuinSpear : WandiModCard
{
    public RuinSpear() : base(
        cost: 2,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(16, ValueProp.Move).WithUpgrade(22),
        new IntVar("Energy", 3),                    // 击杀时回能（升级不变）
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null)
        {
            MainFile.Logger.Error("[破灭之枪] OnPlay 时目标为空，效果未触发");
            return;
        }

        // 用 DamageCmd 建造器结算，拿到结果以判定击杀（与原版 Feed 的写法一致）
        var attack = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithValueProp(ValueProp.Move)
            .Execute(choiceContext);

        // 击杀判定：任一 DamageResult.WasTargetKilled（Results 形如 List&lt;List&lt;DamageResult&gt;&gt;，需 SelectMany 拍平）
        bool killed = attack.Results
            .SelectMany(r => r)
            .Any(r => r.WasTargetKilled);

        if (killed)
        {
            int energy = DynamicVars["Energy"].IntValue;
            await PlayerCmd.GainEnergy(energy, Owner);
            MainFile.Logger.Info($"[破灭之枪] 击杀目标，获得 {energy} 能量");
        }
    }
}
