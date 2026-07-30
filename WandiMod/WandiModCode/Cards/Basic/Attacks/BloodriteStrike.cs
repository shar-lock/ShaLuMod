using BaseLib.Abstracts;                        // ITranscendenceCard
using BaseLib.Extensions;                       // WithUpgrade
using BaseLib.Utils;                            // CommonActions
using MegaCrit.Sts2.Core.Commands;              // CardCmd
using MegaCrit.Sts2.Core.Entities.Cards;        // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.GameActions.Multiplayer;// PlayerChoiceContext
using MegaCrit.Sts2.Core.Localization.DynamicVars;// DamageVar / IntVar
using MegaCrit.Sts2.Core.Models;                 // CardModel / ModelDb
using MegaCrit.Sts2.Core.ValueProps;            // ValueProp
using WandiMod.WandiModCode.Extensions;          // WithUpgradeTo
using WandiMod.WandiModCode.Powers;             // VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 血祭之枪 / Bloodrite Strike（起手 · 攻击，万敌特色打击位）
/// 造成 6 点伤害，获得 1 层【血仇】。升级：9 伤害，2 血仇。
/// —— 打击 + 产血仇二合一，让最基础的攻击也喂养血仇引擎。
/// 实现 ITranscendenceCard：经先古之民欧洛巴斯的古老牙齿替换为「血祭·诛王枪」（先古版）。
/// </summary>
public class BloodriteStrike : WandiModCard, ITranscendenceCard
{
    public BloodriteStrike() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Basic,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6, ValueProp.Move).WithUpgradeTo(9),
        new IntVar("Vengeance", 1).WithUpgradeTo(2),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);
        await VengeancePower.Grant(choiceContext, Owner.Creature, DynamicVars["Vengeance"].IntValue, this);
    }

    /// <summary>
    /// ITranscendenceCard：返回先古超越形态「血祭·诛王枪」。
    /// 游戏在欧洛巴斯古老牙齿替换时调用此方法。
    /// </summary>
    public CardModel GetTranscendenceTransformedCard()
    {
        // 创建可变实例（参考荡平万邦的 CombatState.CreateCard 写法——但这里是模型层，用 ModelDb + ToMutable）
        var card = ModelDb.Card<BloodriteRegicide>().ToMutable();
        // 若原牌已升级，先古版也给升级态
        if (IsUpgraded)
            CardCmd.Upgrade(card);
        MainFile.Logger.Info($"[血祭之枪] 先古替换为血祭·诛王枪（原牌升级={IsUpgraded}）");
        return card;
    }
}
