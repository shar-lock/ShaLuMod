using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;

namespace WandiMod.WandiModCode.Cards;

/// <summary>战争践踏 / War Stomp（稀有 · 技能 · 全体 · 消耗）。施加 2易伤+2虚弱 / 3+3。</summary>
public class WarStomp : WandiModCard
{
    public WarStomp() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies) { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Vulnerable", 2).WithUpgradeTo(4),
        new IntVar("Weak", 2).WithUpgradeTo(4),
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null) return;
        int vuln = DynamicVars["Vulnerable"].IntValue;
        int weak = DynamicVars["Weak"].IntValue;
        foreach (var enemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy, vuln, Owner.Creature, this);
            await PowerCmd.Apply<WeakPower>(choiceContext, enemy, weak, Owner.Creature, this);
        }
        MainFile.Logger.Info($"[战争践踏] 全体 {vuln} 易伤 + {weak} 虚弱");
    }
}
