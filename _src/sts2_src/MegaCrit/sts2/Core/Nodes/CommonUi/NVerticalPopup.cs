// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NVerticalPopup
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NVerticalPopup.cs")]
public class NVerticalPopup : Control
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("ui/vertical_popup");
  private bool _nodesAreSet;
  private Callable? _yesCallable;
  private Callable? _noCallable;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NVerticalPopup._scenePath);
    }
  }

  private MegaLabel TitleLabel { get; set; }

  private MegaRichTextLabel BodyLabel { get; set; }

  public NPopupYesNoButton YesButton { get; private set; }

  public NPopupYesNoButton NoButton { get; private set; }

  public override void _Ready() => this.EnsureNodesAreSet();

  private void EnsureNodesAreSet()
  {
    if (this._nodesAreSet)
      return;
    this.TitleLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Header"));
    this.BodyLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("Description"));
    this.YesButton = ((Node) this).GetNode<NPopupYesNoButton>(NodePath.op_Implicit("YesButton"));
    this.NoButton = ((Node) this).GetNode<NPopupYesNoButton>(NodePath.op_Implicit("NoButton"));
    this._nodesAreSet = true;
  }

  public void SetText(LocString title, LocString body)
  {
    this.EnsureNodesAreSet();
    this.TitleLabel.SetTextAutoSize(title.GetFormattedText());
    this.BodyLabel.SetTextAutoSize(body.GetFormattedText());
  }

  public void SetText(string title, string body)
  {
    this.EnsureNodesAreSet();
    this.TitleLabel.SetTextAutoSize(title);
    this.BodyLabel.SetTextAutoSize(body);
  }

  public void InitYesButton(LocString yesButton, Action<NButton> onPressed)
  {
    this.EnsureNodesAreSet();
    this._yesCallable = new Callable?(Callable.From<NButton>(onPressed));
    this.YesButton.IsYes = true;
    this.YesButton.SetText(yesButton.GetFormattedText());
    ((GodotObject) this.YesButton).Connect(NClickableControl.SignalName.Released, this._yesCallable.Value, 0U);
    ((GodotObject) this.YesButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.Close)), 0U);
  }

  public void InitNoButton(LocString noButton, Action<NButton> onPressed)
  {
    this.EnsureNodesAreSet();
    this._noCallable = new Callable?(Callable.From<NButton>(onPressed));
    ((CanvasItem) this.NoButton).Visible = true;
    this.NoButton.IsYes = false;
    this.NoButton.SetText(noButton.GetFormattedText());
    ((GodotObject) this.NoButton).Connect(NClickableControl.SignalName.Released, this._noCallable.Value, 0U);
    ((GodotObject) this.NoButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.Close)), 0U);
  }

  private void Close(NButton _) => NModalContainer.Instance.Clear();

  public void HideNoButton() => ((CanvasItem) this.NoButton).Visible = false;

  public void DisconnectSignals()
  {
    if (this._yesCallable.HasValue)
    {
      ((GodotObject) this.YesButton).Disconnect(NClickableControl.SignalName.Released, this._yesCallable.Value);
      ((GodotObject) this.YesButton).Disconnect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.Close)));
    }
    if (!this._noCallable.HasValue)
      return;
    ((GodotObject) this.NoButton).Disconnect(NClickableControl.SignalName.Released, this._noCallable.Value);
    ((GodotObject) this.NoButton).Disconnect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.Close)));
  }

  public void DisconnectHotkeys()
  {
    if (this._yesCallable.HasValue)
      this.YesButton.DisconnectHotkeys();
    if (!this._noCallable.HasValue)
      return;
    this.NoButton.DisconnectHotkeys();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NVerticalPopup.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVerticalPopup.MethodName.EnsureNodesAreSet, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVerticalPopup.MethodName.SetText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("title"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("body"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NVerticalPopup.MethodName.Close, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NVerticalPopup.MethodName.HideNoButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVerticalPopup.MethodName.DisconnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVerticalPopup.MethodName.DisconnectHotkeys, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NVerticalPopup.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVerticalPopup.MethodName.EnsureNodesAreSet) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnsureNodesAreSet();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVerticalPopup.MethodName.SetText) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.SetText(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVerticalPopup.MethodName.Close) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.Close(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVerticalPopup.MethodName.HideNoButton) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideNoButton();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVerticalPopup.MethodName.DisconnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisconnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NVerticalPopup.MethodName.DisconnectHotkeys) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.DisconnectHotkeys();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NVerticalPopup.MethodName._Ready) || StringName.op_Equality(ref method, NVerticalPopup.MethodName.EnsureNodesAreSet) || StringName.op_Equality(ref method, NVerticalPopup.MethodName.SetText) || StringName.op_Equality(ref method, NVerticalPopup.MethodName.Close) || StringName.op_Equality(ref method, NVerticalPopup.MethodName.HideNoButton) || StringName.op_Equality(ref method, NVerticalPopup.MethodName.DisconnectSignals) || StringName.op_Equality(ref method, NVerticalPopup.MethodName.DisconnectHotkeys) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NVerticalPopup.PropertyName.TitleLabel))
    {
      this.TitleLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVerticalPopup.PropertyName.BodyLabel))
    {
      this.BodyLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVerticalPopup.PropertyName.YesButton))
    {
      this.YesButton = VariantUtils.ConvertTo<NPopupYesNoButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVerticalPopup.PropertyName.NoButton))
    {
      this.NoButton = VariantUtils.ConvertTo<NPopupYesNoButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NVerticalPopup.PropertyName._nodesAreSet))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._nodesAreSet = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NVerticalPopup.PropertyName.TitleLabel))
    {
      ref godot_variant local = ref value;
      MegaLabel titleLabel = this.TitleLabel;
      godot_variant from = VariantUtils.CreateFrom<MegaLabel>(ref titleLabel);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NVerticalPopup.PropertyName.BodyLabel))
    {
      ref godot_variant local = ref value;
      MegaRichTextLabel bodyLabel = this.BodyLabel;
      godot_variant from = VariantUtils.CreateFrom<MegaRichTextLabel>(ref bodyLabel);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NVerticalPopup.PropertyName.YesButton))
    {
      ref godot_variant local = ref value;
      NPopupYesNoButton yesButton = this.YesButton;
      godot_variant from = VariantUtils.CreateFrom<NPopupYesNoButton>(ref yesButton);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NVerticalPopup.PropertyName.NoButton))
    {
      ref godot_variant local = ref value;
      NPopupYesNoButton noButton = this.NoButton;
      godot_variant from = VariantUtils.CreateFrom<NPopupYesNoButton>(ref noButton);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NVerticalPopup.PropertyName._nodesAreSet))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._nodesAreSet);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NVerticalPopup.PropertyName.TitleLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NVerticalPopup.PropertyName.BodyLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NVerticalPopup.PropertyName.YesButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NVerticalPopup.PropertyName.NoButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NVerticalPopup.PropertyName._nodesAreSet, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName titleLabel1 = NVerticalPopup.PropertyName.TitleLabel;
    MegaLabel titleLabel2 = this.TitleLabel;
    Variant variant1 = Variant.From<MegaLabel>(ref titleLabel2);
    serializationInfo1.AddProperty(titleLabel1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName bodyLabel1 = NVerticalPopup.PropertyName.BodyLabel;
    MegaRichTextLabel bodyLabel2 = this.BodyLabel;
    Variant variant2 = Variant.From<MegaRichTextLabel>(ref bodyLabel2);
    serializationInfo2.AddProperty(bodyLabel1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName yesButton1 = NVerticalPopup.PropertyName.YesButton;
    NPopupYesNoButton yesButton2 = this.YesButton;
    Variant variant3 = Variant.From<NPopupYesNoButton>(ref yesButton2);
    serializationInfo3.AddProperty(yesButton1, variant3);
    GodotSerializationInfo serializationInfo4 = info;
    StringName noButton1 = NVerticalPopup.PropertyName.NoButton;
    NPopupYesNoButton noButton2 = this.NoButton;
    Variant variant4 = Variant.From<NPopupYesNoButton>(ref noButton2);
    serializationInfo4.AddProperty(noButton1, variant4);
    info.AddProperty(NVerticalPopup.PropertyName._nodesAreSet, Variant.From<bool>(ref this._nodesAreSet));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NVerticalPopup.PropertyName.TitleLabel, ref variant1))
      this.TitleLabel = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NVerticalPopup.PropertyName.BodyLabel, ref variant2))
      this.BodyLabel = ((Variant) ref variant2).As<MegaRichTextLabel>();
    Variant variant3;
    if (info.TryGetProperty(NVerticalPopup.PropertyName.YesButton, ref variant3))
      this.YesButton = ((Variant) ref variant3).As<NPopupYesNoButton>();
    Variant variant4;
    if (info.TryGetProperty(NVerticalPopup.PropertyName.NoButton, ref variant4))
      this.NoButton = ((Variant) ref variant4).As<NPopupYesNoButton>();
    Variant variant5;
    if (!info.TryGetProperty(NVerticalPopup.PropertyName._nodesAreSet, ref variant5))
      return;
    this._nodesAreSet = ((Variant) ref variant5).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName EnsureNodesAreSet = StringName.op_Implicit(nameof (EnsureNodesAreSet));
    public static readonly StringName SetText = StringName.op_Implicit(nameof (SetText));
    public static readonly StringName Close = StringName.op_Implicit(nameof (Close));
    public static readonly StringName HideNoButton = StringName.op_Implicit(nameof (HideNoButton));
    public static readonly StringName DisconnectSignals = StringName.op_Implicit(nameof (DisconnectSignals));
    public static readonly StringName DisconnectHotkeys = StringName.op_Implicit(nameof (DisconnectHotkeys));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName TitleLabel = StringName.op_Implicit(nameof (TitleLabel));
    public static readonly StringName BodyLabel = StringName.op_Implicit(nameof (BodyLabel));
    public static readonly StringName YesButton = StringName.op_Implicit(nameof (YesButton));
    public static readonly StringName NoButton = StringName.op_Implicit(nameof (NoButton));
    public static readonly StringName _nodesAreSet = StringName.op_Implicit(nameof (_nodesAreSet));
  }

  public class SignalName : Control.SignalName
  {
  }
}
