# 代码规范

## 中文注释（强制）
关键代码必须写中文注释（沿用 `WandiMod.cs` 风格，如「万敌主色调：虚数金」）。每个类 / 方法 / 关键逻辑注释：
- **类**：对应哪张卡 / 遗物 / Power + 机制概述。
- **钩子**：作用 + 参考的原生类（如「失血叠层——参考 RupturePower」）。
- **非显然数值 / 条件**（如「+2%/层」「血仇≥7 触发」）。

不确定的签名标 `// TODO: 运行时确认`。英文标识符（类名、API）保持原样。

## 日志（易错点必打）
用 `MainFile.Logger`（**不是** Godot `GD.Print`）：
- `Error`：不应发生的空引用 / 异常时序（如 OnPlay 时 `Owner.Creature` 为空）。
- `Warn`：可接受但需关注的边界（如数量 ≤0、打满上限）。
- `Info`：关键触发（如血仇≥7 生成荡平万邦、回合加血仇）。
- `Debug`：常规打出（平衡调试用）。

范例见 `VengeancePower.Grant` / `StrifePower.Grant`：空 target 记 Error 并 return，**不抛异常**（避免打断战斗流程）。

## 命名 / 资源
- 类名 = 英文名 PascalCase（`BloodRite`）；Power 用 `XPower`（`VengeancePower`，对齐原生 `StrengthPower`）。
- 资源文件名 = `Id.Entry.RemovePrefix().ToLowerInvariant()`（类名小写），缺图自动回退占位（`card.png` / `power.png` / `relic.png`）。

## 基类
| 类型 | 基类 | 说明 |
|---|---|---|
| 卡牌 | `WandiModCard` | 带 `[Pool(WandiModCardPool)]`，自动入池 |
| Power | `WandiModPower` | 设 `Type` / `StackType` |
| 遗物 | `WandiModRelic` | 带 `[Pool]`，**强制**（见 relic-dev.md） |

## Power.Grant 模式
卡牌授予权力 / 计数 Power，走 Power 上的 **`static Grant(ctx, target, amount, cardSource)`** 统一入口（参考 `VengeancePower.Grant` / `StrifePower.Grant`）：带空值 / 数量守卫 + 日志 + 集中逻辑。**不要**在卡牌 OnPlay 里直接散写 `PowerCmd.Apply`。
