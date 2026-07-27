// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Ui.NFailedJoinVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Ui;

[ScriptPath("res://src/Core/Nodes/Vfx/Ui/NFailedJoinVfx.cs")]
public class NFailedJoinVfx : Control
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/ui/vfx_failed_join");
  private Tween? _tween;
  private MegaRichTextLabel _label;

  public override void _Ready() => TaskHelper.RunSafely(this.PlayAndSelfDestruct());

  public static NFailedJoinVfx? Create(string text)
  {
    if (TestMode.IsOn)
      return (NFailedJoinVfx) null;
    NFailedJoinVfx nfailedJoinVfx = PreloadManager.Cache.GetScene(NFailedJoinVfx._scenePath).Instantiate<NFailedJoinVfx>((PackedScene.GenEditState) 0L);
    ((Node) nfailedJoinVfx).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Label")).SetTextAutoSize(text);
    return nfailedJoinVfx;
  }

  private async Task PlayAndSelfDestruct()
  {
    ((CanvasItem) this).Modulate = StsColors.transparentWhite;
    Vector2 position = this.Position;
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.05);
    for (int index = 0; index < 5; ++index)
    {
      float num = (float) (24.0 * (1.0 - (double) index / 5.0) * (index % 2 == 0 ? 1.0 : -1.0));
      this._tween.Chain().TweenProperty((GodotObject) this, NodePath.op_Implicit("position:x"), Variant.op_Implicit(position.X + num), 0.05000000074505806).SetTrans((Tween.TransitionType) 1L).SetEase((Tween.EaseType) 2L);
    }
    this._tween.Chain().TweenProperty((GodotObject) this, NodePath.op_Implicit("position:x"), Variant.op_Implicit(position.X), 0.05000000074505806).SetTrans((Tween.TransitionType) 1L).SetEase((Tween.EaseType) 1L);
    this._tween.TweenInterval(3.0);
    this._tween.Chain();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
    if (!await this._tween.AwaitFinished((Node) this))
      return;
    ((Node) this).QueueFreeSafely();
  }

  public override void _ExitTree() => this._tween?.Kill();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NFailedJoinVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFailedJoinVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NFailedJoinVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFailedJoinVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFailedJoinVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NFailedJoinVfx nfailedJoinVfx = NFailedJoinVfx.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NFailedJoinVfx>(ref nfailedJoinVfx);
      return true;
    }
    if (!StringName.op_Equality(ref method, NFailedJoinVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NFailedJoinVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NFailedJoinVfx nfailedJoinVfx = NFailedJoinVfx.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NFailedJoinVfx>(ref nfailedJoinVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFailedJoinVfx.MethodName._Ready) || StringName.op_Equality(ref method, NFailedJoinVfx.MethodName.Create) || StringName.op_Equality(ref method, NFailedJoinVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFailedJoinVfx.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFailedJoinVfx.PropertyName._label))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFailedJoinVfx.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFailedJoinVfx.PropertyName._label))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NFailedJoinVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFailedJoinVfx.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NFailedJoinVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NFailedJoinVfx.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NFailedJoinVfx.PropertyName._tween, ref variant1))
      this._tween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (!info.TryGetProperty(NFailedJoinVfx.PropertyName._label, ref variant2))
      return;
    this._label = ((Variant) ref variant2).As<MegaRichTextLabel>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
  }

  public class SignalName : Control.SignalName
  {
  }
}
