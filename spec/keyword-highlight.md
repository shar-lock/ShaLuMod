# 关键词高亮规范（纷争 / 血仇）

自定义关键词 = `WandiModKeywords`（`[CustomEnum]` 注册，字段名 → 本地化键 `WANDIMOD-<字段名>`）。

## 三件套（缺一不可）
1. **`AutoKeywordPosition.None`**（不追加独立行）——避免与正文重复。
2. **本地化描述用 `[gold]纷争[/gold]`** 内联高亮（颜色）。占位符 `{Strife:diff()}` 不动。
3. **卡牌带 `CanonicalKeywords => [WandiModKeywords.Strife]`**——`CardModel.HoverTips` 自动调 `FromKeyword` 挂 tooltip（hover 卡牌弹词条）。

## 注册（WandiModKeywords.cs）
```csharp
[CustomEnum]
[KeywordProperties(AutoKeywordPosition.None)]  // None=不追加行；richKeyword 默认 true（tooltip 用）
public static CardKeyword Strife;
```

## 本地化
- `card_keywords.json`：`WANDIMOD-STRIFE.title` / `.description`（词条名 + 说明）。
- `cards.json` 描述：
  - zhs：`获得 {Strife:diff()} 点[gold]纷争[/gold]。`
  - eng：`Gain {Strife:diff()} [gold]Strife[/gold].`（⚠️ 别包裹到 `{Strife:diff()}` 占位符里）

## DO / DON'T
- ✅ `[KeywordProperties(None)]` + `[gold]` 内联 + CanonicalKeywords。
- ❌ 用 `AutoKeywordPosition.After`（会在卡牌下追加一行冗余的关键词）。
- ❌ 在 eng 里全局替换 `Strife`→`[gold]Strife[/gold]`（会破坏 `{Strife:diff()}` 占位符；只换显示词）。

## 排查
词条不显示 / 不高亮：检查 `card_keywords.json` 键名 `WANDIMOD-STRIFE.*`、卡牌是否带 `CanonicalKeywords`、描述是否 `[gold]` 包裹。
