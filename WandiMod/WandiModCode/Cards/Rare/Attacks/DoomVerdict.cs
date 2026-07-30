using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>毁灭裁决 / Doom Verdict（稀有 · 攻击）。32伤；击杀获得4血仇 / 40伤。</summary>
public class DoomVerdict : WandiModCard
{
    public DoomVerdict() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(32, ValueProp.Move).WithUpgradeTo(40),
        new IntVar("BloodOnKill", 4),
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        var cmd = CommonActions.CardAttack(this, cardPlay);
        var results = await cmd.Execute(choiceContext);
        // 击杀判定：WasTargetKilled（参考原生 Feed/HandOfGreed）
        bool killed = results?.SelectMany(r => r).Any(r => r.WasTargetKilled) == true;
        if (killed)
        {
            int blood = DynamicVars["BloodOnKill"].IntValue;
            await VengeancePower.Grant(choiceContext, Owner.Creature, blood, this);
            MainFile.Logger.Info($"[毁灭裁决] 击杀！获得 {blood} 血仇");
        }
    }
}
