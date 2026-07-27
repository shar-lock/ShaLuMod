// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NCombatStartBanner
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NCombatStartBanner.cs")]
public class NCombatStartBanner : Control
{
  private ColorRect _colorRect;
  private MegaLabel _label;
  private static readonly string _scenePath = SceneHelper.GetScenePath("combat/combat_start_banner");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NCombatStartBanner._scenePath);
    }
  }

  public static NCombatStartBanner? Create()
  {
    if (TestMode.IsOn)
      return (NCombatStartBanner) null;
    return NCombatUi.IsDebugHideTextVfx ? (NCombatStartBanner) null : PreloadManager.Cache.GetScene(NCombatStartBanner._scenePath).Instantiate<NCombatStartBanner>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._colorRect = ((Node) this).GetNode<ColorRect>(NodePath.op_Implicit("ColorRect"));
    ((CanvasItem) this._colorRect).Modulate = Colors.Transparent;
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    this._label.SetTextAutoSize(new LocString("gameplay_ui", "BATTLE_START").GetFormattedText());
    ((CanvasItem) this._label).Modulate = Colors.Transparent;
    TaskHelper.RunSafely(this.AnimateVfx());
  }

  private async Task AnimateVfx()
  {
    NDebugAudioManager.Instance?.Play(Rng.Chaotic.NextItem<string>(TmpSfx.BattleStart));
    Tween tween1 = ((Node) this).CreateTween().SetParallel(true);
    tween1.TweenInterval(0.3);
    tween1.Chain();
    tween1.TweenProperty((GodotObject) this._colorRect, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.5f), 0.75).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    tween1.TweenProperty((GodotObject) this._colorRect, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.75).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.2f)));
    tween1.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 1.2999999523162842).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    tween1.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.75).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 2f)));
    tween1.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).SetDelay(1.2999999523162842);
    if (!await tween1.AwaitFinished((Node) this))
      return;
    ((Node) this).GetParent().AddChildSafely((Node) NPlayerTurnBanner.Create(1));
    Tween tween2 = ((Node) this).CreateTween();
    tween2.TweenProperty((GodotObject) this._colorRect, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).SetDelay(1.5);
    bool flag = await tween2.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCombatStartBanner.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatStartBanner.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatStartBanner.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCombatStartBanner ncombatStartBanner = NCombatStartBanner.Create();
      ret = VariantUtils.CreateFrom<NCombatStartBanner>(ref ncombatStartBanner);
      return true;
    }
    if (!StringName.op_Equality(ref method, NCombatStartBanner.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatStartBanner.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCombatStartBanner ncombatStartBanner = NCombatStartBanner.Create();
      ret = VariantUtils.CreateFrom<NCombatStartBanner>(ref ncombatStartBanner);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCombatStartBanner.MethodName.Create) || StringName.op_Equality(ref method, NCombatStartBanner.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatStartBanner.PropertyName._colorRect))
    {
      this._colorRect = VariantUtils.ConvertTo<ColorRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatStartBanner.PropertyName._label))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatStartBanner.PropertyName._colorRect))
    {
      value = VariantUtils.CreateFrom<ColorRect>(ref this._colorRect);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatStartBanner.PropertyName._label))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCombatStartBanner.PropertyName._colorRect, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatStartBanner.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCombatStartBanner.PropertyName._colorRect, Variant.From<ColorRect>(ref this._colorRect));
    info.AddProperty(NCombatStartBanner.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCombatStartBanner.PropertyName._colorRect, ref variant1))
      this._colorRect = ((Variant) ref variant1).As<ColorRect>();
    Variant variant2;
    if (!info.TryGetProperty(NCombatStartBanner.PropertyName._label, ref variant2))
      return;
    this._label = ((Variant) ref variant2).As<MegaLabel>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _colorRect = StringName.op_Implicit(nameof (_colorRect));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
  }

  public class SignalName : Control.SignalName
  {
  }
}
