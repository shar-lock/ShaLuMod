// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Ftue.NAscensionMultiplayerFtue
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
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Ftue;

[ScriptPath("res://src/Core/Nodes/Ftue/NAscensionMultiplayerFtue.cs")]
public class NAscensionMultiplayerFtue : NFtue
{
  public const string id = "ascension_multiplayer_ftue";
  private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/ascension_multiplayer_ftue");

  public override void _Ready()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Header")).SetTextAutoSize(new LocString("ftues", "ASCENSION_MULTIPLAYER_FTUE_TITLE").GetFormattedText());
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Description")).SetTextAutoSize(new LocString("ftues", "ASCENSION_MULTIPLAYER_FTUE_DESCRIPTION").GetFormattedText());
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Disclaimer")).SetTextAutoSize(new LocString("ftues", "ASCENSION_MULTIPLAYER_FTUE_DISCLAIMER").GetFormattedText());
    ((GodotObject) ((Node) this).GetNode<NButton>(NodePath.op_Implicit("%FtueConfirmButton"))).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseFtue)), 0U);
    Tween tween = ((Node) this).CreateTween().SetParallel(true);
    Color modulate = ((CanvasItem) this).Modulate;
    modulate.A = 0.0f;
    ((CanvasItem) this).Modulate = modulate;
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this.Position.Y), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(this.Position.Y + 100f)).SetDelay(1.0);
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.3).SetEase((Tween.EaseType) 1L).SetDelay(1.0);
  }

  public static NAscensionMultiplayerFtue? Create()
  {
    return TestMode.IsOn ? (NAscensionMultiplayerFtue) null : PreloadManager.Cache.GetScene(NAscensionMultiplayerFtue._scenePath).Instantiate<NAscensionMultiplayerFtue>((PackedScene.GenEditState) 0L);
  }

  private void CloseFtue(NButton _)
  {
    SaveManager.Instance.MarkFtueAsComplete("ascension_multiplayer_ftue");
    this.CloseFtue();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NAscensionMultiplayerFtue.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAscensionMultiplayerFtue.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAscensionMultiplayerFtue.MethodName.CloseFtue, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAscensionMultiplayerFtue.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAscensionMultiplayerFtue.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NAscensionMultiplayerFtue nascensionMultiplayerFtue = NAscensionMultiplayerFtue.Create();
      ret = VariantUtils.CreateFrom<NAscensionMultiplayerFtue>(ref nascensionMultiplayerFtue);
      return true;
    }
    if (!StringName.op_Equality(ref method, NAscensionMultiplayerFtue.MethodName.CloseFtue) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.CloseFtue(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAscensionMultiplayerFtue.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NAscensionMultiplayerFtue nascensionMultiplayerFtue = NAscensionMultiplayerFtue.Create();
      ret = VariantUtils.CreateFrom<NAscensionMultiplayerFtue>(ref nascensionMultiplayerFtue);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAscensionMultiplayerFtue.MethodName._Ready) || StringName.op_Equality(ref method, NAscensionMultiplayerFtue.MethodName.Create) || StringName.op_Equality(ref method, NAscensionMultiplayerFtue.MethodName.CloseFtue) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
  }

  public new class MethodName : NFtue.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName CloseFtue = StringName.op_Implicit(nameof (CloseFtue));
  }

  public new class PropertyName : NFtue.PropertyName
  {
  }

  public new class SignalName : NFtue.SignalName
  {
  }
}
