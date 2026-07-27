// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NCardUpgradeVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NCardUpgradeVfx.cs")]
public class NCardUpgradeVfx : Node2D
{
  private CardModel _card;
  private CancellationTokenSource? _cts;

  private static string ScenePath => SceneHelper.GetScenePath("vfx/vfx_card_upgrade");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NCardUpgradeVfx.ScenePath);
    }
  }

  public static NCardUpgradeVfx? Create(CardModel card)
  {
    if (TestMode.IsOn)
      return (NCardUpgradeVfx) null;
    NCardUpgradeVfx ncardUpgradeVfx = PreloadManager.Cache.GetScene(NCardUpgradeVfx.ScenePath).Instantiate<NCardUpgradeVfx>((PackedScene.GenEditState) 0L);
    ncardUpgradeVfx._card = card;
    return ncardUpgradeVfx;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlayAnimation());

  public override void _ExitTree() => this._cts?.Cancel();

  private async Task PlayAnimation()
  {
    this._cts = new CancellationTokenSource();
    NCard cardNode = NCard.Create(this._card);
    ((Node) this).AddChildSafely((Node) cardNode);
    ((Node) this).MoveChildSafely((Node) cardNode, 0);
    cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
    ((Node) this).GetNode<CpuParticles2D>(NodePath.op_Implicit("%Particle")).Emitting = true;
    PileType pileType = this._card.Pile.Type;
    ((Node) this).CreateTween().TweenProperty((GodotObject) cardNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1f)), 0.25).From(Variant.op_Implicit(Vector2.Zero)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    await Cmd.Wait(1.75f, this._cts.Token);
    if (this._card.Pile != null)
      pileType = this._card.Pile.Type;
    NCardFlyVfx child = NCardFlyVfx.Create(cardNode, pileType, false, this._card.Owner.Character.TrailPath);
    Node parent = pileType != PileType.Deck ? (Node) NCombatRoom.Instance?.CombatVfxContainer : NRun.Instance?.GlobalUi.TopBar.TrailContainer;
    if (parent != null)
      parent.AddChildSafely((Node) child);
    if (child?.SwooshAwayCompletion != null)
      await child.SwooshAwayCompletion.Task;
    if (this._cts.IsCancellationRequested)
    {
      cardNode = (NCard) null;
    }
    else
    {
      ((Node) this).QueueFreeSafely();
      cardNode = (NCard) null;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCardUpgradeVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardUpgradeVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardUpgradeVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardUpgradeVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardUpgradeVfx.MethodName._Ready) || StringName.op_Equality(ref method, NCardUpgradeVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
