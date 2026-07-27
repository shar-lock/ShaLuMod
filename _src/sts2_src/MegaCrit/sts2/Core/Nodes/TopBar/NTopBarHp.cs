// Decompiled with JetBrains decompiler
// Type: MegaCrit.sts2.Core.Nodes.TopBar.NTopBarHp
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.sts2.Core.Nodes.TopBar;

[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarHp.cs")]
public class NTopBarHp : NClickableControl
{
  private Player? _player;
  private MegaLabel _hpLabel;

  public override void _Ready()
  {
    this._hpLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%HpLabel"));
    this.ConnectSignals();
  }

  public override void _ExitTree()
  {
    if (this._player == null)
      return;
    this._player.Creature.CurrentHpChanged -= new Action<int, int>(this.UpdateHealth);
    this._player.Creature.MaxHpChanged -= new Action<int, int>(this.UpdateHealth);
  }

  public void Initialize(Player player)
  {
    this._player = player;
    this._player.Creature.CurrentHpChanged += new Action<int, int>(this.UpdateHealth);
    this._player.Creature.MaxHpChanged += new Action<int, int>(this.UpdateHealth);
    this.UpdateHealth(0, 0);
  }

  private void UpdateHealth(int _, int __)
  {
    if (this._player == null)
      return;
    Creature creature = this._player.Creature;
    this._hpLabel.SetTextAutoSize($"{creature.CurrentHp}/{creature.MaxHp}");
  }

  protected override void OnFocus()
  {
    NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) new HoverTip(new LocString("static_hover_tips", "HIT_POINTS.title"), new LocString("static_hover_tips", "HIT_POINTS.description")))?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, new Vector2(0.0f, this.Size.Y + 20f)), false);
  }

  protected override void OnUnfocus() => NHoverTipSet.Remove((Control) this);

  public async Task LerpAtNeow()
  {
    if (this._player == null)
      return;
    this._hpLabel.SetTextAutoSize($"0/{this._player.Creature.MaxHp}");
    await Cmd.Wait(0.5f);
    ((Node) this).CreateTween().TweenMethod(Callable.From<float>(new Action<float>(this.UpdateHpTween)), Variant.op_Implicit(0.0f), Variant.op_Implicit(1f), 1.0).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 7L);
  }

  private void UpdateHpTween(float tweenAmount)
  {
    if (this._player == null)
      return;
    Creature creature = this._player.Creature;
    this._hpLabel.SetTextAutoSize($"{(int) Math.Round((double) creature.CurrentHp * (double) tweenAmount)}/{creature.MaxHp}");
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NTopBarHp.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarHp.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarHp.MethodName.UpdateHealth, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTopBarHp.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarHp.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarHp.MethodName.UpdateHpTween, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("tweenAmount"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTopBarHp.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarHp.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarHp.MethodName.UpdateHealth) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.UpdateHealth(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarHp.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarHp.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTopBarHp.MethodName.UpdateHpTween) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateHpTween(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTopBarHp.MethodName._Ready) || StringName.op_Equality(ref method, NTopBarHp.MethodName._ExitTree) || StringName.op_Equality(ref method, NTopBarHp.MethodName.UpdateHealth) || StringName.op_Equality(ref method, NTopBarHp.MethodName.OnFocus) || StringName.op_Equality(ref method, NTopBarHp.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NTopBarHp.MethodName.UpdateHpTween) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NTopBarHp.PropertyName._hpLabel))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hpLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NTopBarHp.PropertyName._hpLabel))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<MegaLabel>(ref this._hpLabel);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NTopBarHp.PropertyName._hpLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NTopBarHp.PropertyName._hpLabel, Variant.From<MegaLabel>(ref this._hpLabel));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NTopBarHp.PropertyName._hpLabel, ref variant))
      return;
    this._hpLabel = ((Variant) ref variant).As<MegaLabel>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName UpdateHealth = StringName.op_Implicit(nameof (UpdateHealth));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName UpdateHpTween = StringName.op_Implicit(nameof (UpdateHpTween));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName _hpLabel = StringName.op_Implicit(nameof (_hpLabel));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
