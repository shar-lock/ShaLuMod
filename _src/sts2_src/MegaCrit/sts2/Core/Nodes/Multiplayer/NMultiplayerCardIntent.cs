// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerCardIntent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerCardIntent.cs")]
public class NMultiplayerCardIntent : Control
{
  private NCard _cardNode;
  private CardModel? _card;

  public CardModel? Card
  {
    get => this._card;
    set
    {
      this._cardNode.Model = value;
      this._card = value;
      if (this._card == null || !((Node) this).IsNodeReady())
        return;
      NCard cardNode = this._cardNode;
      CardPile pile = this._card.Pile;
      int type = pile != null ? (int) pile.Type : 0;
      cardNode.UpdateVisuals((PileType) type, CardPreviewMode.Normal);
    }
  }

  public override void _Ready()
  {
    this._cardNode = ((Node) this).GetNode<NCard>(NodePath.op_Implicit("%Card"));
    if (this._card == null)
      return;
    NCard cardNode = this._cardNode;
    CardPile pile = this._card.Pile;
    int type = pile != null ? (int) pile.Type : 0;
    cardNode.UpdateVisuals((PileType) type, CardPreviewMode.Normal);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NMultiplayerCardIntent.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NMultiplayerCardIntent.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMultiplayerCardIntent.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NMultiplayerCardIntent.PropertyName._cardNode))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NMultiplayerCardIntent.PropertyName._cardNode))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMultiplayerCardIntent.PropertyName._cardNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMultiplayerCardIntent.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NMultiplayerCardIntent.PropertyName._cardNode, ref variant))
      return;
    this._cardNode = ((Variant) ref variant).As<NCard>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _cardNode = StringName.op_Implicit(nameof (_cardNode));
  }

  public class SignalName : Control.SignalName
  {
  }
}
