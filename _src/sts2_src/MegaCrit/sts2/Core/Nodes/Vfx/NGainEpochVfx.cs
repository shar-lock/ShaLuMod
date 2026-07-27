// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NGainEpochVfx
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
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Timeline;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NGainEpochVfx.cs")]
public class NGainEpochVfx : Node
{
  private static int _vfxCount;
  private Control _epoch;
  private EpochModel _model;
  private TextureRect _portrait;
  private MegaLabel _label;
  private CancellationTokenSource? _cts;
  private Tween? _tween;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NGainEpochVfx.ScenePath);
    }
  }

  private static string ScenePath => SceneHelper.GetScenePath("vfx/vfx_gain_epoch");

  public override void _Ready()
  {
    this._epoch = this.GetNode<Control>(NodePath.op_Implicit("%EpochContainer"));
    this._portrait = this.GetNode<TextureRect>(NodePath.op_Implicit("%Portrait"));
    this._portrait.Texture = this._model.Portrait;
    this._label = this.GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
    this._label.SetTextAutoSize(new LocString("vfx", "EPOCH_GAIN").GetRawText());
    TaskHelper.RunSafely(this.AnimateVfx());
  }

  private async Task AnimateVfx()
  {
    this._cts = new CancellationTokenSource();
    if (NGainEpochVfx._vfxCount > 1)
    {
      int num = 3000 * (NGainEpochVfx._vfxCount - 1);
      Log.Info($"Delaying Gain Epoch Vfx by: {num}ms");
      await Task.Delay(num, this._cts.Token);
    }
    this._epoch.RotationDegrees = -30f;
    this._tween = this.CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._epoch, NodePath.op_Implicit("position:x"), Variant.op_Implicit(164f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._tween.TweenProperty((GodotObject) this._epoch, NodePath.op_Implicit("rotation"), Variant.op_Implicit(0.0f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._tween.Chain();
    this._tween.TweenInterval(1.5);
    this._tween.Chain();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0);
    bool flag = await this._tween.AwaitFinished((Node) this);
    this.QueueFreeSafely();
  }

  public static NGainEpochVfx Create(EpochModel model)
  {
    ++NGainEpochVfx._vfxCount;
    NGainEpochVfx ngainEpochVfx = PreloadManager.Cache.GetScene(NGainEpochVfx.ScenePath).Instantiate<NGainEpochVfx>((PackedScene.GenEditState) 0L);
    ngainEpochVfx._model = model;
    return ngainEpochVfx;
  }

  public override void _ExitTree()
  {
    this._cts?.Cancel();
    this._tween?.Kill();
    --NGainEpochVfx._vfxCount;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NGainEpochVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGainEpochVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGainEpochVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NGainEpochVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    base._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGainEpochVfx.MethodName._Ready) || StringName.op_Equality(ref method, NGainEpochVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGainEpochVfx.PropertyName._epoch))
    {
      this._epoch = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGainEpochVfx.PropertyName._portrait))
    {
      this._portrait = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGainEpochVfx.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGainEpochVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGainEpochVfx.PropertyName._epoch))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._epoch);
      return true;
    }
    if (StringName.op_Equality(ref name, NGainEpochVfx.PropertyName._portrait))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._portrait);
      return true;
    }
    if (StringName.op_Equality(ref name, NGainEpochVfx.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGainEpochVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NGainEpochVfx.PropertyName._epoch, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGainEpochVfx.PropertyName._portrait, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGainEpochVfx.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGainEpochVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NGainEpochVfx.PropertyName._epoch, Variant.From<Control>(ref this._epoch));
    info.AddProperty(NGainEpochVfx.PropertyName._portrait, Variant.From<TextureRect>(ref this._portrait));
    info.AddProperty(NGainEpochVfx.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NGainEpochVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NGainEpochVfx.PropertyName._epoch, ref variant1))
      this._epoch = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NGainEpochVfx.PropertyName._portrait, ref variant2))
      this._portrait = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NGainEpochVfx.PropertyName._label, ref variant3))
      this._label = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (!info.TryGetProperty(NGainEpochVfx.PropertyName._tween, ref variant4))
      return;
    this._tween = ((Variant) ref variant4).As<Tween>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _epoch = StringName.op_Implicit(nameof (_epoch));
    public static readonly StringName _portrait = StringName.op_Implicit(nameof (_portrait));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Node.SignalName
  {
  }
}
