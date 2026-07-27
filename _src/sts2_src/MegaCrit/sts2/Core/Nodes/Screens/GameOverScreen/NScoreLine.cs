// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen.NScoreLine
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
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;

[ScriptPath("res://src/Core/Nodes/Screens/GameOverScreen/NScoreLine.cs")]
public class NScoreLine : Control
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/game_over_screen/score_line");
  private Tween? _tween;

  public static NScoreLine Create(string label, string score, Texture2D? icon = null)
  {
    NScoreLine nscoreLine = PreloadManager.Cache.GetScene(NScoreLine._scenePath).Instantiate<NScoreLine>((PackedScene.GenEditState) 0L);
    ((Node) nscoreLine).GetNode<MegaLabel>(NodePath.op_Implicit("%Label")).SetTextAutoSize(label);
    ((Node) nscoreLine).GetNode<MegaLabel>(NodePath.op_Implicit("%Score")).SetTextAutoSize(score);
    if (icon != null)
      ((Node) nscoreLine).GetNode<TextureRect>(NodePath.op_Implicit("%Icon")).Texture = icon;
    return nscoreLine;
  }

  public async Task AnimateIn()
  {
    if (!((Node) this).IsValid())
      return;
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.3);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position:x"), Variant.op_Implicit(this.Position.X), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 11L).From(Variant.op_Implicit(this.Position.X - 50f));
    if (SaveManager.Instance.PrefsSave.FastMode != FastModeType.Instant)
    {
      this._tween.Chain();
      this._tween.TweenInterval(0.1);
    }
    bool flag = await this._tween.AwaitFinished((Node) this);
  }

  public override void _ExitTree() => this._tween?.Kill();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NScoreLine.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("label"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("score"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("icon"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false)
      }, (List<Variant>) null),
      new MethodInfo(NScoreLine.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NScoreLine.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NScoreLine nscoreLine = NScoreLine.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NScoreLine>(ref nscoreLine);
      return true;
    }
    if (!StringName.op_Equality(ref method, NScoreLine.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NScoreLine.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NScoreLine nscoreLine = NScoreLine.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NScoreLine>(ref nscoreLine);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NScoreLine.MethodName.Create) || StringName.op_Equality(ref method, NScoreLine.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NScoreLine.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NScoreLine.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NScoreLine.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NScoreLine.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NScoreLine.PropertyName._tween, ref variant))
      return;
    this._tween = ((Variant) ref variant).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
