# 卡牌开发

## 目录结构
卡牌按 `稀有度/类型/xxx.cs` 归档（namespace 统一 `WandiMod.WandiModCode.Cards`，与文件夹无关）：
```
Cards/
├── Basic/{Attacks,Skills}/     起手牌
├── Common/{Attacks,Skills}/    普通池
├── Uncommon/{Attacks,Skills,Powers}/  罕见池（待开发）
├── Rare/{Attacks,Skills,Powers}/      稀有池（待开发）
├── Token/                      机制卡/生成卡（不入池）
├── WandiModCard.cs             基类（留根）
└── WandiModKeywords.cs         关键词定义（留根）
```

## 基类与构造
继承 `WandiModCard`（自带 `[Pool(WandiModCardPool)]` 自动入万敌卡池）。
```csharp
public BloodRite() : base(cost: 1, type: CardType.Attack, rarity: CardRarity.Common, target: TargetType.AnyEnemy) { }
```

## CanonicalVars（数值真相源）
```csharp
protected override IEnumerable<DynamicVar> CanonicalVars => [
    new DamageVar(12, ValueProp.Move).WithUpgradeTo(16),  // 伤害 12→16
    new HpLossVar(3),                                      // 自伤 3
    new IntVar("Vengeance", 1),                            // 自定义变量（授予血仇）
];
```
- 自定义变量名（如 `"Vengeance"` / `"Strife"`）对应本地化占位符 `{Vengeance:diff()}`。

> ### ⚠️⚠️ 重点：升级值必须用 `WithUpgradeTo`（目标值），**禁用** BaseLib 的 `WithUpgrade`（增量）！
> - BaseLib `WithUpgrade(x)` → 升级时调游戏 `DynamicVar.UpgradeValueBy(x)`，实现是 **`BaseValue += x`（增量）**。
>   写目标值会**双重叠加**：`DamageVar(6).WithUpgrade(9)` 升级后 = **15**（6+9），不是 9！
>   （本 mod 曾全库踩坑：打击升级 15、御敌升级 10 纷争，2026-07-30 全面整改。）
> - 本 mod 扩展 **`WithUpgradeTo(x)`**（`Extensions/WandiDynamicVarExtensions.cs`，
>   `using WandiMod.WandiModCode.Extensions;`）：升级后 `BaseValue == x`，内部自动换算增量 `x − 基础值`。
>   与设计文档「基础值 / 升级值」写法一致，注释里的 `12→16` 等表述也保持成立。
> - 支持**降值**升级（如血仇消耗 2→1）：`WithUpgradeTo(1)` 自动换算为负增量。
> - 例外：`EnergyCost.UpgradeBy(-1)` 是原版增量 API（降费写法 `UpgradeBy(-1)`），不在此列。

## OnPlay(choiceContext, cardPlay)
两层 API（**优先高层**）：
- **高层** `CommonActions.CardAttack(this, cardPlay).Execute(ctx)` / `CardBlock` / `Apply<T>` / `Draw`——自动读 Vars、处理 TargetType、VFX。
- **低层** `DamageCmd.Attack(dmg).FromCard(this).Targeting(enemy).Execute(ctx)`——需手动控制时（如逐敌不同伤害）。`.WithValueProp(ValueProp.Move)`（BaseLib 扩展，`using BaseLib.Extensions;`）让它吃力量。
- 授予 Power → 走该 Power 的 `Grant` 静态入口（见 code-style.md）。
- **自伤**：`CreatureCmd.Damage(ctx, Owner.Creature, amount, ValueProp.Unblockable|Unpowered|Move, this, cardPlay)` → 全额计入失血 → 自动触发血仇叠层。

## 关键词
卡牌带 `public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];`（提供 tooltip，见 keyword-highlight.md）。

## 生成卡（不入卡池）
参考 `ConquerAllLands`（荡平万邦，由遗物生成）：
- 继承 **`CustomCardModel`** + `rarity: CardRarity.Token` + **`[Pool(typeof(TokenCardPool))]`**（原版机制卡池，`MegaCrit.Sts2.Core.Models.CardPools`，与 Soul/Shiv 同池）。
- ⚠️ **[Pool] 不可省略**（两层强制，缺了启动即崩）：
  1. BaseLib `CustomCardModel` 构造时（`autoAdd` 默认 true）走 `CustomContentDictionary.AddModel` → 无 [Pool] 抛「must be marked with a PoolAttribute」；
  2. 游戏 `CardModel.Pool` 找不到所属池抛 `InvalidProgramException`（手牌渲染取 EnergyIcon/Frame 必触达）。
  原生 Token 卡（Soul 等）没 [Pool] 是因为不经过 BaseLib 注册，由 `TokenCardPool.GenerateAllCards` 显式列出——自定义卡只能 [Pool] 注入。Token 池不是奖励/商店/战斗内生成的取数源，配合 Token 稀有度 = 双保险不掉落。
- 手动覆写图标路径（`CustomPortraitPath` / `PortraitPath`，用 `BigCardImagePath()` / `CardImagePath()`）；卡框/能量图标走 Token 池原版无色样式。

## 先古卡（Ancient，经先古之民）
- **达弗·尘封魔典**给的先古能力卡 → 实现 `ITomeCard`（`TomeCharacter` 默认按卡池识别为万敌）。BaseLib `DustyTomeCardPatch` 自动选中。
- **欧洛巴斯·古老牙齿**升级起手卡 → 在该起手卡上实现 `ITranscendenceCard.GetTranscendenceTransformedCard()` 返回先古版（按原牌是否升级返回对应版本）。BaseLib `ArchaicToothTranscendenceUpgradesPatch` 自动替换。
