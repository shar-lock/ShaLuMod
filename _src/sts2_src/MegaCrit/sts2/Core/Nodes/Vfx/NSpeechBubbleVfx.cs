// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSpeechBubbleVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NSpeechBubbleVfx.cs")]
public class NSpeechBubbleVfx : Control
{
  private static readonly StringName _h = new StringName("h");
  private static readonly StringName _s = new StringName("s");
  private static readonly StringName _v = new StringName("v");
  private Control _container;
  private MegaRichTextLabel _label;
  private Node2D _contents;
  private Sprite2D _bubble;
  private Sprite2D _shadow;
  private ShaderMaterial _hsv;
  private const string _path = "res://scenes/vfx/vfx_speech_bubble.tscn";
  private Tween? _tween;
  private const float _spawnProportionToTopOfHitbox = 0.75f;
  private const float _spawnProportionToEdgeOfHitbox = 0.75f;
  private Vector2 _startPos;
  private VfxColor _vfxColor;
  private DialogueStyle _style;
  private DialogueSide _side;
  private string _text;
  private float _elapsedTime = 3.14f;
  private const float _waveFrequency = 4.5f;
  private const float _waveAmplitude = 2f;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/vfx/vfx_speech_bubble.tscn");
    }
  }

  public double SecondsToDisplay { get; private set; }

  public static NSpeechBubbleVfx? Create(
    string text,
    Creature speaker,
    double secondsToDisplay,
    VfxColor vfxColor = VfxColor.White)
  {
    if (TestMode.IsOn)
      return (NSpeechBubbleVfx) null;
    DialogueSide dialogueSide;
    switch (speaker.Side)
    {
      case CombatSide.Player:
        dialogueSide = DialogueSide.Left;
        break;
      case CombatSide.Enemy:
        dialogueSide = DialogueSide.Right;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    DialogueSide side = dialogueSide;
    NSpeechBubbleVfx nspeechBubbleVfx = NSpeechBubbleVfx.CreateInternal(text, side, secondsToDisplay);
    nspeechBubbleVfx._startPos = NSpeechBubbleVfx.GetCreatureSpeechPosition(speaker);
    nspeechBubbleVfx._vfxColor = vfxColor;
    return nspeechBubbleVfx;
  }

  public static NSpeechBubbleVfx? Create(
    string text,
    DialogueSide side,
    Vector2 globalPosition,
    double secondsToDisplay,
    VfxColor vfxColor = VfxColor.White)
  {
    if (TestMode.IsOn)
      return (NSpeechBubbleVfx) null;
    NSpeechBubbleVfx nspeechBubbleVfx = NSpeechBubbleVfx.CreateInternal(text, side, secondsToDisplay);
    nspeechBubbleVfx._startPos = globalPosition;
    nspeechBubbleVfx._vfxColor = vfxColor;
    return nspeechBubbleVfx;
  }

  public override void _Ready()
  {
    this._contents = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%Contents"));
    this._bubble = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("%Bubble"));
    this._shadow = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("%Shadow"));
    this._container = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Container"));
    this._label = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Text"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._bubble).Material;
    this.SetSpeechBubbleColor();
    this.GlobalPosition = this._startPos;
    if (this._side == DialogueSide.Right)
    {
      this._container.Position = new Vector2(-this._container.Size.X - this._container.Position.X, this._container.Position.Y);
      this._bubble.FlipH = true;
      this._shadow.FlipH = true;
    }
    TaskHelper.RunSafely(this.AnimateSpeechBubble());
  }

  private void SetSpeechBubbleColor()
  {
    switch (this._vfxColor)
    {
      case VfxColor.Red:
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, Variant.op_Implicit(0.49f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, Variant.op_Implicit(2.5f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, Variant.op_Implicit(0.38f));
        break;
      case VfxColor.Green:
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, Variant.op_Implicit(0.8f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, Variant.op_Implicit(1.5f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, Variant.op_Implicit(0.6f));
        break;
      case VfxColor.Blue:
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, Variant.op_Implicit(0.05f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, Variant.op_Implicit(1.3f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, Variant.op_Implicit(0.55f));
        break;
      case VfxColor.Purple:
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, Variant.op_Implicit(0.3f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, Variant.op_Implicit(0.6f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, Variant.op_Implicit(0.5f));
        break;
      case VfxColor.Black:
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, Variant.op_Implicit(1f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, Variant.op_Implicit(0.25f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, Variant.op_Implicit(0.3f));
        break;
      case VfxColor.Orange:
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, Variant.op_Implicit(0.6f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, Variant.op_Implicit(4f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, Variant.op_Implicit(0.5f));
        break;
      case VfxColor.Swamp:
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, Variant.op_Implicit(0.72f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, Variant.op_Implicit(1.7f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, Variant.op_Implicit(0.5f));
        break;
      case VfxColor.DarkGray:
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, Variant.op_Implicit(0.093f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, Variant.op_Implicit(0.2f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, Variant.op_Implicit(0.4f));
        break;
      default:
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, Variant.op_Implicit(1f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, Variant.op_Implicit(0.9f));
        this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, Variant.op_Implicit(0.5f));
        break;
    }
  }

  public override void _ExitTree() => this._tween?.Kill();

  private async Task AnimateSpeechBubble()
  {
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._label.Text = $"[center][fly_in offset_x={(float) (60.0 * (this._side == DialogueSide.Left ? -1.0 : 1.0))} offset_y=40]{this._text}[/fly_in][/center]";
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.4).From(Variant.op_Implicit(0.0f));
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("visible_ratio"), Variant.op_Implicit(1f), 0.4).From(Variant.op_Implicit(0.0f));
    this._tween.TweenProperty((GodotObject) this._bubble, NodePath.op_Implicit("scale"), Variant.op_Implicit(new Vector2(0.75f, 0.75f)), 0.5).From(Variant.op_Implicit(new Vector2(0.25f, 0.25f))).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("rotation_degrees"), Variant.op_Implicit(0.0f), 0.3).From(Variant.op_Implicit(7f)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    this._tween.Chain();
    this._tween.TweenInterval(Math.Max(this.SecondsToDisplay - 1.0, 1.0));
    this._tween.Chain();
    await this.AnimOutInternal();
  }

  public async Task AnimOut()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    await this.AnimOutInternal();
  }

  private async Task AnimOutInternal()
  {
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentBlack), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    bool flag = await this._tween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  private static NSpeechBubbleVfx CreateInternal(
    string text,
    DialogueSide side,
    double secondsToDisplay)
  {
    NSpeechBubbleVfx nspeechBubbleVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/vfx_speech_bubble.tscn").Instantiate<NSpeechBubbleVfx>((PackedScene.GenEditState) 0L);
    nspeechBubbleVfx._side = side;
    nspeechBubbleVfx._text = text;
    nspeechBubbleVfx.SecondsToDisplay = secondsToDisplay;
    return nspeechBubbleVfx;
  }

  public override void _Process(double delta)
  {
    this._elapsedTime += (float) delta * 4.5f;
    this._contents.Position = new Vector2(0.0f, Mathf.Sin(this._elapsedTime) * 2f);
  }

  private static Vector2 GetCreatureSpeechPosition(Creature speaker)
  {
    NCreature creatureNode = speaker.GetCreatureNode();
    if (creatureNode == null)
      return Vector2.Zero;
    if (creatureNode.Visuals.TalkPosition != null)
      return ((Node2D) creatureNode.Visuals.TalkPosition).GlobalPosition;
    Vector2 creatureSpeechPosition = Vector2.op_Addition(creatureNode.VfxSpawnPosition, new Vector2(0.0f, (float) (-(double) creatureNode.Hitbox.Size.Y * 0.5 * 0.75)));
    if (speaker.Side == CombatSide.Player)
      creatureSpeechPosition.X += creatureNode.Hitbox.Size.X * 0.75f;
    else
      creatureSpeechPosition.X -= creatureNode.Hitbox.Size.X * 0.75f;
    return creatureSpeechPosition;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NSpeechBubbleVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("side"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("globalPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("secondsToDisplay"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("vfxColor"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSpeechBubbleVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSpeechBubbleVfx.MethodName.SetSpeechBubbleColor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSpeechBubbleVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSpeechBubbleVfx.MethodName.CreateInternal, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("side"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("secondsToDisplay"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSpeechBubbleVfx.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSpeechBubbleVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 5)
    {
      NSpeechBubbleVfx nspeechBubbleVfx = NSpeechBubbleVfx.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<DialogueSide>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[3]), VariantUtils.ConvertTo<VfxColor>(ref ((NativeVariantPtrArgs) ref args)[4]));
      ret = VariantUtils.CreateFrom<NSpeechBubbleVfx>(ref nspeechBubbleVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NSpeechBubbleVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSpeechBubbleVfx.MethodName.SetSpeechBubbleColor) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetSpeechBubbleColor();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSpeechBubbleVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSpeechBubbleVfx.MethodName.CreateInternal) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NSpeechBubbleVfx nspeechBubbleVfx = NSpeechBubbleVfx.CreateInternal(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<DialogueSide>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NSpeechBubbleVfx>(ref nspeechBubbleVfx);
      return true;
    }
    if (!StringName.op_Equality(ref method, NSpeechBubbleVfx.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSpeechBubbleVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 5)
    {
      NSpeechBubbleVfx nspeechBubbleVfx = NSpeechBubbleVfx.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<DialogueSide>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[3]), VariantUtils.ConvertTo<VfxColor>(ref ((NativeVariantPtrArgs) ref args)[4]));
      ret = VariantUtils.CreateFrom<NSpeechBubbleVfx>(ref nspeechBubbleVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NSpeechBubbleVfx.MethodName.CreateInternal) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NSpeechBubbleVfx nspeechBubbleVfx = NSpeechBubbleVfx.CreateInternal(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<DialogueSide>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NSpeechBubbleVfx>(ref nspeechBubbleVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSpeechBubbleVfx.MethodName.Create) || StringName.op_Equality(ref method, NSpeechBubbleVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSpeechBubbleVfx.MethodName.SetSpeechBubbleColor) || StringName.op_Equality(ref method, NSpeechBubbleVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NSpeechBubbleVfx.MethodName.CreateInternal) || StringName.op_Equality(ref method, NSpeechBubbleVfx.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName.SecondsToDisplay))
    {
      this.SecondsToDisplay = VariantUtils.ConvertTo<double>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._container))
    {
      this._container = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._contents))
    {
      this._contents = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._bubble))
    {
      this._bubble = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._shadow))
    {
      this._shadow = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._startPos))
    {
      this._startPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._vfxColor))
    {
      this._vfxColor = VariantUtils.ConvertTo<VfxColor>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._style))
    {
      this._style = VariantUtils.ConvertTo<DialogueStyle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._side))
    {
      this._side = VariantUtils.ConvertTo<DialogueSide>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._text))
    {
      this._text = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._elapsedTime))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._elapsedTime = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName.SecondsToDisplay))
    {
      ref godot_variant local = ref value;
      double secondsToDisplay = this.SecondsToDisplay;
      godot_variant from = VariantUtils.CreateFrom<double>(ref secondsToDisplay);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._container))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._container);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._contents))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._contents);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._bubble))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._bubble);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._shadow))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._shadow);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._startPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._startPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._vfxColor))
    {
      value = VariantUtils.CreateFrom<VfxColor>(ref this._vfxColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._style))
    {
      value = VariantUtils.CreateFrom<DialogueStyle>(ref this._style);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._side))
    {
      value = VariantUtils.CreateFrom<DialogueSide>(ref this._side);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._text))
    {
      value = VariantUtils.CreateFrom<string>(ref this._text);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSpeechBubbleVfx.PropertyName._elapsedTime))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._elapsedTime);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSpeechBubbleVfx.PropertyName._container, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSpeechBubbleVfx.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSpeechBubbleVfx.PropertyName._contents, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSpeechBubbleVfx.PropertyName._bubble, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSpeechBubbleVfx.PropertyName._shadow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSpeechBubbleVfx.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSpeechBubbleVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NSpeechBubbleVfx.PropertyName._startPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NSpeechBubbleVfx.PropertyName._vfxColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NSpeechBubbleVfx.PropertyName._style, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NSpeechBubbleVfx.PropertyName._side, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NSpeechBubbleVfx.PropertyName._text, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NSpeechBubbleVfx.PropertyName.SecondsToDisplay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NSpeechBubbleVfx.PropertyName._elapsedTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName secondsToDisplay1 = NSpeechBubbleVfx.PropertyName.SecondsToDisplay;
    double secondsToDisplay2 = this.SecondsToDisplay;
    Variant variant = Variant.From<double>(ref secondsToDisplay2);
    serializationInfo.AddProperty(secondsToDisplay1, variant);
    info.AddProperty(NSpeechBubbleVfx.PropertyName._container, Variant.From<Control>(ref this._container));
    info.AddProperty(NSpeechBubbleVfx.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
    info.AddProperty(NSpeechBubbleVfx.PropertyName._contents, Variant.From<Node2D>(ref this._contents));
    info.AddProperty(NSpeechBubbleVfx.PropertyName._bubble, Variant.From<Sprite2D>(ref this._bubble));
    info.AddProperty(NSpeechBubbleVfx.PropertyName._shadow, Variant.From<Sprite2D>(ref this._shadow));
    info.AddProperty(NSpeechBubbleVfx.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NSpeechBubbleVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NSpeechBubbleVfx.PropertyName._startPos, Variant.From<Vector2>(ref this._startPos));
    info.AddProperty(NSpeechBubbleVfx.PropertyName._vfxColor, Variant.From<VfxColor>(ref this._vfxColor));
    info.AddProperty(NSpeechBubbleVfx.PropertyName._style, Variant.From<DialogueStyle>(ref this._style));
    info.AddProperty(NSpeechBubbleVfx.PropertyName._side, Variant.From<DialogueSide>(ref this._side));
    info.AddProperty(NSpeechBubbleVfx.PropertyName._text, Variant.From<string>(ref this._text));
    info.AddProperty(NSpeechBubbleVfx.PropertyName._elapsedTime, Variant.From<float>(ref this._elapsedTime));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName.SecondsToDisplay, ref variant1))
      this.SecondsToDisplay = ((Variant) ref variant1).As<double>();
    Variant variant2;
    if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._container, ref variant2))
      this._container = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._label, ref variant3))
      this._label = ((Variant) ref variant3).As<MegaRichTextLabel>();
    Variant variant4;
    if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._contents, ref variant4))
      this._contents = ((Variant) ref variant4).As<Node2D>();
    Variant variant5;
    if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._bubble, ref variant5))
      this._bubble = ((Variant) ref variant5).As<Sprite2D>();
    Variant variant6;
    if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._shadow, ref variant6))
      this._shadow = ((Variant) ref variant6).As<Sprite2D>();
    Variant variant7;
    if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._hsv, ref variant7))
      this._hsv = ((Variant) ref variant7).As<ShaderMaterial>();
    Variant variant8;
    if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._tween, ref variant8))
      this._tween = ((Variant) ref variant8).As<Tween>();
    Variant variant9;
    if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._startPos, ref variant9))
      this._startPos = ((Variant) ref variant9).As<Vector2>();
    Variant variant10;
    if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._vfxColor, ref variant10))
      this._vfxColor = ((Variant) ref variant10).As<VfxColor>();
    Variant variant11;
    if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._style, ref variant11))
      this._style = ((Variant) ref variant11).As<DialogueStyle>();
    Variant variant12;
    if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._side, ref variant12))
      this._side = ((Variant) ref variant12).As<DialogueSide>();
    Variant variant13;
    if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._text, ref variant13))
      this._text = ((Variant) ref variant13).As<string>();
    Variant variant14;
    if (!info.TryGetProperty(NSpeechBubbleVfx.PropertyName._elapsedTime, ref variant14))
      return;
    this._elapsedTime = ((Variant) ref variant14).As<float>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetSpeechBubbleColor = StringName.op_Implicit(nameof (SetSpeechBubbleColor));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName CreateInternal = StringName.op_Implicit(nameof (CreateInternal));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName SecondsToDisplay = StringName.op_Implicit(nameof (SecondsToDisplay));
    public static readonly StringName _container = StringName.op_Implicit(nameof (_container));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _contents = StringName.op_Implicit(nameof (_contents));
    public static readonly StringName _bubble = StringName.op_Implicit(nameof (_bubble));
    public static readonly StringName _shadow = StringName.op_Implicit(nameof (_shadow));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _startPos = StringName.op_Implicit(nameof (_startPos));
    public static readonly StringName _vfxColor = StringName.op_Implicit(nameof (_vfxColor));
    public static readonly StringName _style = StringName.op_Implicit(nameof (_style));
    public static readonly StringName _side = StringName.op_Implicit(nameof (_side));
    public static readonly StringName _text = StringName.op_Implicit(nameof (_text));
    public static readonly StringName _elapsedTime = StringName.op_Implicit(nameof (_elapsedTime));
  }

  public class SignalName : Control.SignalName
  {
  }
}
