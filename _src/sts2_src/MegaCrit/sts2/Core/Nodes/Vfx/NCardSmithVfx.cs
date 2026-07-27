// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NCardSmithVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NCardSmithVfx.cs")]
public class NCardSmithVfx : Node2D
{
  private Tween? _tween;
  public const string smithSfx = "card_smith.mp3";
  private bool _willPlaySfx = true;
  private readonly List<CardModel> _cards = new List<CardModel>();
  private NCard? _cardNode;
  private Control _cardContainer;

  private static string ScenePath => SceneHelper.GetScenePath("vfx/vfx_card_smith");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        NCardSmithVfx.ScenePath,
        TmpSfx.GetPath("card_smith.mp3")
      });
    }
  }

  public float SfxVolume { get; set; } = 1f;

  public static NCardSmithVfx? Create(IEnumerable<CardModel> cards, bool playSfx = true)
  {
    NCardSmithVfx ncardSmithVfx = NCardSmithVfx.Create();
    if (ncardSmithVfx == null)
      return (NCardSmithVfx) null;
    ncardSmithVfx._cards.AddRange(cards);
    ncardSmithVfx._willPlaySfx = playSfx;
    return ncardSmithVfx;
  }

  public static NCardSmithVfx? Create(NCard card, bool playSfx = true)
  {
    NCardSmithVfx ncardSmithVfx = NCardSmithVfx.Create();
    if (ncardSmithVfx == null)
      return (NCardSmithVfx) null;
    ncardSmithVfx._willPlaySfx = playSfx;
    ncardSmithVfx._cardNode = card;
    return ncardSmithVfx;
  }

  public static NCardSmithVfx? Create()
  {
    return TestMode.IsOn ? (NCardSmithVfx) null : PreloadManager.Cache.GetScene(NCardSmithVfx.ScenePath).Instantiate<NCardSmithVfx>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    if (this._cardNode != null)
    {
      this.GlobalPosition = this._cardNode.GlobalPosition;
      this.GlobalScale = this._cardNode.Scale;
      TaskHelper.RunSafely(this.PlayAnimation());
    }
    else if (this._cards.Count > 0)
      TaskHelper.RunSafely(this.PlayAnimation((IEnumerable<CardModel>) this._cards));
    else
      TaskHelper.RunSafely(this.PlayAnimation());
  }

  public override void _ExitTree() => this._tween?.Kill();

  private async Task PlayAnimation()
  {
    if (this._willPlaySfx)
      NDebugAudioManager.Instance?.Play("card_smith.mp3", this.SfxVolume, PitchVariance.Small);
    this._tween = ((Node) this).CreateTween();
    this._tween.Parallel().TweenCallback(Callable.From((Action) (() => this.PlaySubParticles((Node) ((Node) this).GetNode<Control>(NodePath.op_Implicit("Spark1"))))));
    this._tween.TweenInterval(0.25);
    this._tween.Chain().TweenCallback(Callable.From((Action) (() => this.PlaySubParticles((Node) ((Node) this).GetNode<Control>(NodePath.op_Implicit("Spark2"))))));
    this._tween.TweenInterval(0.25);
    this._tween.Chain().TweenCallback(Callable.From((Action) (() => this.PlaySubParticles((Node) ((Node) this).GetNode<Control>(NodePath.op_Implicit("Spark3"))))));
    this._tween.TweenInterval(0.40000000596046448);
    bool flag = await this._tween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  private async Task PlayAnimation(IEnumerable<CardModel> cards)
  {
    Control node = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CardContainer"));
    List<NCard> cardNodes = new List<NCard>();
    foreach (CardModel card in cards)
    {
      NCard child = NCard.Create(card);
      ((Node) node).AddChildSafely((Node) child);
      child.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
      cardNodes.Add(child);
    }
    this._tween = ((Node) this).CreateTween();
    foreach (NCard ncard in cardNodes)
      this._tween.Parallel().TweenProperty((GodotObject) ncard, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1f)), 0.25).From(Variant.op_Implicit(Vector2.Zero)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    if (this._willPlaySfx)
      this._tween.Chain().TweenCallback(Callable.From((Action) (() => NDebugAudioManager.Instance?.Play("card_smith.mp3", this.SfxVolume, PitchVariance.Small))));
    this._tween.Parallel().TweenCallback(Callable.From((Action) (() => this.PlaySubParticles((Node) ((Node) this).GetNode<Control>(NodePath.op_Implicit("Spark1"))))));
    this._tween.Parallel().TweenCallback(Callable.From((Action) (() => NGame.Instance?.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, 180f + Rng.Chaotic.NextFloat(-10f, 10f)))));
    foreach (NCard ncard in cardNodes)
      this._tween.Parallel().TweenProperty((GodotObject) ncard, NodePath.op_Implicit("rotation_degrees"), Variant.op_Implicit(20), 0.15000000596046448).SetTrans((Tween.TransitionType) 6L).SetEase((Tween.EaseType) 1L);
    this._tween.TweenInterval(0.10000000149011612);
    this._tween.Chain().TweenCallback(Callable.From((Action) (() => this.PlaySubParticles((Node) ((Node) this).GetNode<Control>(NodePath.op_Implicit("Spark2"))))));
    foreach (NCard ncard in cardNodes)
      this._tween.Parallel().TweenProperty((GodotObject) ncard, NodePath.op_Implicit("rotation_degrees"), Variant.op_Implicit(-10), 0.15000000596046448).SetTrans((Tween.TransitionType) 6L).SetEase((Tween.EaseType) 1L);
    this._tween.Parallel().TweenCallback(Callable.From((Action) (() => NGame.Instance?.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, 180f + Rng.Chaotic.NextFloat(-10f, 10f)))));
    this._tween.TweenInterval(0.10000000149011612);
    this._tween.Chain().TweenCallback(Callable.From((Action) (() => this.PlaySubParticles((Node) ((Node) this).GetNode<Control>(NodePath.op_Implicit("Spark3"))))));
    foreach (NCard ncard in cardNodes)
      this._tween.Parallel().TweenProperty((GodotObject) ncard, NodePath.op_Implicit("rotation_degrees"), Variant.op_Implicit(5), 0.15000000596046448).SetTrans((Tween.TransitionType) 6L).SetEase((Tween.EaseType) 1L);
    this._tween.Parallel().TweenCallback(Callable.From((Action) (() => NGame.Instance?.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, 180f + Rng.Chaotic.NextFloat(-10f, 10f)))));
    this._tween.TweenInterval(0.34999999403953552);
    if (!await this._tween.AwaitFinished((Node) this))
      cardNodes = (List<NCard>) null;
    else if (((Node) cardNodes[0]).IsInsideTree() && this._cards[0].Pile == null)
    {
      this._tween = ((Node) this).CreateTween();
      foreach (NCard ncard in cardNodes)
        this._tween.SetParallel(true).TweenProperty((GodotObject) ncard, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.Zero), 0.20000000298023224);
      int num = await this._tween.AwaitFinished((Node) this) ? 1 : 0;
      cardNodes = (List<NCard>) null;
    }
    else if (!((Node) cardNodes[0]).IsInsideTree())
    {
      cardNodes = (List<NCard>) null;
    }
    else
    {
      for (int i = 0; i < cardNodes.Count; ++i)
      {
        cardNodes[i].Model.Pile.Type.GetTargetPosition(cardNodes[i]);
        Vector2 globalPosition = cardNodes[i].GlobalPosition;
        ((Node) cardNodes[i]).Reparent((Node) this, true);
        cardNodes[i].GlobalPosition = globalPosition;
        NCardFlyVfx child = NCardFlyVfx.Create(cardNodes[i], cardNodes[i].Model.Pile.Type, false, cardNodes[i].Model.Owner.Character.TrailPath);
        NRun instance = NRun.Instance;
        if (instance != null)
          instance.GlobalUi.TopBar.TrailContainer.AddChildSafely((Node) child);
        if (child.SwooshAwayCompletion != null && i == cardNodes.Count - 1)
          await child.SwooshAwayCompletion.Task;
      }
      ((Node) this).QueueFreeSafely();
      cardNodes = (List<NCard>) null;
    }
  }

  private void PlaySubParticles(Node node)
  {
    foreach (GpuParticles2D gpuParticles2D in ((IEnumerable) node.GetChildren(false)).OfType<GpuParticles2D>())
      gpuParticles2D.Restart();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NCardSmithVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("playSfx"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardSmithVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardSmithVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardSmithVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardSmithVfx.MethodName.PlaySubParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardSmithVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NCardSmithVfx ncardSmithVfx = NCardSmithVfx.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NCardSmithVfx>(ref ncardSmithVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NCardSmithVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCardSmithVfx ncardSmithVfx = NCardSmithVfx.Create();
      ret = VariantUtils.CreateFrom<NCardSmithVfx>(ref ncardSmithVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NCardSmithVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardSmithVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardSmithVfx.MethodName.PlaySubParticles) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.PlaySubParticles(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardSmithVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NCardSmithVfx ncardSmithVfx = NCardSmithVfx.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NCardSmithVfx>(ref ncardSmithVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NCardSmithVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCardSmithVfx ncardSmithVfx = NCardSmithVfx.Create();
      ret = VariantUtils.CreateFrom<NCardSmithVfx>(ref ncardSmithVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardSmithVfx.MethodName.Create) || StringName.op_Equality(ref method, NCardSmithVfx.MethodName._Ready) || StringName.op_Equality(ref method, NCardSmithVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NCardSmithVfx.MethodName.PlaySubParticles) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardSmithVfx.PropertyName.SfxVolume))
    {
      this.SfxVolume = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardSmithVfx.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardSmithVfx.PropertyName._willPlaySfx))
    {
      this._willPlaySfx = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardSmithVfx.PropertyName._cardNode))
    {
      this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardSmithVfx.PropertyName._cardContainer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._cardContainer = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardSmithVfx.PropertyName.SfxVolume))
    {
      ref godot_variant local = ref value;
      float sfxVolume = this.SfxVolume;
      godot_variant from = VariantUtils.CreateFrom<float>(ref sfxVolume);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardSmithVfx.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardSmithVfx.PropertyName._willPlaySfx))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._willPlaySfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardSmithVfx.PropertyName._cardNode))
    {
      value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardSmithVfx.PropertyName._cardContainer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._cardContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardSmithVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardSmithVfx.PropertyName._willPlaySfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardSmithVfx.PropertyName._cardNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardSmithVfx.PropertyName._cardContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardSmithVfx.PropertyName.SfxVolume, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName sfxVolume1 = NCardSmithVfx.PropertyName.SfxVolume;
    float sfxVolume2 = this.SfxVolume;
    Variant variant = Variant.From<float>(ref sfxVolume2);
    serializationInfo.AddProperty(sfxVolume1, variant);
    info.AddProperty(NCardSmithVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NCardSmithVfx.PropertyName._willPlaySfx, Variant.From<bool>(ref this._willPlaySfx));
    info.AddProperty(NCardSmithVfx.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
    info.AddProperty(NCardSmithVfx.PropertyName._cardContainer, Variant.From<Control>(ref this._cardContainer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardSmithVfx.PropertyName.SfxVolume, ref variant1))
      this.SfxVolume = ((Variant) ref variant1).As<float>();
    Variant variant2;
    if (info.TryGetProperty(NCardSmithVfx.PropertyName._tween, ref variant2))
      this._tween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (info.TryGetProperty(NCardSmithVfx.PropertyName._willPlaySfx, ref variant3))
      this._willPlaySfx = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (info.TryGetProperty(NCardSmithVfx.PropertyName._cardNode, ref variant4))
      this._cardNode = ((Variant) ref variant4).As<NCard>();
    Variant variant5;
    if (!info.TryGetProperty(NCardSmithVfx.PropertyName._cardContainer, ref variant5))
      return;
    this._cardContainer = ((Variant) ref variant5).As<Control>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName PlaySubParticles = StringName.op_Implicit(nameof (PlaySubParticles));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName SfxVolume = StringName.op_Implicit(nameof (SfxVolume));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _willPlaySfx = StringName.op_Implicit(nameof (_willPlaySfx));
    public static readonly StringName _cardNode = StringName.op_Implicit(nameof (_cardNode));
    public static readonly StringName _cardContainer = StringName.op_Implicit(nameof (_cardContainer));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
