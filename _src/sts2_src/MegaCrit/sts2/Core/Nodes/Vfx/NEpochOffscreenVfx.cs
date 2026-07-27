// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NEpochOffscreenVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NEpochOffscreenVfx.cs")]
public class NEpochOffscreenVfx : Control
{
  public static readonly string scenePath = SceneHelper.GetScenePath("timeline_screen/epoch_offscreen_vfx");
  private NEpochSlot _slot;
  private Tween? _tween;
  private bool _showVfx;
  private float _viewportSizeX;

  public static NEpochOffscreenVfx Create(NEpochSlot slot)
  {
    NEpochOffscreenVfx nepochOffscreenVfx = PreloadManager.Cache.GetScene(NEpochOffscreenVfx.scenePath).Instantiate<NEpochOffscreenVfx>((PackedScene.GenEditState) 0L);
    nepochOffscreenVfx._slot = slot;
    return nepochOffscreenVfx;
  }

  public override void _Ready()
  {
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    this._viewportSizeX = ((Rect2) ref viewportRect).Size.X;
  }

  public override void _Process(double delta)
  {
    if (!this._showVfx)
    {
      if ((double) this._slot.GlobalPosition.X < 0.0)
      {
        this._showVfx = true;
        this._tween?.Kill();
        this._tween = ((Node) this).CreateTween();
        this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.75f), 0.5);
        this.GlobalPosition = new Vector2(0.0f, this._slot.GlobalPosition.Y + 60f);
      }
      else
      {
        if ((double) this._slot.GlobalPosition.X <= (double) this._viewportSizeX)
          return;
        this._showVfx = true;
        this._tween?.Kill();
        this._tween = ((Node) this).CreateTween();
        this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.75f), 0.5);
        this.GlobalPosition = new Vector2(this._viewportSizeX, this._slot.GlobalPosition.Y + 60f);
      }
    }
    else
    {
      if ((double) this._slot.GlobalPosition.X <= 0.0 || (double) this._slot.GlobalPosition.X >= (double) this._viewportSizeX)
        return;
      this._showVfx = false;
      this._tween?.Kill();
      this._tween = ((Node) this).CreateTween();
      this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.2);
    }
  }

  public override void _ExitTree() => this._tween?.Kill();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NEpochOffscreenVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("slot"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NEpochOffscreenVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEpochOffscreenVfx.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEpochOffscreenVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEpochOffscreenVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NEpochOffscreenVfx nepochOffscreenVfx = NEpochOffscreenVfx.Create(VariantUtils.ConvertTo<NEpochSlot>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NEpochOffscreenVfx>(ref nepochOffscreenVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochOffscreenVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEpochOffscreenVfx.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEpochOffscreenVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEpochOffscreenVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NEpochOffscreenVfx nepochOffscreenVfx = NEpochOffscreenVfx.Create(VariantUtils.ConvertTo<NEpochSlot>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NEpochOffscreenVfx>(ref nepochOffscreenVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEpochOffscreenVfx.MethodName.Create) || StringName.op_Equality(ref method, NEpochOffscreenVfx.MethodName._Ready) || StringName.op_Equality(ref method, NEpochOffscreenVfx.MethodName._Process) || StringName.op_Equality(ref method, NEpochOffscreenVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEpochOffscreenVfx.PropertyName._slot))
    {
      this._slot = VariantUtils.ConvertTo<NEpochSlot>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochOffscreenVfx.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochOffscreenVfx.PropertyName._showVfx))
    {
      this._showVfx = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEpochOffscreenVfx.PropertyName._viewportSizeX))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._viewportSizeX = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEpochOffscreenVfx.PropertyName._slot))
    {
      value = VariantUtils.CreateFrom<NEpochSlot>(ref this._slot);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochOffscreenVfx.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEpochOffscreenVfx.PropertyName._showVfx))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._showVfx);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEpochOffscreenVfx.PropertyName._viewportSizeX))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._viewportSizeX);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NEpochOffscreenVfx.PropertyName._slot, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEpochOffscreenVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEpochOffscreenVfx.PropertyName._showVfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NEpochOffscreenVfx.PropertyName._viewportSizeX, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NEpochOffscreenVfx.PropertyName._slot, Variant.From<NEpochSlot>(ref this._slot));
    info.AddProperty(NEpochOffscreenVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NEpochOffscreenVfx.PropertyName._showVfx, Variant.From<bool>(ref this._showVfx));
    info.AddProperty(NEpochOffscreenVfx.PropertyName._viewportSizeX, Variant.From<float>(ref this._viewportSizeX));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEpochOffscreenVfx.PropertyName._slot, ref variant1))
      this._slot = ((Variant) ref variant1).As<NEpochSlot>();
    Variant variant2;
    if (info.TryGetProperty(NEpochOffscreenVfx.PropertyName._tween, ref variant2))
      this._tween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (info.TryGetProperty(NEpochOffscreenVfx.PropertyName._showVfx, ref variant3))
      this._showVfx = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (!info.TryGetProperty(NEpochOffscreenVfx.PropertyName._viewportSizeX, ref variant4))
      return;
    this._viewportSizeX = ((Variant) ref variant4).As<float>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _slot = StringName.op_Implicit(nameof (_slot));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _showVfx = StringName.op_Implicit(nameof (_showVfx));
    public static readonly StringName _viewportSizeX = StringName.op_Implicit(nameof (_viewportSizeX));
  }

  public class SignalName : Control.SignalName
  {
  }
}
