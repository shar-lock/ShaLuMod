// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens.NUnlockMiscScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockMiscScreen.cs")]
public class NUnlockMiscScreen : NUnlockScreen
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("timeline_screen/unlock_misc_screen");
  private MegaRichTextLabel _label;
  private string _textToSet;
  private Tween? _tween;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NUnlockMiscScreen._scenePath);
    }
  }

  public static NUnlockMiscScreen Create()
  {
    return PreloadManager.Cache.GetScene(NUnlockMiscScreen._scenePath).Instantiate<NUnlockMiscScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._label = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Label"));
    this._label.Text = this._textToSet;
    ((CanvasItem) this._label).Modulate = StsColors.transparentBlack;
  }

  public override void Open()
  {
    base.Open();
    SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_unlock");
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 1.0).SetDelay(0.25);
  }

  public void SetUnlocks(string text) => this._textToSet = text;

  protected override void OnScreenPreClose() => this._tween?.Kill();

  protected override void OnScreenClose() => NTimelineScreen.Instance.EnableInput();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NUnlockMiscScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockMiscScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockMiscScreen.MethodName.Open, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockMiscScreen.MethodName.SetUnlocks, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NUnlockMiscScreen.MethodName.OnScreenPreClose, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockMiscScreen.MethodName.OnScreenClose, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUnlockMiscScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NUnlockMiscScreen nunlockMiscScreen = NUnlockMiscScreen.Create();
      ret = VariantUtils.CreateFrom<NUnlockMiscScreen>(ref nunlockMiscScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockMiscScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockMiscScreen.MethodName.Open) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Open();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockMiscScreen.MethodName.SetUnlocks) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetUnlocks(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockMiscScreen.MethodName.OnScreenPreClose) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnScreenPreClose();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NUnlockMiscScreen.MethodName.OnScreenClose) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnScreenClose();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUnlockMiscScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NUnlockMiscScreen nunlockMiscScreen = NUnlockMiscScreen.Create();
      ret = VariantUtils.CreateFrom<NUnlockMiscScreen>(ref nunlockMiscScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NUnlockMiscScreen.MethodName.Create) || StringName.op_Equality(ref method, NUnlockMiscScreen.MethodName._Ready) || StringName.op_Equality(ref method, NUnlockMiscScreen.MethodName.Open) || StringName.op_Equality(ref method, NUnlockMiscScreen.MethodName.SetUnlocks) || StringName.op_Equality(ref method, NUnlockMiscScreen.MethodName.OnScreenPreClose) || StringName.op_Equality(ref method, NUnlockMiscScreen.MethodName.OnScreenClose) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockMiscScreen.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockMiscScreen.PropertyName._textToSet))
    {
      this._textToSet = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockMiscScreen.PropertyName._tween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockMiscScreen.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockMiscScreen.PropertyName._textToSet))
    {
      value = VariantUtils.CreateFrom<string>(ref this._textToSet);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockMiscScreen.PropertyName._tween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NUnlockMiscScreen.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NUnlockMiscScreen.PropertyName._textToSet, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockMiscScreen.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NUnlockMiscScreen.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
    info.AddProperty(NUnlockMiscScreen.PropertyName._textToSet, Variant.From<string>(ref this._textToSet));
    info.AddProperty(NUnlockMiscScreen.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NUnlockMiscScreen.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NUnlockMiscScreen.PropertyName._textToSet, ref variant2))
      this._textToSet = ((Variant) ref variant2).As<string>();
    Variant variant3;
    if (!info.TryGetProperty(NUnlockMiscScreen.PropertyName._tween, ref variant3))
      return;
    this._tween = ((Variant) ref variant3).As<Tween>();
  }

  public new class MethodName : NUnlockScreen.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName Open = StringName.op_Implicit(nameof (Open));
    public static readonly StringName SetUnlocks = StringName.op_Implicit(nameof (SetUnlocks));
    public new static readonly StringName OnScreenPreClose = StringName.op_Implicit(nameof (OnScreenPreClose));
    public new static readonly StringName OnScreenClose = StringName.op_Implicit(nameof (OnScreenClose));
  }

  public new class PropertyName : NUnlockScreen.PropertyName
  {
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _textToSet = StringName.op_Implicit(nameof (_textToSet));
    public new static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public new class SignalName : NUnlockScreen.SignalName
  {
  }
}
