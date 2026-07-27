// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.NEraColumn
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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Timeline;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NEraColumn.cs")]
public class NEraColumn : Control
{
  private const string _scenePath = "res://scenes/timeline_screen/era_column.tscn";
  public static readonly IEnumerable<string> assetPaths;
  private TextureRect _icon;
  private MegaLabel _name;
  private MegaLabel _year;
  private Tween _iconTween;
  private Tween _labelTween;
  private bool _labelSpawned;
  public EpochEra era;
  private Vector2 _prevLocalPos;
  private Vector2 _prevGlobalPos;
  private Vector2 _predictedPosition;
  private Vector2 _targetPosition;
  private bool _isAnimated;
  private EpochSlotData _data;

  public static NEraColumn Create(EpochSlotData data)
  {
    NEraColumn neraColumn = PreloadManager.Cache.GetScene("res://scenes/timeline_screen/era_column.tscn").Instantiate<NEraColumn>((PackedScene.GenEditState) 0L);
    neraColumn._data = data;
    return neraColumn;
  }

  public override void _Ready()
  {
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Icon"));
    this._name = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Name"));
    this._year = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Year"));
    this.era = this._data.Era;
    ((Node) this).SetName(this._data.Era.ToString());
    this.Init(this._data);
    ((CanvasItem) this).ItemRectChanged += new Action(this.RectChange);
  }

  public void Init(EpochSlotData epochSlot)
  {
    (Texture2D Texture, string Name) eraIcon = NTimelineScreen.GetEraIcon(epochSlot.Era);
    this._icon.Texture = eraIcon.Texture;
    this._name.SetTextAutoSize(new LocString("eras", eraIcon.Name + ".name").GetFormattedText());
    this._year.SetTextAutoSize(new LocString("eras", eraIcon.Name + ".year").GetFormattedText());
    this.AddSlot(epochSlot);
  }

  public void AddSlot(EpochSlotData epochSlotData)
  {
    NEpochSlot child = NEpochSlot.Create(epochSlotData);
    ((Node) this).AddChildSafely((Node) child);
    ((Node) child).Name = StringName.op_Implicit($"Slot{epochSlotData.EraPosition}");
    ((Node) this).MoveChildSafely((Node) child, 0);
  }

  public void SpawnIcon()
  {
    this._iconTween = ((Node) this).CreateTween().SetParallel(true);
    this._iconTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5);
    this._iconTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).From(Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.1f))).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
  }

  public async Task SpawnSlots(bool isAnimated)
  {
    foreach (Node child in ((Node) this).GetChildren(false))
    {
      if (child is NEpochSlot nepochSlot && !nepochSlot.HasSpawned)
      {
        if (isAnimated)
          await nepochSlot.SpawnSlot();
        else
          TaskHelper.RunSafely(nepochSlot.SpawnSlot());
      }
    }
  }

  public async Task SpawnNameAndYear()
  {
    if (this._labelSpawned)
      return;
    this._labelSpawned = true;
    this._labelTween = ((Node) this).CreateTween().SetParallel(true);
    ((CanvasItem) this._name).SelfModulate = new Color(((CanvasItem) this._name).SelfModulate.R, ((CanvasItem) this._name).SelfModulate.G, ((CanvasItem) this._name).SelfModulate.B, 0.0f);
    ((CanvasItem) this._year).Modulate = new Color(((CanvasItem) this._year).Modulate.R, ((CanvasItem) this._year).Modulate.G, ((CanvasItem) this._year).Modulate.B, 0.0f);
    this._labelTween.TweenProperty((GodotObject) this._name, NodePath.op_Implicit("self_modulate:a"), Variant.op_Implicit(1f), 1.0);
    this._labelTween.TweenProperty((GodotObject) this._name, NodePath.op_Implicit("position:y"), Variant.op_Implicit(28f), 1.0).From(Variant.op_Implicit(-36f)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._labelTween.TweenProperty((GodotObject) this._year, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 1.0).SetDelay(0.5);
    this._labelTween.TweenProperty((GodotObject) this._year, NodePath.op_Implicit("position:y"), Variant.op_Implicit(20f), 1.0).SetDelay(0.5).From(Variant.op_Implicit(0.0f)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    bool flag = await this._labelTween.AwaitFinished((Node) this);
    await Task.Delay(500);
  }

  public async Task SaveBeforeAnimationPosition()
  {
    this._isAnimated = true;
    this._prevLocalPos = this.Position;
    this._prevGlobalPos = this.GlobalPosition;
    double num = (double) await ((Node) this).AwaitProcessFrame();
    this._isAnimated = false;
    this._targetPosition = this._predictedPosition;
    this.GlobalPosition = this._prevGlobalPos;
    ((Node) this).CreateTween().SetParallel(true).TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this._targetPosition), 2.0).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 7L);
  }

  public void SetPredictedPosition(Vector2 setPredictedPosition)
  {
    if (!this._isAnimated)
      return;
    this._predictedPosition = setPredictedPosition;
  }

  private void RectChange()
  {
    if (!this._isAnimated)
      return;
    this.GlobalPosition = this._prevGlobalPos;
  }

  public override void _ExitTree()
  {
    ((CanvasItem) this).ItemRectChanged -= new Action(this.RectChange);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NEraColumn.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEraColumn.MethodName.SpawnIcon, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEraColumn.MethodName.SetPredictedPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("setPredictedPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEraColumn.MethodName.RectChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEraColumn.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEraColumn.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEraColumn.MethodName.SpawnIcon) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SpawnIcon();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEraColumn.MethodName.SetPredictedPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetPredictedPosition(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEraColumn.MethodName.RectChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RectChange();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEraColumn.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEraColumn.MethodName._Ready) || StringName.op_Equality(ref method, NEraColumn.MethodName.SpawnIcon) || StringName.op_Equality(ref method, NEraColumn.MethodName.SetPredictedPosition) || StringName.op_Equality(ref method, NEraColumn.MethodName.RectChange) || StringName.op_Equality(ref method, NEraColumn.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._name))
    {
      this._name = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._year))
    {
      this._year = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._iconTween))
    {
      this._iconTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._labelTween))
    {
      this._labelTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._labelSpawned))
    {
      this._labelSpawned = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName.era))
    {
      this.era = VariantUtils.ConvertTo<EpochEra>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._prevLocalPos))
    {
      this._prevLocalPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._prevGlobalPos))
    {
      this._prevGlobalPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._predictedPosition))
    {
      this._predictedPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._targetPosition))
    {
      this._targetPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEraColumn.PropertyName._isAnimated))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isAnimated = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._name))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._name);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._year))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._year);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._iconTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._iconTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._labelTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._labelTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._labelSpawned))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._labelSpawned);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName.era))
    {
      value = VariantUtils.CreateFrom<EpochEra>(ref this.era);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._prevLocalPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._prevLocalPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._prevGlobalPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._prevGlobalPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._predictedPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._predictedPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NEraColumn.PropertyName._targetPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._targetPosition);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEraColumn.PropertyName._isAnimated))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isAnimated);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NEraColumn.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEraColumn.PropertyName._name, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEraColumn.PropertyName._year, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEraColumn.PropertyName._iconTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEraColumn.PropertyName._labelTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEraColumn.PropertyName._labelSpawned, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NEraColumn.PropertyName.era, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NEraColumn.PropertyName._prevLocalPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NEraColumn.PropertyName._prevGlobalPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NEraColumn.PropertyName._predictedPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NEraColumn.PropertyName._targetPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEraColumn.PropertyName._isAnimated, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NEraColumn.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NEraColumn.PropertyName._name, Variant.From<MegaLabel>(ref this._name));
    info.AddProperty(NEraColumn.PropertyName._year, Variant.From<MegaLabel>(ref this._year));
    info.AddProperty(NEraColumn.PropertyName._iconTween, Variant.From<Tween>(ref this._iconTween));
    info.AddProperty(NEraColumn.PropertyName._labelTween, Variant.From<Tween>(ref this._labelTween));
    info.AddProperty(NEraColumn.PropertyName._labelSpawned, Variant.From<bool>(ref this._labelSpawned));
    info.AddProperty(NEraColumn.PropertyName.era, Variant.From<EpochEra>(ref this.era));
    info.AddProperty(NEraColumn.PropertyName._prevLocalPos, Variant.From<Vector2>(ref this._prevLocalPos));
    info.AddProperty(NEraColumn.PropertyName._prevGlobalPos, Variant.From<Vector2>(ref this._prevGlobalPos));
    info.AddProperty(NEraColumn.PropertyName._predictedPosition, Variant.From<Vector2>(ref this._predictedPosition));
    info.AddProperty(NEraColumn.PropertyName._targetPosition, Variant.From<Vector2>(ref this._targetPosition));
    info.AddProperty(NEraColumn.PropertyName._isAnimated, Variant.From<bool>(ref this._isAnimated));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEraColumn.PropertyName._icon, ref variant1))
      this._icon = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NEraColumn.PropertyName._name, ref variant2))
      this._name = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NEraColumn.PropertyName._year, ref variant3))
      this._year = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NEraColumn.PropertyName._iconTween, ref variant4))
      this._iconTween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (info.TryGetProperty(NEraColumn.PropertyName._labelTween, ref variant5))
      this._labelTween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (info.TryGetProperty(NEraColumn.PropertyName._labelSpawned, ref variant6))
      this._labelSpawned = ((Variant) ref variant6).As<bool>();
    Variant variant7;
    if (info.TryGetProperty(NEraColumn.PropertyName.era, ref variant7))
      this.era = ((Variant) ref variant7).As<EpochEra>();
    Variant variant8;
    if (info.TryGetProperty(NEraColumn.PropertyName._prevLocalPos, ref variant8))
      this._prevLocalPos = ((Variant) ref variant8).As<Vector2>();
    Variant variant9;
    if (info.TryGetProperty(NEraColumn.PropertyName._prevGlobalPos, ref variant9))
      this._prevGlobalPos = ((Variant) ref variant9).As<Vector2>();
    Variant variant10;
    if (info.TryGetProperty(NEraColumn.PropertyName._predictedPosition, ref variant10))
      this._predictedPosition = ((Variant) ref variant10).As<Vector2>();
    Variant variant11;
    if (info.TryGetProperty(NEraColumn.PropertyName._targetPosition, ref variant11))
      this._targetPosition = ((Variant) ref variant11).As<Vector2>();
    Variant variant12;
    if (!info.TryGetProperty(NEraColumn.PropertyName._isAnimated, ref variant12))
      return;
    this._isAnimated = ((Variant) ref variant12).As<bool>();
  }

  static NEraColumn()
  {
    List<string> items = new List<string>();
    items.Add("res://scenes/timeline_screen/era_column.tscn");
    items.AddRange(NEpochSlot.assetPaths);
    // ISSUE: object of a compiler-generated type is created
    NEraColumn.assetPaths = (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyList<string>(items);
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SpawnIcon = StringName.op_Implicit(nameof (SpawnIcon));
    public static readonly StringName SetPredictedPosition = StringName.op_Implicit(nameof (SetPredictedPosition));
    public static readonly StringName RectChange = StringName.op_Implicit(nameof (RectChange));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _name = StringName.op_Implicit(nameof (_name));
    public static readonly StringName _year = StringName.op_Implicit(nameof (_year));
    public static readonly StringName _iconTween = StringName.op_Implicit(nameof (_iconTween));
    public static readonly StringName _labelTween = StringName.op_Implicit(nameof (_labelTween));
    public static readonly StringName _labelSpawned = StringName.op_Implicit(nameof (_labelSpawned));
    public static readonly StringName era = StringName.op_Implicit(nameof (era));
    public static readonly StringName _prevLocalPos = StringName.op_Implicit(nameof (_prevLocalPos));
    public static readonly StringName _prevGlobalPos = StringName.op_Implicit(nameof (_prevGlobalPos));
    public static readonly StringName _predictedPosition = StringName.op_Implicit(nameof (_predictedPosition));
    public static readonly StringName _targetPosition = StringName.op_Implicit(nameof (_targetPosition));
    public static readonly StringName _isAnimated = StringName.op_Implicit(nameof (_isAnimated));
  }

  public class SignalName : Control.SignalName
  {
  }
}
