// Decompiled with JetBrains decompiler
// Type: NSoulNexusVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
[ScriptPath("res://src/Core/Nodes/Vfx/NSoulNexusVfx.cs")]
public class NSoulNexusVfx : Node
{
  private MegaSprite _megaSprite;
  private NBasicTrail _trail1;
  private NBasicTrail _trail2;
  private NBasicTrail _trail3;
  private TextureRect _fireTexture;

  public override void _Ready()
  {
    this._megaSprite = new MegaSprite(Variant.op_Implicit((GodotObject) this.GetParent<Node2D>()));
    this._fireTexture = this.GetNode<TextureRect>(NodePath.op_Implicit("../HeadFireSlot/FireTexture"));
    this._trail1 = this.GetNode<NBasicTrail>(NodePath.op_Implicit("../PathSlot1/Trail"));
    this._trail2 = this.GetNode<NBasicTrail>(NodePath.op_Implicit("../PathSlot2/Trail"));
    this._trail3 = this.GetNode<NBasicTrail>(NodePath.op_Implicit("../PathSlot3/Trail"));
    ((CanvasItem) this._trail1).Visible = false;
    ((CanvasItem) this._trail2).Visible = false;
    ((CanvasItem) this._trail3).Visible = false;
    this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    string eventName = new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName();
    if (eventName == null)
      return;
    switch (eventName.Length)
    {
      case 9:
        switch (eventName[0])
        {
          case 'h':
            if (!(eventName == "hide_fire"))
              return;
            this.ShowFire(false);
            return;
          case 's':
            if (!(eventName == "show_fire"))
              return;
            this.ShowFire(true);
            return;
          default:
            return;
        }
      case 11:
        switch (eventName[5])
        {
          case '1':
            if (!(eventName == "path_1_stop"))
              return;
            this.EndPath1();
            return;
          case '2':
            if (!(eventName == "path_2_stop"))
              return;
            this.EndPath2();
            return;
          case '3':
            if (!(eventName == "path_3_stop"))
              return;
            this.EndPath3();
            return;
          default:
            return;
        }
      case 12:
        switch (eventName[5])
        {
          case '1':
            if (!(eventName == "path_1_start"))
              return;
            this.StartPath1();
            return;
          case '2':
            if (!(eventName == "path_2_start"))
              return;
            this.StartPath2();
            return;
          case '3':
            if (!(eventName == "path_3_start"))
              return;
            this.StartPath3();
            return;
          default:
            return;
        }
    }
  }

  private void ShowFire(bool show) => ((CanvasItem) this._fireTexture).Visible = show;

  private void StartPath1()
  {
    ((CanvasItem) this._trail1).Visible = true;
    this._trail1.ClearPoints();
  }

  private void EndPath1() => ((CanvasItem) this._trail1).Visible = false;

  private void StartPath2()
  {
    ((CanvasItem) this._trail2).Visible = true;
    this._trail2.ClearPoints();
  }

  private void EndPath2() => ((CanvasItem) this._trail2).Visible = false;

  private void StartPath3()
  {
    ((CanvasItem) this._trail3).Visible = true;
    this._trail3.ClearPoints();
  }

  private void EndPath3() => ((CanvasItem) this._trail3).Visible = false;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NSoulNexusVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSoulNexusVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSoulNexusVfx.MethodName.ShowFire, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("show"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSoulNexusVfx.MethodName.StartPath1, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSoulNexusVfx.MethodName.EndPath1, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSoulNexusVfx.MethodName.StartPath2, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSoulNexusVfx.MethodName.EndPath2, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSoulNexusVfx.MethodName.StartPath3, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSoulNexusVfx.MethodName.EndPath3, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSoulNexusVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.ShowFire) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ShowFire(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.StartPath1) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartPath1();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.EndPath1) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndPath1();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.StartPath2) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartPath2();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.EndPath2) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndPath2();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.StartPath3) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartPath3();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.EndPath3) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.EndPath3();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSoulNexusVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.ShowFire) || StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.StartPath1) || StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.EndPath1) || StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.StartPath2) || StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.EndPath2) || StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.StartPath3) || StringName.op_Equality(ref method, NSoulNexusVfx.MethodName.EndPath3) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSoulNexusVfx.PropertyName._trail1))
    {
      this._trail1 = VariantUtils.ConvertTo<NBasicTrail>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSoulNexusVfx.PropertyName._trail2))
    {
      this._trail2 = VariantUtils.ConvertTo<NBasicTrail>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSoulNexusVfx.PropertyName._trail3))
    {
      this._trail3 = VariantUtils.ConvertTo<NBasicTrail>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSoulNexusVfx.PropertyName._fireTexture))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._fireTexture = VariantUtils.ConvertTo<TextureRect>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSoulNexusVfx.PropertyName._trail1))
    {
      value = VariantUtils.CreateFrom<NBasicTrail>(ref this._trail1);
      return true;
    }
    if (StringName.op_Equality(ref name, NSoulNexusVfx.PropertyName._trail2))
    {
      value = VariantUtils.CreateFrom<NBasicTrail>(ref this._trail2);
      return true;
    }
    if (StringName.op_Equality(ref name, NSoulNexusVfx.PropertyName._trail3))
    {
      value = VariantUtils.CreateFrom<NBasicTrail>(ref this._trail3);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSoulNexusVfx.PropertyName._fireTexture))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<TextureRect>(ref this._fireTexture);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSoulNexusVfx.PropertyName._trail1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSoulNexusVfx.PropertyName._trail2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSoulNexusVfx.PropertyName._trail3, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSoulNexusVfx.PropertyName._fireTexture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSoulNexusVfx.PropertyName._trail1, Variant.From<NBasicTrail>(ref this._trail1));
    info.AddProperty(NSoulNexusVfx.PropertyName._trail2, Variant.From<NBasicTrail>(ref this._trail2));
    info.AddProperty(NSoulNexusVfx.PropertyName._trail3, Variant.From<NBasicTrail>(ref this._trail3));
    info.AddProperty(NSoulNexusVfx.PropertyName._fireTexture, Variant.From<TextureRect>(ref this._fireTexture));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSoulNexusVfx.PropertyName._trail1, ref variant1))
      this._trail1 = ((Variant) ref variant1).As<NBasicTrail>();
    Variant variant2;
    if (info.TryGetProperty(NSoulNexusVfx.PropertyName._trail2, ref variant2))
      this._trail2 = ((Variant) ref variant2).As<NBasicTrail>();
    Variant variant3;
    if (info.TryGetProperty(NSoulNexusVfx.PropertyName._trail3, ref variant3))
      this._trail3 = ((Variant) ref variant3).As<NBasicTrail>();
    Variant variant4;
    if (!info.TryGetProperty(NSoulNexusVfx.PropertyName._fireTexture, ref variant4))
      return;
    this._fireTexture = ((Variant) ref variant4).As<TextureRect>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName ShowFire = StringName.op_Implicit(nameof (ShowFire));
    public static readonly StringName StartPath1 = StringName.op_Implicit(nameof (StartPath1));
    public static readonly StringName EndPath1 = StringName.op_Implicit(nameof (EndPath1));
    public static readonly StringName StartPath2 = StringName.op_Implicit(nameof (StartPath2));
    public static readonly StringName EndPath2 = StringName.op_Implicit(nameof (EndPath2));
    public static readonly StringName StartPath3 = StringName.op_Implicit(nameof (StartPath3));
    public static readonly StringName EndPath3 = StringName.op_Implicit(nameof (EndPath3));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _trail1 = StringName.op_Implicit(nameof (_trail1));
    public static readonly StringName _trail2 = StringName.op_Implicit(nameof (_trail2));
    public static readonly StringName _trail3 = StringName.op_Implicit(nameof (_trail3));
    public static readonly StringName _fireTexture = StringName.op_Implicit(nameof (_fireTexture));
  }

  public class SignalName : Node.SignalName
  {
  }
}
