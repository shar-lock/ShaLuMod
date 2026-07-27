// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Relics.NRelicInventoryHolder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Relics;

[ScriptPath("res://src/Core/Nodes/Relics/NRelicInventoryHolder.cs")]
public class NRelicInventoryHolder : NButton
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("relics/relic_inventory_holder");
  private static readonly string _flashPath = SceneHelper.GetScenePath("vfx/relic_inventory_flash_vfx");
  private const float _newlyAcquiredPopDuration = 0.35f;
  private const float _newlyAcquiredFadeInDuration = 0.1f;
  private const float _newlyAcquiredPopDistance = 40f;
  private NRelic _relic;
  private RelicModel? _subscribedRelic;
  private MegaLabel _amountLabel;
  private Tween? _hoverTween;
  private Tween? _obtainedTween;
  private CancellationTokenSource? _cancellationTokenSource;
  private Vector2 _originalIconPosition;
  private RelicModel _model;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        NRelicInventoryHolder._scenePath,
        NRelicInventoryHolder._flashPath
      });
    }
  }

  public NRelic Relic => this._relic;

  public NRelicInventory Inventory { get; set; }

  public static NRelicInventoryHolder? Create(RelicModel relic)
  {
    if (TestMode.IsOn)
      return (NRelicInventoryHolder) null;
    NRelicInventoryHolder nrelicInventoryHolder = PreloadManager.Cache.GetScene(NRelicInventoryHolder._scenePath).Instantiate<NRelicInventoryHolder>((PackedScene.GenEditState) 0L);
    ((Node) nrelicInventoryHolder).Name = StringName.op_Implicit($"NRelicContainerHolder-{relic.Id}");
    nrelicInventoryHolder._model = relic;
    return nrelicInventoryHolder;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._relic = ((Node) this).GetNode<NRelic>(NodePath.op_Implicit("%Relic"));
    this._amountLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%AmountLabel"));
    this._originalIconPosition = ((Control) this._relic.Icon).Position;
    this._relic.ModelChanged += new Action<RelicModel, RelicModel>(this.OnModelChanged);
    this._relic.Model = this._model;
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._hoverTween?.Kill();
    this._cancellationTokenSource?.Cancel();
    if (this._subscribedRelic != null)
    {
      this._subscribedRelic.DisplayAmountChanged -= new Action(this.OnDisplayAmountChanged);
      this._subscribedRelic.StatusChanged -= new Action(this.OnStatusChanged);
      this._subscribedRelic.Flashed -= new Action<RelicModel, IEnumerable<Creature>>(this.OnRelicFlashed);
    }
    this._subscribedRelic = (RelicModel) null;
    this._relic.ModelChanged -= new Action<RelicModel, RelicModel>(this.OnModelChanged);
  }

  private void OnModelChanged(RelicModel? oldModel, RelicModel? newModel)
  {
    if (oldModel != null)
    {
      oldModel.DisplayAmountChanged -= new Action(this.OnDisplayAmountChanged);
      oldModel.StatusChanged -= new Action(this.OnStatusChanged);
      oldModel.Flashed -= new Action<RelicModel, IEnumerable<Creature>>(this.OnRelicFlashed);
    }
    if (newModel != null)
    {
      newModel.DisplayAmountChanged += new Action(this.OnDisplayAmountChanged);
      newModel.StatusChanged += new Action(this.OnStatusChanged);
      newModel.Flashed += new Action<RelicModel, IEnumerable<Creature>>(this.OnRelicFlashed);
    }
    this.RefreshAmount();
    this.RefreshStatus();
    this._subscribedRelic = newModel;
  }

  private void RefreshAmount()
  {
    if (this._relic.Model.ShowCounter && RunManager.Instance.IsInProgress)
    {
      ((CanvasItem) this._amountLabel).Visible = true;
      this._amountLabel.SetTextAutoSize(this._relic.Model.DisplayAmount.ToString());
    }
    else
      ((CanvasItem) this._amountLabel).Visible = false;
  }

  private void RefreshStatus()
  {
    if (!RunManager.Instance.IsInProgress)
    {
      ((CanvasItem) this._relic.Icon).Modulate = Colors.White;
    }
    else
    {
      this._relic.Model.UpdateTexture(this._relic.Icon);
      TextureRect icon = this._relic.Icon;
      Color color;
      switch (this._relic.Model.Status)
      {
        case RelicStatus.Normal:
        case RelicStatus.Active:
          color = Colors.White;
          break;
        case RelicStatus.Disabled:
          color = new Color("#808080");
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      ((CanvasItem) icon).Modulate = color;
    }
  }

  public async Task PlayNewlyAcquiredAnimation(Vector2? startLocation, Vector2? startScale)
  {
    if (this._cancellationTokenSource != null)
      await this._cancellationTokenSource.CancelAsync();
    CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
    this._cancellationTokenSource = cancelTokenSource;
    double num = (double) await ((Node) this).AwaitProcessFrame(cancelTokenSource.Token);
    if (cancelTokenSource.IsCancellationRequested)
    {
      cancelTokenSource = (CancellationTokenSource) null;
    }
    else
    {
      this._obtainedTween?.Kill();
      if (!startLocation.HasValue)
      {
        TextureRect icon1 = this._relic.Icon;
        Vector2 position = ((Control) this._relic.Icon).Position;
        position.Y = ((Control) this._relic.Icon).Position.Y + 40f;
        Vector2 vector2 = position;
        ((Control) icon1).Position = vector2;
        TextureRect icon2 = this._relic.Icon;
        Color modulate = ((CanvasItem) this._relic.Icon).Modulate;
        modulate.A = 0.0f;
        Color color = modulate;
        ((CanvasItem) icon2).Modulate = color;
        this._obtainedTween = ((Node) this).GetTree().CreateTween();
        this._obtainedTween.TweenProperty((GodotObject) this._relic.Icon, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.10000000149011612);
        this._obtainedTween.Parallel();
        this._obtainedTween.SetEase((Tween.EaseType) 1L);
        this._obtainedTween.SetTrans((Tween.TransitionType) 10L);
        this._obtainedTween.TweenProperty((GodotObject) this._relic.Icon, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this._originalIconPosition.Y), 0.34999999403953552);
        this._obtainedTween.TweenCallback(Callable.From(new Action(this.DoFlash)));
        cancelTokenSource = (CancellationTokenSource) null;
      }
      else
      {
        ((Control) this._relic.Icon).GlobalPosition = startLocation.Value;
        ((Control) this._relic.Icon).Scale = startScale ?? Vector2.One;
        TextureRect icon = this._relic.Icon;
        Color modulate = ((CanvasItem) this._relic.Icon).Modulate;
        modulate.A = 1f;
        Color color = modulate;
        ((CanvasItem) icon).Modulate = color;
        this._obtainedTween = ((Node) this).GetTree().CreateTween();
        this._obtainedTween.SetEase((Tween.EaseType) 1L);
        this._obtainedTween.SetTrans((Tween.TransitionType) 1L);
        this._obtainedTween.TweenProperty((GodotObject) this._relic.Icon, NodePath.op_Implicit("position"), Variant.op_Implicit(this._originalIconPosition), 0.34999999403953552);
        this._obtainedTween.Parallel().TweenProperty((GodotObject) this._relic.Icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.34999999403953552);
        this._obtainedTween.TweenCallback(Callable.From(new Action(this.DoFlash)));
        cancelTokenSource = (CancellationTokenSource) null;
      }
    }
  }

  protected override void OnFocus()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this._relic.Icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.25f)), 0.05);
    NHoverTipSet.CreateAndShow((Control) this, this._relic.Model.HoverTips)?.SetAlignmentForRelic(this._relic);
  }

  protected override void OnUnfocus()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this._relic.Icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    NHoverTipSet.Remove((Control) this);
  }

  private void OnRelicFlashed(RelicModel _, IEnumerable<Creature> __) => this.DoFlash();

  private void DoFlash()
  {
    Node2D child = PreloadManager.Cache.GetScene(NRelicInventoryHolder._flashPath).Instantiate<Node2D>((PackedScene.GenEditState) 0L);
    Node topBarVfxContainer = (Node) NRun.Instance.GlobalUi.AboveTopBarVfxContainer;
    ((Node) child).GetNode<GpuParticles2D>(NodePath.op_Implicit("Particles")).Texture = this._relic.Model.Icon;
    child.GlobalPosition = Vector2.op_Addition(this.GlobalPosition, Vector2.op_Multiply(this.Size, 0.5f));
    topBarVfxContainer.AddChildSafely((Node) child);
  }

  private void OnDisplayAmountChanged() => this.RefreshAmount();

  private void OnStatusChanged() => this.RefreshStatus();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NRelicInventoryHolder.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventoryHolder.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventoryHolder.MethodName.RefreshAmount, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventoryHolder.MethodName.RefreshStatus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventoryHolder.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventoryHolder.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventoryHolder.MethodName.DoFlash, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventoryHolder.MethodName.OnDisplayAmountChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicInventoryHolder.MethodName.OnStatusChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName.RefreshAmount) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshAmount();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName.RefreshStatus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshStatus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName.DoFlash) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DoFlash();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName.OnDisplayAmountChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisplayAmountChanged();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName.OnStatusChanged) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnStatusChanged();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName._Ready) || StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName._ExitTree) || StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName.RefreshAmount) || StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName.RefreshStatus) || StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName.OnFocus) || StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName.DoFlash) || StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName.OnDisplayAmountChanged) || StringName.op_Equality(ref method, NRelicInventoryHolder.MethodName.OnStatusChanged) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicInventoryHolder.PropertyName.Inventory))
    {
      this.Inventory = VariantUtils.ConvertTo<NRelicInventory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicInventoryHolder.PropertyName._relic))
    {
      this._relic = VariantUtils.ConvertTo<NRelic>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicInventoryHolder.PropertyName._amountLabel))
    {
      this._amountLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicInventoryHolder.PropertyName._hoverTween))
    {
      this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicInventoryHolder.PropertyName._obtainedTween))
    {
      this._obtainedTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicInventoryHolder.PropertyName._originalIconPosition))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._originalIconPosition = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicInventoryHolder.PropertyName.Relic))
    {
      ref godot_variant local = ref value;
      NRelic relic = this.Relic;
      godot_variant from = VariantUtils.CreateFrom<NRelic>(ref relic);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicInventoryHolder.PropertyName.Inventory))
    {
      ref godot_variant local = ref value;
      NRelicInventory inventory = this.Inventory;
      godot_variant from = VariantUtils.CreateFrom<NRelicInventory>(ref inventory);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicInventoryHolder.PropertyName._relic))
    {
      value = VariantUtils.CreateFrom<NRelic>(ref this._relic);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicInventoryHolder.PropertyName._amountLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._amountLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicInventoryHolder.PropertyName._hoverTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicInventoryHolder.PropertyName._obtainedTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._obtainedTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicInventoryHolder.PropertyName._originalIconPosition))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._originalIconPosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRelicInventoryHolder.PropertyName._relic, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicInventoryHolder.PropertyName._amountLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicInventoryHolder.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicInventoryHolder.PropertyName._obtainedTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NRelicInventoryHolder.PropertyName._originalIconPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicInventoryHolder.PropertyName.Relic, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicInventoryHolder.PropertyName.Inventory, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName inventory1 = NRelicInventoryHolder.PropertyName.Inventory;
    NRelicInventory inventory2 = this.Inventory;
    Variant variant = Variant.From<NRelicInventory>(ref inventory2);
    serializationInfo.AddProperty(inventory1, variant);
    info.AddProperty(NRelicInventoryHolder.PropertyName._relic, Variant.From<NRelic>(ref this._relic));
    info.AddProperty(NRelicInventoryHolder.PropertyName._amountLabel, Variant.From<MegaLabel>(ref this._amountLabel));
    info.AddProperty(NRelicInventoryHolder.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
    info.AddProperty(NRelicInventoryHolder.PropertyName._obtainedTween, Variant.From<Tween>(ref this._obtainedTween));
    info.AddProperty(NRelicInventoryHolder.PropertyName._originalIconPosition, Variant.From<Vector2>(ref this._originalIconPosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRelicInventoryHolder.PropertyName.Inventory, ref variant1))
      this.Inventory = ((Variant) ref variant1).As<NRelicInventory>();
    Variant variant2;
    if (info.TryGetProperty(NRelicInventoryHolder.PropertyName._relic, ref variant2))
      this._relic = ((Variant) ref variant2).As<NRelic>();
    Variant variant3;
    if (info.TryGetProperty(NRelicInventoryHolder.PropertyName._amountLabel, ref variant3))
      this._amountLabel = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NRelicInventoryHolder.PropertyName._hoverTween, ref variant4))
      this._hoverTween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (info.TryGetProperty(NRelicInventoryHolder.PropertyName._obtainedTween, ref variant5))
      this._obtainedTween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (!info.TryGetProperty(NRelicInventoryHolder.PropertyName._originalIconPosition, ref variant6))
      return;
    this._originalIconPosition = ((Variant) ref variant6).As<Vector2>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName RefreshAmount = StringName.op_Implicit(nameof (RefreshAmount));
    public static readonly StringName RefreshStatus = StringName.op_Implicit(nameof (RefreshStatus));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName DoFlash = StringName.op_Implicit(nameof (DoFlash));
    public static readonly StringName OnDisplayAmountChanged = StringName.op_Implicit(nameof (OnDisplayAmountChanged));
    public static readonly StringName OnStatusChanged = StringName.op_Implicit(nameof (OnStatusChanged));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName Relic = StringName.op_Implicit(nameof (Relic));
    public static readonly StringName Inventory = StringName.op_Implicit(nameof (Inventory));
    public static readonly StringName _relic = StringName.op_Implicit(nameof (_relic));
    public static readonly StringName _amountLabel = StringName.op_Implicit(nameof (_amountLabel));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
    public static readonly StringName _obtainedTween = StringName.op_Implicit(nameof (_obtainedTween));
    public static readonly StringName _originalIconPosition = StringName.op_Implicit(nameof (_originalIconPosition));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
