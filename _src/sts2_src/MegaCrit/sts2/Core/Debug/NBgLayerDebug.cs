// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.NBgLayerDebug
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

#nullable enable
namespace MegaCrit.Sts2.Core.Debug;

[Tool]
[ScriptPath("res://src/Core/Debug/NBgLayerDebug.cs")]
public class NBgLayerDebug : Control
{
  private const string _layerNodePrefix = "Layer_";
  private PackedScene? _layerA;
  private PackedScene? _layerB;
  private PackedScene? _layerC;
  private NBgLayerDebug.LayerVisibility _visibleLayer;

  [Export]
  public NBgLayerDebug.LayerVisibility VisibleLayer
  {
    get => this._visibleLayer;
    set
    {
      this._visibleLayer = value;
      if (!Engine.IsEditorHint())
        return;
      this.UpdateLayers();
    }
  }

  [ExportToolButton("Reload Layers")]
  private Callable ReloadLayersCallable => Callable.From(new Action(this.ReloadLayers));

  public override void _EnterTree()
  {
    if (!Engine.IsEditorHint())
      return;
    this.ReloadLayers();
  }

  private void ReloadLayers()
  {
    string sceneFilePath = ((Node) this).GetTree().GetEditedSceneRoot().SceneFilePath;
    if (sceneFilePath == null)
      return;
    string str1;
    if (((Node) this).Name.ToString() == "Foreground")
    {
      str1 = "fg";
    }
    else
    {
      string str2 = ((Node) this).Name.ToString();
      int length = "Layer_".Length;
      int result;
      if (!int.TryParse(str2.Substring(length, str2.Length - length), out result))
        return;
      str1 = $"bg_{result:D2}";
    }
    string file = StringExtensions.GetFile(sceneFilePath);
    string str3 = file.Substring(0, file.LastIndexOf('_'));
    string str4 = Path.Combine(StringExtensions.GetBaseDir(sceneFilePath), "layers", $"{str3}_{str1}");
    string str5 = str4 + "_a.tscn";
    string str6 = str4 + "_b.tscn";
    string str7 = str4 + "_c.tscn";
    if (ResourceLoader.Exists(str5, ""))
      this._layerA = ResourceLoader.Load<PackedScene>(str5, (string) null, (ResourceLoader.CacheMode) 1L);
    if (ResourceLoader.Exists(str6, ""))
      this._layerB = ResourceLoader.Load<PackedScene>(str6, (string) null, (ResourceLoader.CacheMode) 1L);
    if (ResourceLoader.Exists(str7, ""))
      this._layerC = ResourceLoader.Load<PackedScene>(str7, (string) null, (ResourceLoader.CacheMode) 1L);
    this.UpdateLayers();
  }

  private void UpdateLayers()
  {
    this.ClearLayers();
    if (this._visibleLayer == NBgLayerDebug.LayerVisibility.A && this._layerA != null)
      this.AddLayer(NBgLayerDebug.LayerVisibility.A, this._layerA);
    if (this._visibleLayer == NBgLayerDebug.LayerVisibility.B && this._layerB != null)
      this.AddLayer(NBgLayerDebug.LayerVisibility.B, this._layerB);
    if (this._visibleLayer != NBgLayerDebug.LayerVisibility.C || this._layerC == null)
      return;
    this.AddLayer(NBgLayerDebug.LayerVisibility.C, this._layerC);
  }

  private void AddLayer(NBgLayerDebug.LayerVisibility name, PackedScene layerScene)
  {
    Control child = layerScene.Instantiate<Control>((PackedScene.GenEditState) 0L);
    ((Node) child).Name = StringName.op_Implicit(NBgLayerDebug.ToLayerName(name));
    ((Node) this).AddChildSafely((Node) child);
  }

  private static string ToLayerName(NBgLayerDebug.LayerVisibility layer) => $"{"Layer_"}{layer}";

  private IEnumerable<Control> GetLayerNodes()
  {
    foreach (Node child in ((Node) this).GetChildren(false))
    {
      if (child.Name.ToString().StartsWith("Layer_"))
        yield return (Control) child;
    }
  }

  private void ClearLayers()
  {
    foreach (Control layerNode in this.GetLayerNodes())
    {
      ((Node) this).RemoveChildSafely((Node) layerNode);
      ((Node) layerNode).QueueFreeSafely();
    }
  }

  public override void _ExitTree() => this.ClearLayers();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NBgLayerDebug.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBgLayerDebug.MethodName.ReloadLayers, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBgLayerDebug.MethodName.UpdateLayers, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBgLayerDebug.MethodName.AddLayer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("name"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("layerScene"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("PackedScene"), false)
      }, (List<Variant>) null),
      new MethodInfo(NBgLayerDebug.MethodName.ToLayerName, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("layer"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NBgLayerDebug.MethodName.ClearLayers, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBgLayerDebug.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBgLayerDebug.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBgLayerDebug.MethodName.ReloadLayers) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ReloadLayers();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBgLayerDebug.MethodName.UpdateLayers) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateLayers();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBgLayerDebug.MethodName.AddLayer) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.AddLayer(VariantUtils.ConvertTo<NBgLayerDebug.LayerVisibility>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<PackedScene>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBgLayerDebug.MethodName.ToLayerName) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string layerName = NBgLayerDebug.ToLayerName(VariantUtils.ConvertTo<NBgLayerDebug.LayerVisibility>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref layerName);
      return true;
    }
    if (StringName.op_Equality(ref method, NBgLayerDebug.MethodName.ClearLayers) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearLayers();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBgLayerDebug.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NBgLayerDebug.MethodName.ToLayerName) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string layerName = NBgLayerDebug.ToLayerName(VariantUtils.ConvertTo<NBgLayerDebug.LayerVisibility>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref layerName);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBgLayerDebug.MethodName._EnterTree) || StringName.op_Equality(ref method, NBgLayerDebug.MethodName.ReloadLayers) || StringName.op_Equality(ref method, NBgLayerDebug.MethodName.UpdateLayers) || StringName.op_Equality(ref method, NBgLayerDebug.MethodName.AddLayer) || StringName.op_Equality(ref method, NBgLayerDebug.MethodName.ToLayerName) || StringName.op_Equality(ref method, NBgLayerDebug.MethodName.ClearLayers) || StringName.op_Equality(ref method, NBgLayerDebug.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBgLayerDebug.PropertyName.VisibleLayer))
    {
      this.VisibleLayer = VariantUtils.ConvertTo<NBgLayerDebug.LayerVisibility>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBgLayerDebug.PropertyName._layerA))
    {
      this._layerA = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBgLayerDebug.PropertyName._layerB))
    {
      this._layerB = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBgLayerDebug.PropertyName._layerC))
    {
      this._layerC = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBgLayerDebug.PropertyName._visibleLayer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._visibleLayer = VariantUtils.ConvertTo<NBgLayerDebug.LayerVisibility>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBgLayerDebug.PropertyName.VisibleLayer))
    {
      ref godot_variant local = ref value;
      NBgLayerDebug.LayerVisibility visibleLayer = this.VisibleLayer;
      godot_variant from = VariantUtils.CreateFrom<NBgLayerDebug.LayerVisibility>(ref visibleLayer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBgLayerDebug.PropertyName.ReloadLayersCallable))
    {
      ref godot_variant local = ref value;
      Callable reloadLayersCallable = this.ReloadLayersCallable;
      godot_variant from = VariantUtils.CreateFrom<Callable>(ref reloadLayersCallable);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBgLayerDebug.PropertyName._layerA))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._layerA);
      return true;
    }
    if (StringName.op_Equality(ref name, NBgLayerDebug.PropertyName._layerB))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._layerB);
      return true;
    }
    if (StringName.op_Equality(ref name, NBgLayerDebug.PropertyName._layerC))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._layerC);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBgLayerDebug.PropertyName._visibleLayer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NBgLayerDebug.LayerVisibility>(ref this._visibleLayer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NBgLayerDebug.PropertyName._layerA, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBgLayerDebug.PropertyName._layerB, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBgLayerDebug.PropertyName._layerC, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NBgLayerDebug.PropertyName._visibleLayer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NBgLayerDebug.PropertyName.VisibleLayer, (PropertyHint) 2L, "A,B,C", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 25L, NBgLayerDebug.PropertyName.ReloadLayersCallable, (PropertyHint) 39L, "Reload Layers", (PropertyUsageFlags) 4L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName visibleLayer1 = NBgLayerDebug.PropertyName.VisibleLayer;
    NBgLayerDebug.LayerVisibility visibleLayer2 = this.VisibleLayer;
    Variant variant = Variant.From<NBgLayerDebug.LayerVisibility>(ref visibleLayer2);
    serializationInfo.AddProperty(visibleLayer1, variant);
    info.AddProperty(NBgLayerDebug.PropertyName._layerA, Variant.From<PackedScene>(ref this._layerA));
    info.AddProperty(NBgLayerDebug.PropertyName._layerB, Variant.From<PackedScene>(ref this._layerB));
    info.AddProperty(NBgLayerDebug.PropertyName._layerC, Variant.From<PackedScene>(ref this._layerC));
    info.AddProperty(NBgLayerDebug.PropertyName._visibleLayer, Variant.From<NBgLayerDebug.LayerVisibility>(ref this._visibleLayer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBgLayerDebug.PropertyName.VisibleLayer, ref variant1))
      this.VisibleLayer = ((Variant) ref variant1).As<NBgLayerDebug.LayerVisibility>();
    Variant variant2;
    if (info.TryGetProperty(NBgLayerDebug.PropertyName._layerA, ref variant2))
      this._layerA = ((Variant) ref variant2).As<PackedScene>();
    Variant variant3;
    if (info.TryGetProperty(NBgLayerDebug.PropertyName._layerB, ref variant3))
      this._layerB = ((Variant) ref variant3).As<PackedScene>();
    Variant variant4;
    if (info.TryGetProperty(NBgLayerDebug.PropertyName._layerC, ref variant4))
      this._layerC = ((Variant) ref variant4).As<PackedScene>();
    Variant variant5;
    if (!info.TryGetProperty(NBgLayerDebug.PropertyName._visibleLayer, ref variant5))
      return;
    this._visibleLayer = ((Variant) ref variant5).As<NBgLayerDebug.LayerVisibility>();
  }

  public enum LayerVisibility
  {
    A,
    B,
    C,
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName ReloadLayers = StringName.op_Implicit(nameof (ReloadLayers));
    public static readonly StringName UpdateLayers = StringName.op_Implicit(nameof (UpdateLayers));
    public static readonly StringName AddLayer = StringName.op_Implicit(nameof (AddLayer));
    public static readonly StringName ToLayerName = StringName.op_Implicit(nameof (ToLayerName));
    public static readonly StringName ClearLayers = StringName.op_Implicit(nameof (ClearLayers));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName VisibleLayer = StringName.op_Implicit(nameof (VisibleLayer));
    public static readonly StringName ReloadLayersCallable = StringName.op_Implicit(nameof (ReloadLayersCallable));
    public static readonly StringName _layerA = StringName.op_Implicit(nameof (_layerA));
    public static readonly StringName _layerB = StringName.op_Implicit(nameof (_layerB));
    public static readonly StringName _layerC = StringName.op_Implicit(nameof (_layerC));
    public static readonly StringName _visibleLayer = StringName.op_Implicit(nameof (_visibleLayer));
  }

  public class SignalName : Control.SignalName
  {
  }
}
