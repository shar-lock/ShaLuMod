// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Map.NMapBg
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Map;

[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapBg.cs")]
public class NMapBg : VBoxContainer
{
  private IRunState _runState;
  private TextureRect _mapTop;
  private TextureRect _mapMid;
  private TextureRect _mapBot;
  private NMapDrawings _drawings;
  private Window _window;
  private const float _sixteenByNine = 1.77777779f;
  private const float _fourByThree = 1.33333337f;
  private const float _defaultY = -1620f;
  private const float _adjustY = -1540f;
  private float _offsetX;

  public override void _Ready()
  {
    this._mapTop = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("MapTop"));
    this._mapMid = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("MapMid"));
    this._mapBot = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("MapBot"));
    this._drawings = ((Node) this).GetNode<NMapDrawings>(NodePath.op_Implicit("%Drawings"));
    this._window = ((Node) this).GetTree().Root;
    ((GodotObject) this._window).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
    this.OnWindowChange();
    this._offsetX = ((Control) this).Position.X;
    ((GodotObject) this).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnVisibilityChanged)), 0U);
  }

  public void Initialize(IRunState runState) => this._runState = runState;

  private void OnVisibilityChanged()
  {
    ActModel act = this._runState.Act;
    this._mapTop.Texture = act.MapTopBg;
    this._mapMid.Texture = act.MapMidBg;
    this._mapBot.Texture = act.MapBotBg;
  }

  private void OnWindowChange()
  {
    float num = Math.Max(1.33333337f, (float) this._window.Size.X / (float) this._window.Size.Y);
    if ((double) num < 1.7777777910232544)
      ((Control) this).Position = new Vector2(this._offsetX, Mathf.Remap(Ease.CubicOut((float) (((double) num - 1.3333333730697632) / 0.44444441795349121)), 0.0f, 1f, -1540f, -1620f));
    else
      ((Control) this).Position = new Vector2(this._offsetX, -1620f);
    this._drawings.RepositionBasedOnBackground((Control) this);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NMapBg.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapBg.MethodName.OnVisibilityChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapBg.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMapBg.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapBg.MethodName.OnVisibilityChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnVisibilityChanged();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMapBg.MethodName.OnWindowChange) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnWindowChange();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMapBg.MethodName._Ready) || StringName.op_Equality(ref method, NMapBg.MethodName.OnVisibilityChanged) || StringName.op_Equality(ref method, NMapBg.MethodName.OnWindowChange) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapBg.PropertyName._mapTop))
    {
      this._mapTop = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapBg.PropertyName._mapMid))
    {
      this._mapMid = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapBg.PropertyName._mapBot))
    {
      this._mapBot = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapBg.PropertyName._drawings))
    {
      this._drawings = VariantUtils.ConvertTo<NMapDrawings>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapBg.PropertyName._window))
    {
      this._window = VariantUtils.ConvertTo<Window>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapBg.PropertyName._offsetX))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._offsetX = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapBg.PropertyName._mapTop))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._mapTop);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapBg.PropertyName._mapMid))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._mapMid);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapBg.PropertyName._mapBot))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._mapBot);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapBg.PropertyName._drawings))
    {
      value = VariantUtils.CreateFrom<NMapDrawings>(ref this._drawings);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapBg.PropertyName._window))
    {
      value = VariantUtils.CreateFrom<Window>(ref this._window);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapBg.PropertyName._offsetX))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._offsetX);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMapBg.PropertyName._mapTop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapBg.PropertyName._mapMid, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapBg.PropertyName._mapBot, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapBg.PropertyName._drawings, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapBg.PropertyName._window, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMapBg.PropertyName._offsetX, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMapBg.PropertyName._mapTop, Variant.From<TextureRect>(ref this._mapTop));
    info.AddProperty(NMapBg.PropertyName._mapMid, Variant.From<TextureRect>(ref this._mapMid));
    info.AddProperty(NMapBg.PropertyName._mapBot, Variant.From<TextureRect>(ref this._mapBot));
    info.AddProperty(NMapBg.PropertyName._drawings, Variant.From<NMapDrawings>(ref this._drawings));
    info.AddProperty(NMapBg.PropertyName._window, Variant.From<Window>(ref this._window));
    info.AddProperty(NMapBg.PropertyName._offsetX, Variant.From<float>(ref this._offsetX));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMapBg.PropertyName._mapTop, ref variant1))
      this._mapTop = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NMapBg.PropertyName._mapMid, ref variant2))
      this._mapMid = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NMapBg.PropertyName._mapBot, ref variant3))
      this._mapBot = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NMapBg.PropertyName._drawings, ref variant4))
      this._drawings = ((Variant) ref variant4).As<NMapDrawings>();
    Variant variant5;
    if (info.TryGetProperty(NMapBg.PropertyName._window, ref variant5))
      this._window = ((Variant) ref variant5).As<Window>();
    Variant variant6;
    if (!info.TryGetProperty(NMapBg.PropertyName._offsetX, ref variant6))
      return;
    this._offsetX = ((Variant) ref variant6).As<float>();
  }

  public class MethodName : VBoxContainer.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnVisibilityChanged = StringName.op_Implicit(nameof (OnVisibilityChanged));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
  }

  public class PropertyName : VBoxContainer.PropertyName
  {
    public static readonly StringName _mapTop = StringName.op_Implicit(nameof (_mapTop));
    public static readonly StringName _mapMid = StringName.op_Implicit(nameof (_mapMid));
    public static readonly StringName _mapBot = StringName.op_Implicit(nameof (_mapBot));
    public static readonly StringName _drawings = StringName.op_Implicit(nameof (_drawings));
    public static readonly StringName _window = StringName.op_Implicit(nameof (_window));
    public static readonly StringName _offsetX = StringName.op_Implicit(nameof (_offsetX));
  }

  public class SignalName : VBoxContainer.SignalName
  {
  }
}
