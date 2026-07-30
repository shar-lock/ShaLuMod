using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;                 // VengeancePower（血仇消耗门控）
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;        // Creature.GetPowerAmount<T>

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// This is the base class for your mod's cards, which is set up to load the card's images from your mod's resources.
/// When creating a card, right click the Cards folder and create a new file with the Custom Card template.
/// This will generate a class that extends this one.
/// You can also just create the class manually; just make sure to inherit from this class.
/// </summary>
[Pool(typeof(WandiModCardPool))]
public abstract class WandiModCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    // ── 契约终结式血仇消耗门控（GrandFinale 范式）──────────────────────────────────
    // 子类只要声明 new IntVar("BloodCost", N) 即自动启用：
    //   - 血仇不足以支付（消耗后会跌破保底 1 层）→ IsPlayable=false，卡牌灰显、无法拖出（绝不白花能量）。
    //   - 血仇充足 → ShouldGlowGoldInternal=true，卡牌金边高亮提示「现在可以打出」。
    // 参考原生 PactsEnd（始终可打出+fizzle）与 GrandFinale（IsPlayable 灰显）。本 mod 采用 GrandFinale 式
    // （血仇卡有 1-2 费，灰显更省心）。条件 = 当前血仇 > BloodCost（消耗后保底 ≥ 1 层）。
    /// <summary>本卡是否声明了「血仇消耗」(BloodCost 变量)。</summary>
    protected bool HasBloodCost => DynamicVars.TryGetValue("BloodCost", out _);

    /// <summary>
    /// 当前血仇是否足够支付 BloodCost（保底留 1 层）。战斗外（无 Creature，如牌库/预览）返回 true——不阻拦展示。
    /// </summary>
    protected bool CanPayBloodCost
    {
        get
        {
            var creature = Owner?.Creature;
            if (creature == null) return true;                         // 战斗外不阻拦
            if (!DynamicVars.TryGetValue("BloodCost", out var costVar)) return true; // 无 BloodCost 不阻拦
            int blood = creature.GetPowerAmount<VengeancePower>();     // 当前真实血仇层数（无 Power = 0）
            return blood > costVar.IntValue;                           // 消耗 cost 后保底 ≥ 1 层
        }
    }

    /// <summary>血仇消耗卡：血仇不足时灰显不可打出（GrandFinale 式）。</summary>
    protected override bool IsPlayable => HasBloodCost ? CanPayBloodCost : base.IsPlayable;

    /// <summary>血仇消耗卡：血仇充足时金边高亮（战斗内才高亮，避免牌库里误亮）。</summary>
    protected override bool ShouldGlowGoldInternal =>
        HasBloodCost && Owner?.Creature != null && CanPayBloodCost;
}