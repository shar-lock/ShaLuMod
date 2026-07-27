// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Bestiary.NBestiaryEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;

[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryEntry.cs")]
public class NBestiaryEntry : NButton
{
  private MegaRichTextLabel _label;
  private Control _highlight;
  private TextureRect _underConstructionIcon;
  private Color _defaultColor;
  private Tween? _tween;

  protected override string? HoveredSfx => (string) null;

  private static string ScenePath => SceneHelper.GetScenePath("screens/bestiary/bestiary_entry");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NBestiaryEntry.ScenePath);
    }
  }

  public BestiaryEntry Entry { get; private set; }

  public bool IsDiscovered { get; private set; }

  public bool IsUnderConstruction => ((CanvasItem) this._underConstructionIcon).Visible;

  public static NBestiaryEntry Create(BestiaryEntry entry, bool isDiscovered)
  {
    NBestiaryEntry nbestiaryEntry = PreloadManager.Cache.GetScene(NBestiaryEntry.ScenePath).Instantiate<NBestiaryEntry>((PackedScene.GenEditState) 0L);
    nbestiaryEntry.Entry = entry;
    nbestiaryEntry.IsDiscovered = isDiscovered;
    return nbestiaryEntry;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this.FocusNeighborLeft = new NodePath(".");
    this.FocusNeighborRight = new NodePath(".");
    this._label = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Label"));
    this._highlight = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Highlight"));
    this._underConstructionIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%UnderConstructionIcon"));
    if (!this.IsDiscovered)
    {
      this.Disable();
      this._label.Text = new LocString("bestiary", "UNSEEN.monsterName").GetRawText();
      ((CanvasItem) this._label).SelfModulate = StsColors.gray;
      ((CanvasItem) this._underConstructionIcon).Visible = false;
    }
    else
    {
      this._label.Text = this.Entry.GetEntryTitle();
      Color color;
      switch (this.Entry.roomType)
      {
        case RoomType.Elite:
          color = StsColors.purple;
          break;
        case RoomType.Boss:
          color = StsColors.red;
          break;
        default:
          color = StsColors.cream;
          break;
      }
      this._defaultColor = color;
      ((CanvasItem) this._label).SelfModulate = this._defaultColor;
      ((CanvasItem) this._underConstructionIcon).Visible = false;
    }
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.1f)), 0.05);
    if (!NControllerManager.Instance.IsUsingController)
      return;
    ((CanvasItem) this._highlight).Visible = true;
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    ((CanvasItem) this._highlight).Visible = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NBestiaryEntry.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryEntry.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryEntry.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBestiaryEntry.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiaryEntry.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBestiaryEntry.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBestiaryEntry.MethodName._Ready) || StringName.op_Equality(ref method, NBestiaryEntry.MethodName.OnFocus) || StringName.op_Equality(ref method, NBestiaryEntry.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBestiaryEntry.PropertyName.IsDiscovered))
    {
      this.IsDiscovered = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryEntry.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryEntry.PropertyName._highlight))
    {
      this._highlight = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryEntry.PropertyName._underConstructionIcon))
    {
      this._underConstructionIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryEntry.PropertyName._defaultColor))
    {
      this._defaultColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBestiaryEntry.PropertyName._tween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBestiaryEntry.PropertyName.HoveredSfx))
    {
      ref godot_variant local = ref value;
      string hoveredSfx = this.HoveredSfx;
      godot_variant from = VariantUtils.CreateFrom<string>(ref hoveredSfx);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryEntry.PropertyName.IsDiscovered))
    {
      ref godot_variant local = ref value;
      bool isDiscovered = this.IsDiscovered;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isDiscovered);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryEntry.PropertyName.IsUnderConstruction))
    {
      ref godot_variant local = ref value;
      bool underConstruction = this.IsUnderConstruction;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref underConstruction);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryEntry.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryEntry.PropertyName._highlight))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._highlight);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryEntry.PropertyName._underConstructionIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._underConstructionIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryEntry.PropertyName._defaultColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._defaultColor);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBestiaryEntry.PropertyName._tween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 4L, NBestiaryEntry.PropertyName.HoveredSfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryEntry.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryEntry.PropertyName._highlight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryEntry.PropertyName._underConstructionIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NBestiaryEntry.PropertyName._defaultColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NBestiaryEntry.PropertyName.IsDiscovered, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NBestiaryEntry.PropertyName.IsUnderConstruction, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryEntry.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isDiscovered1 = NBestiaryEntry.PropertyName.IsDiscovered;
    bool isDiscovered2 = this.IsDiscovered;
    Variant variant = Variant.From<bool>(ref isDiscovered2);
    serializationInfo.AddProperty(isDiscovered1, variant);
    info.AddProperty(NBestiaryEntry.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
    info.AddProperty(NBestiaryEntry.PropertyName._highlight, Variant.From<Control>(ref this._highlight));
    info.AddProperty(NBestiaryEntry.PropertyName._underConstructionIcon, Variant.From<TextureRect>(ref this._underConstructionIcon));
    info.AddProperty(NBestiaryEntry.PropertyName._defaultColor, Variant.From<Color>(ref this._defaultColor));
    info.AddProperty(NBestiaryEntry.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBestiaryEntry.PropertyName.IsDiscovered, ref variant1))
      this.IsDiscovered = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NBestiaryEntry.PropertyName._label, ref variant2))
      this._label = ((Variant) ref variant2).As<MegaRichTextLabel>();
    Variant variant3;
    if (info.TryGetProperty(NBestiaryEntry.PropertyName._highlight, ref variant3))
      this._highlight = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NBestiaryEntry.PropertyName._underConstructionIcon, ref variant4))
      this._underConstructionIcon = ((Variant) ref variant4).As<TextureRect>();
    Variant variant5;
    if (info.TryGetProperty(NBestiaryEntry.PropertyName._defaultColor, ref variant5))
      this._defaultColor = ((Variant) ref variant5).As<Color>();
    Variant variant6;
    if (!info.TryGetProperty(NBestiaryEntry.PropertyName._tween, ref variant6))
      return;
    this._tween = ((Variant) ref variant6).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName HoveredSfx = StringName.op_Implicit(nameof (HoveredSfx));
    public static readonly StringName IsDiscovered = StringName.op_Implicit(nameof (IsDiscovered));
    public static readonly StringName IsUnderConstruction = StringName.op_Implicit(nameof (IsUnderConstruction));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _highlight = StringName.op_Implicit(nameof (_highlight));
    public static readonly StringName _underConstructionIcon = StringName.op_Implicit(nameof (_underConstructionIcon));
    public static readonly StringName _defaultColor = StringName.op_Implicit(nameof (_defaultColor));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
