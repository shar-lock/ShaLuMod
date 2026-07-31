using MegaCrit.Sts2.Core.Commands;                  // PowerCmd
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardMultiplayerConstraint / CardKeyword
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo
using WandiMod.WandiModCode.Powers;                 // BodyguardPower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 🔗 浴血带冠 / Bodyguard（稀有 · 技能 · 联机）。
/// 本回合内，每当你受到攻击，所有其他队友获得 5 层【纷争】。升级：8 层。
/// —— 联机承伤件：授予单回合能力（BodyguardPower），挨打越多队友纷争（临时上限）越多。
/// </summary>
public class Bodyguard : WandiModCard
{
    public Bodyguard() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("StrifePerHit", 5).WithUpgradeTo(8)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 授予单回合 Power（Amount=每次受击给队友的纷争 5/8；敌方回合末自毁，见 BodyguardPower）
        await PowerCmd.Apply<BodyguardPower>(choiceContext, Owner.Creature, DynamicVars["StrifePerHit"].IntValue, Owner.Creature, this);
        MainFile.Logger.Info($"[浴血带冠] 授予单回合能力：受击时给其他队友 {DynamicVars["StrifePerHit"].IntValue} 纷争");
    }
}
