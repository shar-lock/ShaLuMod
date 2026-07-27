// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Cards.NFanOfKnivesVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NFanOfKnivesVfx.cs")]
public class NFanOfKnivesVfx : Node2D
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/fan_of_knives_vfx");
  private const string _fanOfKnivesSfx = "event:/sfx/characters/silent/silent_fan_of_knives";
  private readonly List<Node2D> _shivs = new List<Node2D>();
  private Node2D _shiv1;
  private Node2D _shiv2;
  private Node2D _shiv3;
  private Node2D _shiv4;
  private Node2D _shiv5;
  private Node2D _shiv6;
  private Node2D _shiv7;
  private Node2D _shiv8;
  private Node2D _shiv9;
  private Vector2 _spawnPosition;
  private const double _fanDuration = 0.8;
  private Tween? _spawnTween;
  private Tween? _fanTween;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NFanOfKnivesVfx._scenePath);
    }
  }

  public static NFanOfKnivesVfx? Create(Creature target)
  {
    if (TestMode.IsOn)
      return (NFanOfKnivesVfx) null;
    NFanOfKnivesVfx nfanOfKnivesVfx = PreloadManager.Cache.GetScene(NFanOfKnivesVfx._scenePath).Instantiate<NFanOfKnivesVfx>((PackedScene.GenEditState) 0L);
    nfanOfKnivesVfx._spawnPosition = NCombatRoom.Instance.GetCreatureNode(target).VfxSpawnPosition;
    return nfanOfKnivesVfx;
  }

  public override void _Ready()
  {
    this._shiv1 = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("ShivFanParticle1"));
    this._shiv2 = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("ShivFanParticle2"));
    this._shiv3 = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("ShivFanParticle3"));
    this._shiv4 = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("ShivFanParticle4"));
    this._shiv5 = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("ShivFanParticle5"));
    this._shiv6 = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("ShivFanParticle6"));
    this._shiv7 = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("ShivFanParticle7"));
    this._shiv8 = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("ShivFanParticle8"));
    this._shiv9 = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("ShivFanParticle9"));
    this._shivs.Add(this._shiv1);
    this._shivs.Add(this._shiv2);
    this._shivs.Add(this._shiv3);
    this._shivs.Add(this._shiv4);
    this._shivs.Add(this._shiv5);
    this._shivs.Add(this._shiv6);
    this._shivs.Add(this._shiv7);
    this._shivs.Add(this._shiv8);
    this._shivs.Add(this._shiv9);
    foreach (Node2D shiv in this._shivs)
    {
      shiv.Scale = Vector2.op_Multiply(Vector2.One, Rng.Chaotic.NextFloat(0.98f, 1.02f));
      shiv.GlobalPosition = this._spawnPosition;
    }
    TaskHelper.RunSafely(this.Animate());
  }

  public override void _ExitTree()
  {
    this._fanTween?.Kill();
    this._spawnTween?.Kill();
  }

  private async Task Animate()
  {
    SfxCmd.Play("event:/sfx/characters/silent/silent_fan_of_knives");
    this._spawnTween = ((Node) this).CreateTween().SetParallel(true);
    foreach (Node2D shiv in this._shivs)
    {
      float num = Rng.Chaotic.NextFloat(0.4f, 0.8f);
      this._spawnTween.TweenProperty((GodotObject) shiv, NodePath.op_Implicit("offset:y"), Variant.op_Implicit(-180f), (double) num).From(Variant.op_Implicit(0.0f)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
      this._spawnTween.TweenProperty((GodotObject) shiv, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), (double) num).From(Variant.op_Implicit(StsColors.transparentBlack));
      this._spawnTween.TweenProperty((GodotObject) ((Node) shiv).GetNode<Node2D>(NodePath.op_Implicit("Shadow")), NodePath.op_Implicit("offset:y"), Variant.op_Implicit(-180f), (double) num).From(Variant.op_Implicit(0.0f)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    }
    this._spawnTween.Chain();
    foreach (GodotObject shiv in this._shivs)
      this._spawnTween.TweenProperty(shiv, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentWhite), 0.4).SetDelay(Rng.Chaotic.NextDouble(0.25, 0.5));
    this._fanTween = ((Node) this).CreateTween().SetParallel(true);
    this._fanTween.TweenInterval(0.40000000596046448);
    this._fanTween.Chain();
    this._fanTween.TweenProperty((GodotObject) this._shiv1, NodePath.op_Implicit("rotation"), Variant.op_Implicit(-1.74533f), 0.8).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._fanTween.TweenProperty((GodotObject) this._shiv2, NodePath.op_Implicit("rotation"), Variant.op_Implicit(-1.30899751f), 0.8).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._fanTween.TweenProperty((GodotObject) this._shiv3, NodePath.op_Implicit("rotation"), Variant.op_Implicit(-0.872665f), 0.8).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._fanTween.TweenProperty((GodotObject) this._shiv4, NodePath.op_Implicit("rotation"), Variant.op_Implicit(-0.4363325f), 0.8).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._fanTween.TweenProperty((GodotObject) this._shiv6, NodePath.op_Implicit("rotation"), Variant.op_Implicit(0.4363325f), 0.8).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._fanTween.TweenProperty((GodotObject) this._shiv7, NodePath.op_Implicit("rotation"), Variant.op_Implicit(0.872665f), 0.8).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._fanTween.TweenProperty((GodotObject) this._shiv8, NodePath.op_Implicit("rotation"), Variant.op_Implicit(1.30899751f), 0.8).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._fanTween.TweenProperty((GodotObject) this._shiv9, NodePath.op_Implicit("rotation"), Variant.op_Implicit(1.74533f), 0.8).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    if (!await this._spawnTween.AwaitFinished((Node) this))
      return;
    if (!await this._fanTween.AwaitFinished((Node) this))
      return;
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NFanOfKnivesVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFanOfKnivesVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFanOfKnivesVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NFanOfKnivesVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFanOfKnivesVfx.MethodName._Ready) || StringName.op_Equality(ref method, NFanOfKnivesVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv1))
    {
      this._shiv1 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv2))
    {
      this._shiv2 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv3))
    {
      this._shiv3 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv4))
    {
      this._shiv4 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv5))
    {
      this._shiv5 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv6))
    {
      this._shiv6 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv7))
    {
      this._shiv7 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv8))
    {
      this._shiv8 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv9))
    {
      this._shiv9 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._spawnPosition))
    {
      this._spawnPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._spawnTween))
    {
      this._spawnTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._fanTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._fanTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv1))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._shiv1);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv2))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._shiv2);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv3))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._shiv3);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv4))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._shiv4);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv5))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._shiv5);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv6))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._shiv6);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv7))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._shiv7);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv8))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._shiv8);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._shiv9))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._shiv9);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._spawnPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._spawnPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._spawnTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._spawnTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFanOfKnivesVfx.PropertyName._fanTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._fanTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NFanOfKnivesVfx.PropertyName._shiv1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFanOfKnivesVfx.PropertyName._shiv2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFanOfKnivesVfx.PropertyName._shiv3, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFanOfKnivesVfx.PropertyName._shiv4, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFanOfKnivesVfx.PropertyName._shiv5, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFanOfKnivesVfx.PropertyName._shiv6, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFanOfKnivesVfx.PropertyName._shiv7, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFanOfKnivesVfx.PropertyName._shiv8, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFanOfKnivesVfx.PropertyName._shiv9, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NFanOfKnivesVfx.PropertyName._spawnPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFanOfKnivesVfx.PropertyName._spawnTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFanOfKnivesVfx.PropertyName._fanTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv1, Variant.From<Node2D>(ref this._shiv1));
    info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv2, Variant.From<Node2D>(ref this._shiv2));
    info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv3, Variant.From<Node2D>(ref this._shiv3));
    info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv4, Variant.From<Node2D>(ref this._shiv4));
    info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv5, Variant.From<Node2D>(ref this._shiv5));
    info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv6, Variant.From<Node2D>(ref this._shiv6));
    info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv7, Variant.From<Node2D>(ref this._shiv7));
    info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv8, Variant.From<Node2D>(ref this._shiv8));
    info.AddProperty(NFanOfKnivesVfx.PropertyName._shiv9, Variant.From<Node2D>(ref this._shiv9));
    info.AddProperty(NFanOfKnivesVfx.PropertyName._spawnPosition, Variant.From<Vector2>(ref this._spawnPosition));
    info.AddProperty(NFanOfKnivesVfx.PropertyName._spawnTween, Variant.From<Tween>(ref this._spawnTween));
    info.AddProperty(NFanOfKnivesVfx.PropertyName._fanTween, Variant.From<Tween>(ref this._fanTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv1, ref variant1))
      this._shiv1 = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv2, ref variant2))
      this._shiv2 = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv3, ref variant3))
      this._shiv3 = ((Variant) ref variant3).As<Node2D>();
    Variant variant4;
    if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv4, ref variant4))
      this._shiv4 = ((Variant) ref variant4).As<Node2D>();
    Variant variant5;
    if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv5, ref variant5))
      this._shiv5 = ((Variant) ref variant5).As<Node2D>();
    Variant variant6;
    if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv6, ref variant6))
      this._shiv6 = ((Variant) ref variant6).As<Node2D>();
    Variant variant7;
    if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv7, ref variant7))
      this._shiv7 = ((Variant) ref variant7).As<Node2D>();
    Variant variant8;
    if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv8, ref variant8))
      this._shiv8 = ((Variant) ref variant8).As<Node2D>();
    Variant variant9;
    if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._shiv9, ref variant9))
      this._shiv9 = ((Variant) ref variant9).As<Node2D>();
    Variant variant10;
    if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._spawnPosition, ref variant10))
      this._spawnPosition = ((Variant) ref variant10).As<Vector2>();
    Variant variant11;
    if (info.TryGetProperty(NFanOfKnivesVfx.PropertyName._spawnTween, ref variant11))
      this._spawnTween = ((Variant) ref variant11).As<Tween>();
    Variant variant12;
    if (!info.TryGetProperty(NFanOfKnivesVfx.PropertyName._fanTween, ref variant12))
      return;
    this._fanTween = ((Variant) ref variant12).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _shiv1 = StringName.op_Implicit(nameof (_shiv1));
    public static readonly StringName _shiv2 = StringName.op_Implicit(nameof (_shiv2));
    public static readonly StringName _shiv3 = StringName.op_Implicit(nameof (_shiv3));
    public static readonly StringName _shiv4 = StringName.op_Implicit(nameof (_shiv4));
    public static readonly StringName _shiv5 = StringName.op_Implicit(nameof (_shiv5));
    public static readonly StringName _shiv6 = StringName.op_Implicit(nameof (_shiv6));
    public static readonly StringName _shiv7 = StringName.op_Implicit(nameof (_shiv7));
    public static readonly StringName _shiv8 = StringName.op_Implicit(nameof (_shiv8));
    public static readonly StringName _shiv9 = StringName.op_Implicit(nameof (_shiv9));
    public static readonly StringName _spawnPosition = StringName.op_Implicit(nameof (_spawnPosition));
    public static readonly StringName _spawnTween = StringName.op_Implicit(nameof (_spawnTween));
    public static readonly StringName _fanTween = StringName.op_Implicit(nameof (_fanTween));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
