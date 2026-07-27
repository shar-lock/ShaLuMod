// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NOpenProfileScreenButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NOpenProfileScreenButton.cs")]
public class NOpenProfileScreenButton : NButton
{
  private readonly LocString _titleLoc = new LocString("main_menu_ui", "OPEN_PROFILE_SCREEN.title");
  private static readonly LocString _descriptionLoc = new LocString("main_menu_ui", "OPEN_PROFILE_SCREEN.description");
  private NProfileIcon _profileIcon;
  private MegaLabel _title;
  private MegaLabel _description;
  private Tween? _tween;

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.pauseAndBack)
      };
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._profileIcon = ((Node) this).GetNode<NProfileIcon>(NodePath.op_Implicit("ProfileIcon"));
    this._title = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Title"));
    this._description = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Description"));
    this.RefreshLabels();
    this._profileIcon.SetProfileId(SaveManager.Instance.CurrentProfileId);
    this.UpdateDescription();
  }

  public override void _EnterTree()
  {
    base._EnterTree();
    if (NControllerManager.Instance == null)
      return;
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateDescription)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateDescription)), 0U);
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    if (NControllerManager.Instance == null)
      return;
    ((GodotObject) NControllerManager.Instance).Disconnect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateDescription)));
    ((GodotObject) NControllerManager.Instance).Disconnect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateDescription)));
  }

  public override void _Notification(int what)
  {
    if (what != 2010 || !((Node) this).IsNodeReady())
      return;
    this.RefreshLabels();
  }

  private void RefreshLabels()
  {
    this._titleLoc.Add("Id", (Decimal) SaveManager.Instance.CurrentProfileId);
    this._title.SetTextAutoSize(this._titleLoc.GetFormattedText());
    this._description.SetTextAutoSize(NOpenProfileScreenButton._descriptionLoc.GetFormattedText());
  }

  protected override void OnRelease() => NGame.Instance.MainMenu.OpenProfileScreen();

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this.Scale = Vector2.op_Multiply(Vector2.One, 1.02f);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1f)), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void UpdateDescription()
  {
    if (NControllerManager.Instance == null)
      return;
    ((CanvasItem) this._description).SetVisible(!NControllerManager.Instance.IsUsingController);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NOpenProfileScreenButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOpenProfileScreenButton.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOpenProfileScreenButton.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOpenProfileScreenButton.MethodName._Notification, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("what"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NOpenProfileScreenButton.MethodName.RefreshLabels, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOpenProfileScreenButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOpenProfileScreenButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOpenProfileScreenButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOpenProfileScreenButton.MethodName.UpdateDescription, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName._Notification) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((GodotObject) this)._Notification(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName.RefreshLabels) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshLabels();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName.UpdateDescription) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateDescription();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName._Ready) || StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName._EnterTree) || StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName._ExitTree) || StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName._Notification) || StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName.RefreshLabels) || StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NOpenProfileScreenButton.MethodName.UpdateDescription) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NOpenProfileScreenButton.PropertyName._profileIcon))
    {
      this._profileIcon = VariantUtils.ConvertTo<NProfileIcon>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOpenProfileScreenButton.PropertyName._title))
    {
      this._title = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOpenProfileScreenButton.PropertyName._description))
    {
      this._description = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NOpenProfileScreenButton.PropertyName._tween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NOpenProfileScreenButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NOpenProfileScreenButton.PropertyName._profileIcon))
    {
      value = VariantUtils.CreateFrom<NProfileIcon>(ref this._profileIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NOpenProfileScreenButton.PropertyName._title))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._title);
      return true;
    }
    if (StringName.op_Equality(ref name, NOpenProfileScreenButton.PropertyName._description))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._description);
      return true;
    }
    if (!StringName.op_Equality(ref name, NOpenProfileScreenButton.PropertyName._tween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NOpenProfileScreenButton.PropertyName._profileIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOpenProfileScreenButton.PropertyName._title, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOpenProfileScreenButton.PropertyName._description, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOpenProfileScreenButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NOpenProfileScreenButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NOpenProfileScreenButton.PropertyName._profileIcon, Variant.From<NProfileIcon>(ref this._profileIcon));
    info.AddProperty(NOpenProfileScreenButton.PropertyName._title, Variant.From<MegaLabel>(ref this._title));
    info.AddProperty(NOpenProfileScreenButton.PropertyName._description, Variant.From<MegaLabel>(ref this._description));
    info.AddProperty(NOpenProfileScreenButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NOpenProfileScreenButton.PropertyName._profileIcon, ref variant1))
      this._profileIcon = ((Variant) ref variant1).As<NProfileIcon>();
    Variant variant2;
    if (info.TryGetProperty(NOpenProfileScreenButton.PropertyName._title, ref variant2))
      this._title = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NOpenProfileScreenButton.PropertyName._description, ref variant3))
      this._description = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (!info.TryGetProperty(NOpenProfileScreenButton.PropertyName._tween, ref variant4))
      return;
    this._tween = ((Variant) ref variant4).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Notification = StringName.op_Implicit(nameof (_Notification));
    public static readonly StringName RefreshLabels = StringName.op_Implicit(nameof (RefreshLabels));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName UpdateDescription = StringName.op_Implicit(nameof (UpdateDescription));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _profileIcon = StringName.op_Implicit(nameof (_profileIcon));
    public static readonly StringName _title = StringName.op_Implicit(nameof (_title));
    public static readonly StringName _description = StringName.op_Implicit(nameof (_description));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
