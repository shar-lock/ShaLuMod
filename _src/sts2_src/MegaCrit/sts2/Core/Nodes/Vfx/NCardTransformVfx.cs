// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NCardTransformVfx
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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NCardTransformVfx.cs")]
public class NCardTransformVfx : Node2D
{
  private Tween? _tween;
  private CardModel _startCard;
  private CardModel _endCard;
  private IEnumerable<RelicModel>? _relicsToFlash;

  private static string ScenePath => SceneHelper.GetScenePath("vfx/vfx_card_transform");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NCardTransformVfx.ScenePath);
    }
  }

  public static NCardTransformVfx? Create(
    CardModel startCard,
    CardModel endCard,
    IEnumerable<RelicModel>? relicsToFlash)
  {
    if (TestMode.IsOn)
      return (NCardTransformVfx) null;
    NCardTransformVfx ncardTransformVfx = PreloadManager.Cache.GetScene(NCardTransformVfx.ScenePath).Instantiate<NCardTransformVfx>((PackedScene.GenEditState) 0L);
    ncardTransformVfx._startCard = startCard;
    ncardTransformVfx._endCard = endCard;
    ncardTransformVfx._relicsToFlash = relicsToFlash;
    return ncardTransformVfx;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlayAnimation());

  private async Task<bool> WaitAndInterruptIfNecessary(float seconds, NCard cardNode)
  {
    float num;
    for (float num1 = 0.0f; (double) num1 <= (double) seconds; num1 = num + await ((Node) this).AwaitProcessFrame())
    {
      if (!((Node) cardNode).IsInsideTree() || this._endCard.Pile == null)
        return false;
      num = num1;
    }
    return true;
  }

  public override void _ExitTree() => this._tween?.Kill();

  private async Task PlayAnimation()
  {
    SfxCmd.Play("event:/sfx/ui/cards/card_transform");
    Control node = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CardContainer"));
    NCard cardNode = NCard.Create(this._startCard);
    ((Node) node).AddChildSafely((Node) cardNode);
    cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) cardNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1f)), 0.25).From(Variant.op_Implicit(Vector2.Zero)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    if (!await this.WaitAndInterruptIfNecessary(0.75f, cardNode))
    {
      ((Node) this).QueueFreeSafely();
      cardNode = (NCard) null;
    }
    else
    {
      NCardTransformShineVfx transformShineVfx = NCardTransformShineVfx.Create(cardNode, this._endCard, this._relicsToFlash);
      if (transformShineVfx != null)
        await transformShineVfx.PlayAnimation();
      if (!await this.WaitAndInterruptIfNecessary(0.3f, cardNode))
      {
        ((Node) this).QueueFreeSafely();
        cardNode = (NCard) null;
      }
      else
      {
        if (this._relicsToFlash != null)
        {
          foreach (RelicModel relic in this._relicsToFlash)
          {
            relic.Flash();
            cardNode.FlashRelicOnCard(relic);
          }
        }
        if (!await this.WaitAndInterruptIfNecessary(0.5f, cardNode))
        {
          ((Node) this).QueueFreeSafely();
          cardNode = (NCard) null;
        }
        else if (this._endCard.Pile == null)
        {
          ((Node) this).QueueFreeSafely();
          cardNode = (NCard) null;
        }
        else
        {
          ((Node) cardNode).Reparent((Node) this, true);
          cardNode.Position = Vector2.Zero;
          NCardFlyVfx child = NCardFlyVfx.Create(cardNode, this._endCard.Pile.Type, false, this._endCard.Owner.Character.TrailPath);
          Node parent = this._endCard.Pile.Type != PileType.Deck ? (Node) NCombatRoom.Instance?.CombatVfxContainer : NRun.Instance?.GlobalUi.TopBar.TrailContainer;
          if (parent != null)
            parent.AddChildSafely((Node) child);
          if (child?.SwooshAwayCompletion != null)
            await child.SwooshAwayCompletion.Task;
          ((Node) this).QueueFreeSafely();
          cardNode = (NCard) null;
        }
      }
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCardTransformVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardTransformVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardTransformVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardTransformVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardTransformVfx.MethodName._Ready) || StringName.op_Equality(ref method, NCardTransformVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NCardTransformVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NCardTransformVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardTransformVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardTransformVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NCardTransformVfx.PropertyName._tween, ref variant))
      return;
    this._tween = ((Variant) ref variant).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
