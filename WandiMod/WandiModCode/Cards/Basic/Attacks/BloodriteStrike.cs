using BaseLib.Abstracts;                        // ITranscendenceCard
using BaseLib.Extensions;                       // WithUpgrade
using BaseLib.Utils;                            // CommonActions
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
    /// ITranscendenceCard：返回先古超越形态「血祭·诛王枪」的**规范（canonical）模型**。
    /// 契约（以 MutableModelException 换来的教训）：BaseLib 补丁会把本方法的返回值当作 canonicalCard
    /// 传给 RunState.CreateCard → 内部 ToMutable() 要求规范实例——
    /// 若在这里返回 ToMutable()/已升级的可变实例，AssertCanonical 直接崩溃（欧洛巴斯事件卡死）。
    /// 升级态/附魔由原版 ArchaicTooth.GetTranscendenceTransformedCard 流程自行复制，无需在此处理。
    /// </summary>
    public CardModel GetTranscendenceTransformedCard()
    {
        MainFile.Logger.Info($"[血祭之枪] 先古替换为血祭·诛王枪（原牌升级={IsUpgraded}，由原版流程复制升级态）");
        return ModelDb.Card<BloodriteRegicide>();
    }
}
