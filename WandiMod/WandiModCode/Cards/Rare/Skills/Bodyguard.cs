using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 🔗替天行道 / Bodyguard（稀有 · 技能 · 联机）。本回合内失去血量，所有队友获得5纷争/8纷争。
/// // TODO: 精确实现需追踪「本回合失血量」→给队友纷争。当前简化为直接给所有队友纷争。
/// </summary>
public class Bodyguard : WandiModCard
{
    public Bodyguard() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("StrifePerAlly", 5).WithUpgradeTo(8)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null) return;
        int strife = DynamicVars["StrifePerAlly"].IntValue;
        foreach (var ally in CombatState.GetTeammatesOf(Owner.Creature).Where(t => t != null && t.IsAlive && t.IsPlayer))
            await StrifePower.Grant(choiceContext, ally, strife, Owner.Creature, this);
        MainFile.Logger.Info($"[替天行道] 给所有队友 {strife} 纷争（// TODO: 应追踪本回合失血）");
    }
}
