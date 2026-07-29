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
    new DamageVar(12, ValueProp.Move).WithUpgrade(16),  // 伤害 12→16
    new HpLossVar(3),                                    // 自伤 3
    new IntVar("Vengeance", 1),                          // 自定义变量（授予血仇）
];
```
- `.WithUpgrade(n)` 设升级值，逻辑代码读到的就是升级后值，**无需为升级改逻辑**。
- 自定义变量名（如 `"Vengeance"` / `"Strife"`）对应本地化占位符 `{Vengeance:diff()}`。

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
- 继承 **`CustomCardModel`（不标 [Pool]）** + `rarity: CardRarity.Token`（Token = 生成卡不掉落，参考原生 `Soul` / `Apparition`）。
- 手动覆写图标路径（`CustomPortraitPath` / `PortraitPath`，用 `BigCardImagePath()` / `CardImagePath()`）。
- 卡牌**不像遗物那样强制 PoolAttribute**（原生 Token 卡即先例），无 [Pool] 不会崩。

## 先古卡（Ancient，经先古之民）
- **达弗·尘封魔典**给的先古能力卡 → 实现 `ITomeCard`（`TomeCharacter` 默认按卡池识别为万敌）。BaseLib `DustyTomeCardPatch` 自动选中。
- **欧洛巴斯·古老牙齿**升级起手卡 → 在该起手卡上实现 `ITranscendenceCard.GetTranscendenceTransformedCard()` 返回先古版（按原牌是否升级返回对应版本）。BaseLib `ArchaicToothTranscendenceUpgradesPatch` 自动替换。
