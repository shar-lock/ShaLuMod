// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NThoughtBubbleVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NThoughtBubbleVfx.cs")]
public class NThoughtBubbleVfx : Control
{
  private Control _container;
  private MegaRichTextLabel _label;
  private TextureRect _textureRect;
  private Node2D _contents;
  private Node2D _tail;
  private const string _path = "res://scenes/vfx/vfx_thought_bubble.tscn";
  private const float _spawnProportionToTopOfHitbox = 0.75f;
  private const float _spawnProportionToEdgeOfHitbox = 0.75f;
  private Vector2? _startPos;
  private DialogueStyle _style;
  private DialogueSide _side;
  private string? _text;
  private Texture2D? _texture;
  private double? _secondsToDisplay;
  private Tween? _tween;
  private CancellationTokenSource? _cts;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/vfx/vfx_thought_bubble.tscn");
    }
  }

  public static NThoughtBubbleVfx? Create(string text, Creature speaker, double? secondsToDisplay)
  {
    if (TestMode.IsOn)
      return (NThoughtBubbleVfx) null;
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
    NThoughtBubbleVfx nthoughtBubbleVfx = NThoughtBubbleVfx.CreateInternal(text, (Texture2D) null, side, secondsToDisplay);
    nthoughtBubbleVfx._startPos = new Vector2?(NThoughtBubbleVfx.GetCreatureSpeechPosition(speaker));
    return nthoughtBubbleVfx;
  }

  public static NThoughtBubbleVfx? Create(string text, DialogueSide side, double? secondsToDisplay)
  {
    return TestMode.IsOn ? (NThoughtBubbleVfx) null : NThoughtBubbleVfx.CreateInternal(text, (Texture2D) null, side, secondsToDisplay);
  }

  public static NThoughtBubbleVfx? Create(
    Texture2D texture,
    DialogueSide side,
    double? secondsToDisplay)
  {
    return TestMode.IsOn ? (NThoughtBubbleVfx) null : NThoughtBubbleVfx.CreateInternal((string) null, texture, side, secondsToDisplay);
  }

  public override void _Ready()
  {
    this._contents = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%Contents"));
    this._container = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Container"));
    this._label = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Text"));
    this._textureRect = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Image"));
    this._tail = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%Tail"));
    if (this._startPos.HasValue)
      this.GlobalPosition = this._startPos.Value;
    if (this._side == DialogueSide.Right)
    {
      this._container.Position = new Vector2(-this._container.Size.X - this._container.Position.X, this._container.Position.Y);
      this._tail.Scale = new Vector2(-1f, 1f);
    }
    TaskHelper.RunSafely(this.AnimateThoughtBubble());
  }

  private async Task AnimateThoughtBubble()
  {
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    float num = (float) (30.0 * (this._side == DialogueSide.Left ? -1.0 : 1.0));
    if (this._text != null)
      this._label.Text = $"[center][fly_in offset_x={num} offset_y=40]{this._text}[/fly_in][/center]";
    ((CanvasItem) this._label).Visible = this._text != null;
    if (this._texture != null)
      this._textureRect.Texture = this._texture;
    ((CanvasItem) this._textureRect).Visible = this._texture != null;
    this.Scale = Vector2.op_Multiply(Vector2.One, 0.75f);
    ((CanvasItem) this).Modulate = StsColors.transparentWhite;
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("visible_ratio"), Variant.op_Implicit(1f), 0.4).From(Variant.op_Implicit(0.0f));
    this._tween.TweenProperty((GodotObject) this._textureRect, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.4).From(Variant.op_Implicit(0.0f));
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    if (!this._secondsToDisplay.HasValue)
      return;
    double seconds = Math.Max(this._secondsToDisplay.Value, 1.0);
    this._cts = new CancellationTokenSource();
    await Cmd.Wait((float) seconds, this._cts.Token);
    await this.GoAway();
  }

  public override void _ExitTree()
  {
    this._tween?.Kill();
    this._cts?.Cancel();
  }

  public async Task GoAway()
  {
    if (!GodotObject.IsInstanceValid((GodotObject) this))
      return;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    bool flag = await this._tween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  private static NThoughtBubbleVfx CreateInternal(
    string? text,
    Texture2D? texture,
    DialogueSide side,
    double? secondsToDisplay)
  {
    NThoughtBubbleVfx nthoughtBubbleVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/vfx_thought_bubble.tscn").Instantiate<NThoughtBubbleVfx>((PackedScene.GenEditState) 0L);
    nthoughtBubbleVfx._side = side;
    nthoughtBubbleVfx._text = text;
    nthoughtBubbleVfx._texture = texture;
    nthoughtBubbleVfx._secondsToDisplay = secondsToDisplay;
    return nthoughtBubbleVfx;
  }

  public static Vector2 GetCreatureSpeechPosition(Creature speaker)
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

  public void SetTexture(Texture2D texture)
  {
    this._texture = this._texture != null ? texture : throw new NotImplementedException("Can't set texture unless thought bubble was initialized with a texture");
    this._textureRect.Texture = texture;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NThoughtBubbleVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NThoughtBubbleVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NThoughtBubbleVfx.MethodName.SetTexture, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("texture"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NThoughtBubbleVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NThoughtBubbleVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NThoughtBubbleVfx.MethodName.SetTexture) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetTexture(VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NThoughtBubbleVfx.MethodName._Ready) || StringName.op_Equality(ref method, NThoughtBubbleVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NThoughtBubbleVfx.MethodName.SetTexture) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._container))
    {
      this._container = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._textureRect))
    {
      this._textureRect = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._contents))
    {
      this._contents = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._tail))
    {
      this._tail = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._style))
    {
      this._style = VariantUtils.ConvertTo<DialogueStyle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._side))
    {
      this._side = VariantUtils.ConvertTo<DialogueSide>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._text))
    {
      this._text = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._texture))
    {
      this._texture = VariantUtils.ConvertTo<Texture2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._container))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._container);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._textureRect))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._textureRect);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._contents))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._contents);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._tail))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._tail);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._style))
    {
      value = VariantUtils.CreateFrom<DialogueStyle>(ref this._style);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._side))
    {
      value = VariantUtils.CreateFrom<DialogueSide>(ref this._side);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._text))
    {
      value = VariantUtils.CreateFrom<string>(ref this._text);
      return true;
    }
    if (StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._texture))
    {
      value = VariantUtils.CreateFrom<Texture2D>(ref this._texture);
      return true;
    }
    if (!StringName.op_Equality(ref name, NThoughtBubbleVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NThoughtBubbleVfx.PropertyName._container, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NThoughtBubbleVfx.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NThoughtBubbleVfx.PropertyName._textureRect, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NThoughtBubbleVfx.PropertyName._contents, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NThoughtBubbleVfx.PropertyName._tail, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NThoughtBubbleVfx.PropertyName._style, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NThoughtBubbleVfx.PropertyName._side, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NThoughtBubbleVfx.PropertyName._text, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NThoughtBubbleVfx.PropertyName._texture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NThoughtBubbleVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NThoughtBubbleVfx.PropertyName._container, Variant.From<Control>(ref this._container));
    info.AddProperty(NThoughtBubbleVfx.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
    info.AddProperty(NThoughtBubbleVfx.PropertyName._textureRect, Variant.From<TextureRect>(ref this._textureRect));
    info.AddProperty(NThoughtBubbleVfx.PropertyName._contents, Variant.From<Node2D>(ref this._contents));
    info.AddProperty(NThoughtBubbleVfx.PropertyName._tail, Variant.From<Node2D>(ref this._tail));
    info.AddProperty(NThoughtBubbleVfx.PropertyName._style, Variant.From<DialogueStyle>(ref this._style));
    info.AddProperty(NThoughtBubbleVfx.PropertyName._side, Variant.From<DialogueSide>(ref this._side));
    info.AddProperty(NThoughtBubbleVfx.PropertyName._text, Variant.From<string>(ref this._text));
    info.AddProperty(NThoughtBubbleVfx.PropertyName._texture, Variant.From<Texture2D>(ref this._texture));
    info.AddProperty(NThoughtBubbleVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._container, ref variant1))
      this._container = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._label, ref variant2))
      this._label = ((Variant) ref variant2).As<MegaRichTextLabel>();
    Variant variant3;
    if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._textureRect, ref variant3))
      this._textureRect = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._contents, ref variant4))
      this._contents = ((Variant) ref variant4).As<Node2D>();
    Variant variant5;
    if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._tail, ref variant5))
      this._tail = ((Variant) ref variant5).As<Node2D>();
    Variant variant6;
    if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._style, ref variant6))
      this._style = ((Variant) ref variant6).As<DialogueStyle>();
    Variant variant7;
    if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._side, ref variant7))
      this._side = ((Variant) ref variant7).As<DialogueSide>();
    Variant variant8;
    if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._text, ref variant8))
      this._text = ((Variant) ref variant8).As<string>();
    Variant variant9;
    if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._texture, ref variant9))
      this._texture = ((Variant) ref variant9).As<Texture2D>();
    Variant variant10;
    if (!info.TryGetProperty(NThoughtBubbleVfx.PropertyName._tween, ref variant10))
      return;
    this._tween = ((Variant) ref variant10).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName SetTexture = StringName.op_Implicit(nameof (SetTexture));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _container = StringName.op_Implicit(nameof (_container));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _textureRect = StringName.op_Implicit(nameof (_textureRect));
    public static readonly StringName _contents = StringName.op_Implicit(nameof (_contents));
    public static readonly StringName _tail = StringName.op_Implicit(nameof (_tail));
    public static readonly StringName _style = StringName.op_Implicit(nameof (_style));
    public static readonly StringName _side = StringName.op_Implicit(nameof (_side));
    public static readonly StringName _text = StringName.op_Implicit(nameof (_text));
    public static readonly StringName _texture = StringName.op_Implicit(nameof (_texture));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
