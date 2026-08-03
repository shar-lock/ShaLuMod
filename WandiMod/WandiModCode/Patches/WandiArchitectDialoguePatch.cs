using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using WandiMod.WandiModCode.Character;

namespace WandiMod.WandiModCode.Patches;

/// <summary>
/// 注入万敌的建筑师对话——原版 TheArchitect.DefineDialogues() 硬编码了 5 个原版角色，
/// 不含 mod 角色。此 Postfix 向返回的 CharacterDialogues 字典注入万敌的对话条目。
/// 对话文本在 localization/{lang}/ancients.json，由 PopulateLocKeys 自动匹配。
/// </summary>
[HarmonyPatch(typeof(TheArchitect), nameof(TheArchitect.DefineDialogues))]
static class WandiArchitectDialoguePatch
{
    static void Postfix(ref AncientDialogueSet __result)
    {
        string charKey = ModelDb.Character<WandiMod>().Id.Entry;

        // 万敌对话：2 轮
        //  dialogueIndex=0：首次通关（VisitIndex=0，3 行，非重复）
        //  dialogueIndex=1：重复通关（VisitIndex=1，2 行，r 后缀=重复——通关≥2次后循环）
        __result.CharacterDialogues[charKey] = new AncientDialogue[]
        {
            // 首次通关（3 行：万敌→建筑师→万敌）
            new AncientDialogue("", "", "")
            {
                VisitIndex = 0,
                EndAttackers = ArchitectAttackers.Both
            },
            // 重复通关（2 行：万敌→建筑师，r 后缀）
            new AncientDialogue("", "")
            {
                VisitIndex = 1,
                EndAttackers = ArchitectAttackers.Both
            },
        };

        MainFile.Logger.Info("[万敌] 建筑师对话已注入");
    }
}
