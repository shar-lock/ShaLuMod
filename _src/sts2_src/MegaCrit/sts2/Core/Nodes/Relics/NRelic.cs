// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Relics.NRelic
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Relics;

[ScriptPath("res://src/Core/Nodes/Relics/NRelic.cs")]
public class NRelic : Control
{
  public const string relicMatPath = "res://materials/ui/relic_mat.tres";
  private static readonly string _scenePath = SceneHelper.GetScenePath("relics/relic");
  private RelicModel? _model;
  private NRelic.IconSize _iconSize;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        "res://materials/ui/relic_mat.tres",
        NRelic._scenePath
      });
    }
  }

  public TextureRect Icon { get; private set; }

  public TextureRect Outline { get; private set; }

  public event Action<RelicModel?, RelicModel?>? ModelChanged;

  public RelicModel Model
  {
    get
    {
      return this._model ?? throw new InvalidOperationException("Model was accessed before it was set.");
    }
    set
    {
      if (this._model != value)
      {
        RelicModel model = this._model;
        this._model = value;
        Action<RelicModel, RelicModel> modelChanged = this.ModelChanged;
        if (modelChanged != null)
          modelChanged(model, this._model);
      }
      this.Reload();
    }
  }

  public static NRelic? Create(RelicModel relic, NRelic.IconSize iconSize)
  {
    if (TestMode.IsOn)
      return (NRelic) null;
    NRelic nrelic = PreloadManager.Cache.GetScene(NRelic._scenePath).Instantiate<NRelic>((PackedScene.GenEditState) 0L);
    ((Node) nrelic).Name = StringName.op_Implicit($"NRelic-{relic.Id}");
    nrelic.Model = relic;
    nrelic._iconSize = iconSize;
    return nrelic;
  }

  public override void _Ready()
  {
    this.Icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Icon"));
    this.Outline = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Outline"));
    this.Reload();
  }

  private void Reload()
  {
    if (!((Node) this).IsNodeReady() || this._model == null)
      return;
    this.Model.UpdateTexture(this.Icon);
    switch (this._iconSize)
    {
      case NRelic.IconSize.Small:
        this.Icon.Texture = this.Model.Icon;
        ((CanvasItem) this.Outline).Visible = true;
        this.Outline.Texture = this.Model.IconOutline;
        break;
      case NRelic.IconSize.Large:
        this.Icon.Texture = this.Model.BigIcon;
        ((CanvasItem) this.Outline).Visible = false;
        break;
      default:
        throw new ArgumentOutOfRangeException("_iconSize", (object) this._iconSize, (string) null);
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NRelic.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelic.MethodName.Reload, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRelic.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRelic.MethodName.Reload) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.Reload();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRelic.MethodName._Ready) || StringName.op_Equality(ref method, NRelic.MethodName.Reload) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelic.PropertyName.Icon))
    {
      this.Icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelic.PropertyName.Outline))
    {
      this.Outline = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelic.PropertyName._iconSize))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._iconSize = VariantUtils.ConvertTo<NRelic.IconSize>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelic.PropertyName.Icon))
    {
      ref godot_variant local = ref value;
      TextureRect icon = this.Icon;
      godot_variant from = VariantUtils.CreateFrom<TextureRect>(ref icon);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRelic.PropertyName.Outline))
    {
      ref godot_variant local = ref value;
      TextureRect outline = this.Outline;
      godot_variant from = VariantUtils.CreateFrom<TextureRect>(ref outline);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelic.PropertyName._iconSize))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NRelic.IconSize>(ref this._iconSize);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRelic.PropertyName.Icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelic.PropertyName.Outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NRelic.PropertyName._iconSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName icon1 = NRelic.PropertyName.Icon;
    TextureRect icon2 = this.Icon;
    Variant variant1 = Variant.From<TextureRect>(ref icon2);
    serializationInfo1.AddProperty(icon1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName outline1 = NRelic.PropertyName.Outline;
    TextureRect outline2 = this.Outline;
    Variant variant2 = Variant.From<TextureRect>(ref outline2);
    serializationInfo2.AddProperty(outline1, variant2);
    info.AddProperty(NRelic.PropertyName._iconSize, Variant.From<NRelic.IconSize>(ref this._iconSize));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRelic.PropertyName.Icon, ref variant1))
      this.Icon = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NRelic.PropertyName.Outline, ref variant2))
      this.Outline = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (!info.TryGetProperty(NRelic.PropertyName._iconSize, ref variant3))
      return;
    this._iconSize = ((Variant) ref variant3).As<NRelic.IconSize>();
  }

  public enum IconSize
  {
    Small,
    Large,
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Reload = StringName.op_Implicit(nameof (Reload));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Icon = StringName.op_Implicit(nameof (Icon));
    public static readonly StringName Outline = StringName.op_Implicit(nameof (Outline));
    public static readonly StringName _iconSize = StringName.op_Implicit(nameof (_iconSize));
  }

  public class SignalName : Control.SignalName
  {
  }
}
