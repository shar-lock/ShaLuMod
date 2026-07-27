// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.ForgeCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class ForgeCmd
{
  private const string _forgeInitialSfx = "event:/sfx/characters/regent/regent_forge";
  private const string _forgeRefineSfx = "event:/sfx/characters/regent/regent_refine";

  public static async Task<IEnumerable<SovereignBlade>> Forge(
    Decimal amount,
    Player player,
    AbstractModel? source)
  {
    if (CombatManager.Instance.IsOverOrEnding)
      return (IEnumerable<SovereignBlade>) Array.Empty<SovereignBlade>();
    List<SovereignBlade> blades = ForgeCmd.GetSovereignBlades(player, false).ToList<SovereignBlade>();
    if (blades.Count == 0)
    {
      SovereignBlade sovereignBlade = player.Creature.CombatState.CreateCard<SovereignBlade>(player);
      sovereignBlade.CreatedThroughForge = true;
      CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat((CardModel) sovereignBlade, PileType.Hand, player);
      blades.Add(sovereignBlade);
      sovereignBlade = (SovereignBlade) null;
    }
    ForgeCmd.IncreaseSovereignBladeDamage(amount, player);
    await Hook.AfterForge(player.Creature.CombatState, amount, player, source);
    return (IEnumerable<SovereignBlade>) blades;
  }

  private static void IncreaseSovereignBladeDamage(Decimal amount, Player player)
  {
    List<SovereignBlade> list = ForgeCmd.GetSovereignBlades(player, true).ToList<SovereignBlade>();
    foreach (SovereignBlade card in list)
    {
      card.AddDamage(amount);
      card.AfterForged();
      ForgeCmd.PlayCombatRoomForgeVfx(player, (CardModel) card);
    }
    ForgeCmd.PreviewSovereignBlade((IReadOnlyCollection<SovereignBlade>) list);
  }

  private static IEnumerable<SovereignBlade> GetSovereignBlades(
    Player player,
    bool includeExhausted)
  {
    return player.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c =>
    {
      if (c.IsDupe)
        return false;
      if (includeExhausted)
        return true;
      CardPile pile = c.Pile;
      return pile == null || pile.Type != PileType.Exhaust;
    })).OfType<SovereignBlade>();
  }

  private static void PreviewSovereignBlade(IReadOnlyCollection<SovereignBlade> blades)
  {
    if (TestMode.IsOn || !LocalContext.IsMine((CardModel) blades.First<SovereignBlade>()))
      return;
    List<SovereignBlade> list1 = blades.Where<SovereignBlade>((Func<SovereignBlade, bool>) (c => c.Pile.Type == PileType.Hand)).ToList<SovereignBlade>();
    List<SovereignBlade> list2 = blades.Where<SovereignBlade>((Func<SovereignBlade, bool>) (c => c.Pile.Type != PileType.Hand)).ToList<SovereignBlade>();
    foreach (CardModel card in list1)
      ((Node) NRun.Instance.GlobalUi.AboveTopBarVfxContainer).AddChildSafely((Node) NCardSmithVfx.Create(NCombatRoom.Instance.Ui.Hand.GetCard(card), false));
    if (list2.Count == 0)
      return;
    ((Node) NRun.Instance.GlobalUi.CardPreviewContainer).AddChildSafely((Node) NCardSmithVfx.Create((IEnumerable<CardModel>) list2, false));
  }

  public static void PlayCombatRoomForgeVfx(Player player, CardModel card)
  {
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(player.Creature);
    if (creatureNode == null)
      return;
    NSovereignBladeVfx vfxNode1 = SovereignBlade.GetVfxNode(player, card);
    bool showFlames = vfxNode1 == null;
    if (showFlames)
    {
      vfxNode1 = NSovereignBladeVfx.Create(card);
      ((Node) creatureNode).AddChildSafely((Node) vfxNode1);
      vfxNode1.Position = Vector2.Zero;
      SfxCmd.Play("event:/sfx/characters/regent/regent_forge");
    }
    else
      SfxCmd.Play("event:/sfx/characters/regent/regent_refine");
    vfxNode1.Forge((float) card.DynamicVars.Damage.IntValue, showFlames);
    if (!showFlames)
      return;
    List<SovereignBlade> list = ForgeCmd.GetSovereignBlades(player, false).ToList<SovereignBlade>();
    for (int index = 0; index < list.Count; ++index)
    {
      NSovereignBladeVfx vfxNode2 = SovereignBlade.GetVfxNode(player, (CardModel) list[index]);
      if (vfxNode2 != null)
        vfxNode2.OrbitProgress = (double) index / (double) list.Count;
    }
  }
}
