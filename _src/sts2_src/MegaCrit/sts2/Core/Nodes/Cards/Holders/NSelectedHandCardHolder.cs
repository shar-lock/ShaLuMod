// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Cards.Holders.NSelectedHandCardHolder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Cards.Holders;

[ScriptPath("res://src/Core/Nodes/Cards/Holders/NSelectedHandCardHolder.cs")]
public class NSelectedHandCardHolder : NCardHolder
{
  private Tween? _tween;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("/cards/holders/selected_hand_card_holder");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NSelectedHandCardHolder.ScenePath);
    }
  }

  public static NSelectedHandCardHolder Create(NHandCardHolder originalHolder)
  {
    NCard cardNode = originalHolder.CardNode;
    NSelectedHandCardHolder nselectedHandCardHolder = PreloadManager.Cache.GetScene(NSelectedHandCardHolder.ScenePath).Instantiate<NSelectedHandCardHolder>((PackedScene.GenEditState) 0L);
    originalHolder.Clear();
    ((Node) nselectedHandCardHolder).Name = StringName.op_Implicit($"SelectedHandCardHolder-{cardNode.Model.Id}");
    nselectedHandCardHolder.SetCard(cardNode);
    nselectedHandCardHolder.Scale = nselectedHandCardHolder.SmallScale;
    return nselectedHandCardHolder;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this.CardNode, NodePath.op_Implicit("position"), Variant.op_Implicit(this.Position), 0.15000000596046448).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.Play();
  }

  protected override void CreateHoverTips()
  {
  }

  public override void _ExitTree() => this._tween?.Kill();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NSelectedHandCardHolder.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("originalHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSelectedHandCardHolder.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSelectedHandCardHolder.MethodName.CreateHoverTips, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSelectedHandCardHolder.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSelectedHandCardHolder.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NSelectedHandCardHolder nselectedHandCardHolder = NSelectedHandCardHolder.Create(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NSelectedHandCardHolder>(ref nselectedHandCardHolder);
      return true;
    }
    if (StringName.op_Equality(ref method, NSelectedHandCardHolder.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSelectedHandCardHolder.MethodName.CreateHoverTips) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CreateHoverTips();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSelectedHandCardHolder.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
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
    if (StringName.op_Equality(ref method, NSelectedHandCardHolder.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NSelectedHandCardHolder nselectedHandCardHolder = NSelectedHandCardHolder.Create(VariantUtils.ConvertTo<NHandCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NSelectedHandCardHolder>(ref nselectedHandCardHolder);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSelectedHandCardHolder.MethodName.Create) || StringName.op_Equality(ref method, NSelectedHandCardHolder.MethodName._Ready) || StringName.op_Equality(ref method, NSelectedHandCardHolder.MethodName.CreateHoverTips) || StringName.op_Equality(ref method, NSelectedHandCardHolder.MethodName._ExitTree) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NSelectedHandCardHolder.PropertyName._tween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NSelectedHandCardHolder.PropertyName._tween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSelectedHandCardHolder.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NSelectedHandCardHolder.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NSelectedHandCardHolder.PropertyName._tween, ref variant))
      return;
    this._tween = ((Variant) ref variant).As<Tween>();
  }

  public new class MethodName : NCardHolder.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName CreateHoverTips = StringName.op_Implicit(nameof (CreateHoverTips));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public new class PropertyName : NCardHolder.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public new class SignalName : NCardHolder.SignalName
  {
  }
}
