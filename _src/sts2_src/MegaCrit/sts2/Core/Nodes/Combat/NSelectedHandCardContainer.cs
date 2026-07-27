// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NSelectedHandCardContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NSelectedHandCardContainer.cs")]
public class NSelectedHandCardContainer : Control
{
  public NPlayerHand? Hand { get; set; }

  public List<NSelectedHandCardHolder> Holders
  {
    get
    {
      return ((IEnumerable) ((Node) this).GetChildren(false)).OfType<NSelectedHandCardHolder>().ToList<NSelectedHandCardHolder>();
    }
  }

  public override void _Ready()
  {
    ((GodotObject) this).Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
  }

  public NSelectedHandCardHolder Add(NHandCardHolder originalHolder)
  {
    NCard cardNode = originalHolder.CardNode;
    Vector2 globalPosition = cardNode.GlobalPosition;
    NSelectedHandCardHolder child = NSelectedHandCardHolder.Create(originalHolder);
    ((GodotObject) child).Connect(NCardHolder.SignalName.Pressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.DeselectHolder)), 4U);
    ((Node) this).AddChildSafely((Node) child);
    this.RefreshHolderPositions();
    cardNode.GlobalPosition = globalPosition;
    return child;
  }

  private void RefreshHolderPositions()
  {
    int count = this.Holders.Count;
    this.FocusMode = count > 0 ? (Control.FocusModeEnum) 2L : (Control.FocusModeEnum) 0L;
    if (count == 0)
      return;
    float x = this.Holders.First<NSelectedHandCardHolder>().Size.X;
    float num = (float) (-(double) x * (double) (count - 1) / 2.0);
    for (int index = 0; index < count; ++index)
    {
      this.Holders[index].Position = new Vector2(num, 0.0f);
      num += x;
      this.Holders[index].FocusNeighborLeft = index > 0 ? ((Node) this.Holders[index - 1]).GetPath() : ((Node) this.Holders[this.Holders.Count - 1]).GetPath();
      this.Holders[index].FocusNeighborRight = index < this.Holders.Count - 1 ? ((Node) this.Holders[index + 1]).GetPath() : ((Node) this.Holders[0]).GetPath();
    }
  }

  private void DeselectHolder(NCardHolder holder)
  {
    NSelectedHandCardHolder child = (NSelectedHandCardHolder) holder;
    this.Hand.DeselectCard(child.CardNode);
    ((Node) this).RemoveChildSafely((Node) child);
    ((Node) child).QueueFreeSafely();
    this.RefreshHolderPositions();
  }

  public void DeselectCard(CardModel card)
  {
    this.DeselectHolder((NCardHolder) this.Holders.First<NSelectedHandCardHolder>((Func<NSelectedHandCardHolder, bool>) (child => child.CardNode.Model == card)));
  }

  private void OnFocus()
  {
    NSelectedHandCardHolder control = this.Holders.FirstOrDefault<NSelectedHandCardHolder>();
    if (control == null)
      return;
    control.TryGrabFocus();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NSelectedHandCardContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSelectedHandCardContainer.MethodName.Add, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("originalHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSelectedHandCardContainer.MethodName.RefreshHolderPositions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSelectedHandCardContainer.MethodName.DeselectHolder, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSelectedHandCardContainer.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSelectedHandCardContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSelectedHandCardContainer.MethodName.Add) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NSelectedHandCardHolder nselectedHandCardHolder = this.Add(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NSelectedHandCardHolder>(ref nselectedHandCardHolder);
      return true;
    }
    if (StringName.op_Equality(ref method, NSelectedHandCardContainer.MethodName.RefreshHolderPositions) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshHolderPositions();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSelectedHandCardContainer.MethodName.DeselectHolder) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.DeselectHolder(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSelectedHandCardContainer.MethodName.OnFocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnFocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSelectedHandCardContainer.MethodName._Ready) || StringName.op_Equality(ref method, NSelectedHandCardContainer.MethodName.Add) || StringName.op_Equality(ref method, NSelectedHandCardContainer.MethodName.RefreshHolderPositions) || StringName.op_Equality(ref method, NSelectedHandCardContainer.MethodName.DeselectHolder) || StringName.op_Equality(ref method, NSelectedHandCardContainer.MethodName.OnFocus) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NSelectedHandCardContainer.PropertyName.Hand))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this.Hand = VariantUtils.ConvertTo<NPlayerHand>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NSelectedHandCardContainer.PropertyName.Hand))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    ref godot_variant local = ref value;
    NPlayerHand hand = this.Hand;
    godot_variant from = VariantUtils.CreateFrom<NPlayerHand>(ref hand);
    local = from;
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSelectedHandCardContainer.PropertyName.Hand, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName hand1 = NSelectedHandCardContainer.PropertyName.Hand;
    NPlayerHand hand2 = this.Hand;
    Variant variant = Variant.From<NPlayerHand>(ref hand2);
    serializationInfo.AddProperty(hand1, variant);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NSelectedHandCardContainer.PropertyName.Hand, ref variant))
      return;
    this.Hand = ((Variant) ref variant).As<NPlayerHand>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Add = StringName.op_Implicit(nameof (Add));
    public static readonly StringName RefreshHolderPositions = StringName.op_Implicit(nameof (RefreshHolderPositions));
    public static readonly StringName DeselectHolder = StringName.op_Implicit(nameof (DeselectHolder));
    public static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Hand = StringName.op_Implicit(nameof (Hand));
  }

  public class SignalName : Control.SignalName
  {
  }
}
