// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.PileTypeExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

public static class PileTypeExtensions
{
  public static CardPile GetPile(this PileType pileType, Player player)
  {
    ArgumentNullException.ThrowIfNull((object) player, nameof (player));
    return CardPile.Get(pileType, player) ?? throw new InvalidOperationException($"Tried to get {pileType} pile while out of combat.");
  }

  public static bool IsCombatPile(this PileType pileType)
  {
    bool flag;
    switch (pileType)
    {
      case PileType.Draw:
      case PileType.Hand:
      case PileType.Discard:
      case PileType.Exhaust:
      case PileType.Play:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    return flag;
  }

  public static Vector2 GetTargetPosition(this PileType pileType, NCard? node)
  {
    if (pileType.IsCombatPile() && !CombatManager.Instance.IsInProgress)
      return Vector2.Zero;
    Rect2 viewportRect = ((CanvasItem) NGame.Instance).GetViewportRect();
    Vector2 size = ((Rect2) ref viewportRect).Size;
    switch (pileType)
    {
      case PileType.None:
        return size;
      case PileType.Draw:
        return Vector2.op_Addition(NCombatRoom.Instance.Ui.DrawPile.GlobalPosition, Vector2.op_Multiply(NCombatRoom.Instance.Ui.DrawPile.Size, 0.5f));
      case PileType.Hand:
        return new Vector2((float) ((double) size.X * 0.5 - (double) node.Size.X * 0.5), size.Y - node.Size.Y * 0.5f);
      case PileType.Discard:
        return Vector2.op_Addition(NCombatRoom.Instance.Ui.DiscardPile.GlobalPosition, Vector2.op_Multiply(NCombatRoom.Instance.Ui.DiscardPile.Size, 0.5f));
      case PileType.Exhaust:
        return Vector2.op_Addition(NCombatRoom.Instance.Ui.ExhaustPile.GlobalPosition, Vector2.op_Multiply(NCombatRoom.Instance.Ui.ExhaustPile.Size, 0.5f));
      case PileType.Play:
        return Vector2.op_Addition(Vector2.op_Subtraction(Vector2.op_Multiply(NCombatRoom.Instance.Ui.PlayContainer.Size, 0.5f), Vector2.op_Multiply(node.Size, 0.5f)), Vector2.op_Multiply(Vector2.Up, 100f));
      case PileType.Deck:
        return Vector2.op_Addition(NRun.Instance.GlobalUi.TopBar.Deck.GlobalPosition, Vector2.op_Multiply(NRun.Instance.GlobalUi.TopBar.Deck.Size, 0.5f));
      default:
        throw new ArgumentOutOfRangeException(nameof (pileType), (object) pileType, "Unknown pile type");
    }
  }
}
