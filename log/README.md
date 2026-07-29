# log/ 开发记录目录

## 目录结构

```
log/
├── TODO.md              ← 始终在根目录。当前待办（运行时确认/未完成项）
├── README.md            ← 本文件（规范）
└── YYYY/MM/DD/
    └── dev-log.md       ← 当天的开发记录（做了什么 + commit hash）
```

## 规范

### 开发前
1. 读 `log/TODO.md` — 看有哪些待确认/未完成项，优先处理。
2. 读最近一次 `log/YYYY/MM/DD/dev-log.md` — 了解上次做了什么。

### 开发后（必须做）
1. **更新 `log/TODO.md`** — 完成的项标 ✅ 或删除，新增的项追加。
2. **写当天记录** — 在 `log/{年}/{月}/{日}/dev-log.md` 追加（同一天多次开发则 append，不新建文件）。
   - 格式：`### 任务名（commit hash）` + 要点列表。
3. 提交 git。

### AI 规则
- 每次 session 开始时，**必须先读 `log/TODO.md`** 和最近一次 dev-log。
- session 结束前，**必须更新 TODO.md + 写 dev-log**，然后才能提交。
