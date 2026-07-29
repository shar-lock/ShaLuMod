# 遗物开发

## ⚠️ 强制 PoolAttribute（0.109+）
**所有遗物模型必须带 `[Pool]`**，否则游戏启动注册模型时直接致命错误。故万敌遗物一律继承 `WandiModRelic`（已带 `[Pool(WandiModRelicPool)]`）。**不入奖励池靠 `Rarity` 控制，不是靠去掉 [Pool]**：
- 起手遗物：`Rarity = RelicRarity.Starter`
- 觉醒遗物：`Rarity = RelicRarity.Ancient`

> 踩坑记录见 `BloodOfTheKinslayer` 类注释。

## 起手遗物 + 觉醒替换
- 起手遗物 `Rarity=Starter`，在 `WandiMod.cs` 的 `StartingRelics` 接线。
- 觉醒版（经先古之民欧洛巴斯的「欧洛巴斯之触」替换）：
  - 起手遗物重写 `GetUpgradeReplacement()` → 返回觉醒遗物。BaseLib `StarterUpgradePatches` 让 `TouchOfOrobas` 走此方法。**无需自建先古之民**。
  - 觉醒遗物继承起手遗物（复用效果）+ `Rarity=Ancient` + `GetUpgradeReplacement()=>null`（避免循环）。

## 免死（参考 LizardTail）
- `[SavedProperty] int Charges`（计数器，存档持久化）。
- `ShouldDieLate(Creature)` → `creature != Owner.Creature || Charges <= 0`（返回 false = 拦截死亡）。
- `AfterPreventingDeath(Creature)` → `Flash(); Charges--; CreatureCmd.Heal(...)`。
- 计数器角标数字显示 API 待确认（`SetCounter` / `ChangeCounter`，见代码 TODO）。

## 战斗开始赋予 Power
重写 `BeforeCombatStart()`（参考 `BeltBuckle`）→ `PowerCmd.Apply<XPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, ...)`。
> `ThrowingPlayerChoiceContext` 在 `MegaCrit.Sts2.Core.GameActions.Multiplayer`（不是 CardSelection）。

## 回合钩子
`AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)`（参考 `Akabeko`）→ `participants.Contains(Owner.Creature)` 判断是自己的回合。
