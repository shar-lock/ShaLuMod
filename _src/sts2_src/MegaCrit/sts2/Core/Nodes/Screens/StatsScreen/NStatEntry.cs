// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen.NStatEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;

[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NStatEntry.cs")]
public class NStatEntry : NClickableControl
{
  private HoverTip? _hoverTip;
  private Tween? _tween;
  private string? _imgUrl;
  private TextureRect _icon;
  private MegaRichTextLabel _topLabel;
  private MegaRichTextLabel _bottomLabel;
  private NSelectionReticle _controllerFocusReticle;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/stats_screen/stats_screen_section");
  }

  public static NStatEntry Create(string imgUrl)
  {
    NStatEntry nstatEntry = PreloadManager.Cache.GetScene(NStatEntry.ScenePath).Instantiate<NStatEntry>((PackedScene.GenEditState) 0L);
    nstatEntry._imgUrl = imgUrl;
    return nstatEntry;
  }

  public void SetTopText(string text)
  {
    ((CanvasItem) this._topLabel).Visible = true;
    this._topLabel.SetTextAutoSize(text);
  }

  public void SetBottomText(string text)
  {
    ((CanvasItem) this._bottomLabel).Visible = true;
    this._bottomLabel.SetTextAutoSize(text);
  }

  public override void _Ready()
  {
    this.SetPivotOffset(new Vector2(50f, this.Size.Y * 0.5f));
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Icon"));
    if (this._imgUrl != null)
      this._icon.Texture = PreloadManager.Cache.GetTexture2D(this._imgUrl);
    this._topLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%TopLabel"));
    this._bottomLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%BottomLabel"));
    this._controllerFocusReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%SelectionReticle"));
    this.ConnectSignals();
  }

  public void SetHoverTip(HoverTip hoverTip) => this._hoverTip = new HoverTip?(hoverTip);

  protected override void OnFocus()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.05f)), 0.05);
    if (this._hoverTip.HasValue)
    {
      double x = (double) this.GlobalPosition.X;
      Rect2 visibleRect = ((Node) this).GetViewport().GetVisibleRect();
      double num = (double) ((Rect2) ref visibleRect).Size.X * 0.40000000596046448;
      if (x < num)
        NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) this._hoverTip)?.SetGlobalPosition(new Vector2(this.GlobalPosition.X - 392f, this.GlobalPosition.Y), false);
      else
        NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) this._hoverTip)?.SetGlobalPosition(new Vector2(this.GlobalPosition.X + 532f, this.GlobalPosition.Y), false);
    }
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._controllerFocusReticle.OnSelect();
  }

  protected override void OnUnfocus()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    if (this._hoverTip.HasValue)
      NHoverTipSet.Remove((Control) this);
    this._controllerFocusReticle.OnDeselect();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NStatEntry.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("imgUrl"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NStatEntry.MethodName.SetTopText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NStatEntry.MethodName.SetBottomText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NStatEntry.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStatEntry.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStatEntry.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NStatEntry.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NStatEntry nstatEntry = NStatEntry.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NStatEntry>(ref nstatEntry);
      return true;
    }
    if (StringName.op_Equality(ref method, NStatEntry.MethodName.SetTopText) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetTopText(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStatEntry.MethodName.SetBottomText) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetBottomText(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStatEntry.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStatEntry.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NStatEntry.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NStatEntry.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NStatEntry nstatEntry = NStatEntry.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NStatEntry>(ref nstatEntry);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NStatEntry.MethodName.Create) || StringName.op_Equality(ref method, NStatEntry.MethodName.SetTopText) || StringName.op_Equality(ref method, NStatEntry.MethodName.SetBottomText) || StringName.op_Equality(ref method, NStatEntry.MethodName._Ready) || StringName.op_Equality(ref method, NStatEntry.MethodName.OnFocus) || StringName.op_Equality(ref method, NStatEntry.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NStatEntry.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatEntry.PropertyName._imgUrl))
    {
      this._imgUrl = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatEntry.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatEntry.PropertyName._topLabel))
    {
      this._topLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatEntry.PropertyName._bottomLabel))
    {
      this._bottomLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NStatEntry.PropertyName._controllerFocusReticle))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._controllerFocusReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NStatEntry.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatEntry.PropertyName._imgUrl))
    {
      value = VariantUtils.CreateFrom<string>(ref this._imgUrl);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatEntry.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatEntry.PropertyName._topLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._topLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatEntry.PropertyName._bottomLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._bottomLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NStatEntry.PropertyName._controllerFocusReticle))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._controllerFocusReticle);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NStatEntry.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NStatEntry.PropertyName._imgUrl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStatEntry.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStatEntry.PropertyName._topLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStatEntry.PropertyName._bottomLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStatEntry.PropertyName._controllerFocusReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NStatEntry.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NStatEntry.PropertyName._imgUrl, Variant.From<string>(ref this._imgUrl));
    info.AddProperty(NStatEntry.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NStatEntry.PropertyName._topLabel, Variant.From<MegaRichTextLabel>(ref this._topLabel));
    info.AddProperty(NStatEntry.PropertyName._bottomLabel, Variant.From<MegaRichTextLabel>(ref this._bottomLabel));
    info.AddProperty(NStatEntry.PropertyName._controllerFocusReticle, Variant.From<NSelectionReticle>(ref this._controllerFocusReticle));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NStatEntry.PropertyName._tween, ref variant1))
      this._tween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (info.TryGetProperty(NStatEntry.PropertyName._imgUrl, ref variant2))
      this._imgUrl = ((Variant) ref variant2).As<string>();
    Variant variant3;
    if (info.TryGetProperty(NStatEntry.PropertyName._icon, ref variant3))
      this._icon = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NStatEntry.PropertyName._topLabel, ref variant4))
      this._topLabel = ((Variant) ref variant4).As<MegaRichTextLabel>();
    Variant variant5;
    if (info.TryGetProperty(NStatEntry.PropertyName._bottomLabel, ref variant5))
      this._bottomLabel = ((Variant) ref variant5).As<MegaRichTextLabel>();
    Variant variant6;
    if (!info.TryGetProperty(NStatEntry.PropertyName._controllerFocusReticle, ref variant6))
      return;
    this._controllerFocusReticle = ((Variant) ref variant6).As<NSelectionReticle>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName SetTopText = StringName.op_Implicit(nameof (SetTopText));
    public static readonly StringName SetBottomText = StringName.op_Implicit(nameof (SetBottomText));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _imgUrl = StringName.op_Implicit(nameof (_imgUrl));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _topLabel = StringName.op_Implicit(nameof (_topLabel));
    public static readonly StringName _bottomLabel = StringName.op_Implicit(nameof (_bottomLabel));
    public static readonly StringName _controllerFocusReticle = StringName.op_Implicit(nameof (_controllerFocusReticle));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
