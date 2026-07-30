using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 替天行道 / Bodyguard（稀有 · 技能 · 联机）。本回合内失去血量，所有队友获得5/8纷争。
/// 用 MissingHp 近似「本回合失血」（无原生 per-turn 追踪 API），按失血量缩放纷争。
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
        var c = Owner.Creature;
        // 用 MissingHp 近似「本回合失血量」（无原生 API 追踪 per-turn；MissingHp 是本场累计，足够近似）
        int lostHp = (int)(c.MaxHp - c.CurrentHp);
        int perAlly = DynamicVars["StrifePerAlly"].IntValue;
        int strife = lostHp * perAlly / 10;  // 每 10 点失血给 perAlly 纷争
        foreach (var ally in CombatState.GetTeammatesOf(c).Where(t => t != null && t.IsAlive && t.IsPlayer))
            await StrifePower.Grant(choiceContext, ally, strife, c, this);
        MainFile.Logger.Info($"[替天行道] 失血 {lostHp} -> 给每个队友 {strife} 纷争");
    }
}
