# WandiMod 开发规范（spec/）

> **AI 开发前必读**：动 `WandiMod/` 下任何代码前，先读完本目录全部 md。本目录是已确立的约束，不是建议。

万敌（Mydei）角色 mod 的开发约束汇总。配套设计文档在 `doc/`（设计方案 / 卡牌 / 遗物 / 构建验证）。

## 文件索引（建议按序读）

| 文件 | 内容 |
|---|---|
| [code-style.md](code-style.md) | 中文注释、日志、命名、Power.Grant 模式等通用代码规范 |
| [core-mechanics.md](core-mechanics.md) | 血仇（Vengeance）+ 纷争（Strife）两大 Power 的机制与钩子 |
| [card-dev.md](card-dev.md) | 卡牌开发：基类、Vars、OnPlay、生成卡、先古卡 |
| [keyword-highlight.md](keyword-highlight.md) | 纷争 / 血仇关键词高亮规范（None + [gold] + CanonicalKeywords） |
| [relic-dev.md](relic-dev.md) | 遗物开发：Pool 强制要求、欧洛巴斯替换、免死 |
| [build-reference.md](build-reference.md) | 构建约束 + API / 数据参考路径 |
| [assets.md](assets.md) | 图片资源注册（卡牌/Power/遗物/角色 UI）；含 **原版角色 UI 调研**（路径、PNG/Spine/场景、分辨率）与去战士化清单 |

## 开发记录

开发进度和待办在 [`log/`](../log/) 目录，按 `YYYY/MM/DD/dev-log.md` 归档：
- [`log/TODO.md`](../log/TODO.md) — 始终最新的待办（运行时确认/未完成项）
- [`log/README.md`](../log/README.md) — 记录规范（开发前读 TODO，开发后写 dev-log + 更新 TODO）

## 快速定位

| 要找 | 去哪 |
|---|---|
| 游戏钩子 / 方法签名 | `_src/sts2_src/`（反编译源，**查证 API 的权威**） |
| BaseLib 封装（CommonActions / CustomXModel / Grant 范式） | `BaseLib-StS2/` |
| 原版卡数值 / JSON 字段 | `sts2_database/cards/*.json`（580 张） |
| wiki 工作流 | `ModTemplate-StS2.wiki/`（含 `zh/` 中文译本） |
| 本地化文案 | `WandiMod/WandiMod/localization/{eng,zhs}/*.json` |
| 设计文档 | `doc/` |
