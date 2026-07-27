// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NIntent
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
using MegaCrit.Sts2.Core.Entities.Intents;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NIntent.cs")]
public class NIntent : Control
{
  private const string _scenePath = "res://scenes/combat/intent.tscn";
  private const float _bobSpeed = 3.14159274f;
  private const float _bobDistance = 10f;
  private const float _bobOffset = 8f;
  private const int _animationFps = 15;
  private Control _intentHolder;
  private Sprite2D _intentSprite;
  private MegaRichTextLabel _valueLabel;
  private CpuParticles2D _intentParticle;
  private Creature _owner;
  private IEnumerable<Creature> _targets;
  private AbstractIntent _intent;
  private float _timeOffset;
  private float _timeAccumulator;
  private bool _isFrozen;
  private string? _animationName;
  private readonly List<Texture2D> _animationFrames = new List<Texture2D>();
  private int? _animationFrame;
  private NCombatRoom? _combatRoom;

  public override void _Ready()
  {
    this._intentHolder = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%IntentHolder"));
    this._intentSprite = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("%Intent"));
    this._valueLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Value"));
    this._intentParticle = ((Node) this).GetNode<CpuParticles2D>(NodePath.op_Implicit("%IntentParticle"));
    ((GodotObject) this).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
    ((CanvasItem) this._intentHolder).Modulate = NCombatUi.IsDebugHidingIntent ? Colors.Transparent : Colors.White;
  }

  public override void _EnterTree()
  {
    CombatManager.Instance.StateTracker.CombatStateChanged += new Action<CombatState>(this.OnCombatStateChanged);
    if (NCombatRoom.Instance == null)
      return;
    this._combatRoom = NCombatRoom.Instance;
    this._combatRoom.Ui.DebugToggleIntent += new Action(this.DebugToggleVisibility);
  }

  private void DebugToggleVisibility()
  {
    ((CanvasItem) this._intentHolder).Modulate = NCombatUi.IsDebugHidingIntent ? Colors.Transparent : Colors.White;
  }

  public override void _ExitTree()
  {
    CombatManager.Instance.StateTracker.CombatStateChanged -= new Action<CombatState>(this.OnCombatStateChanged);
    if (this._combatRoom == null)
      return;
    this._combatRoom.Ui.DebugToggleIntent -= new Action(this.DebugToggleVisibility);
  }

  public void UpdateIntent(AbstractIntent intent, IEnumerable<Creature> targets, Creature owner)
  {
    this._owner = owner;
    this._targets = targets;
    this._intent = intent;
    this.UpdateVisuals();
  }

  private void OnCombatStateChanged(CombatState _)
  {
    if (this._isFrozen)
      return;
    this.UpdateVisuals();
  }

  private void UpdateVisuals()
  {
    string animation = this._intent.GetAnimation(this._targets, this._owner);
    if (this._animationName != animation)
    {
      this._animationName = animation;
      this._animationFrame = new int?();
      this._timeAccumulator = 0.0f;
      int animationFrameCount = IntentAnimData.GetAnimationFrameCount(animation);
      this._animationFrames.Clear();
      for (int frame = 0; frame < animationFrameCount; ++frame)
        this._animationFrames.Add(PreloadManager.Cache.GetTexture2D(IntentAnimData.GetAnimationFrame(animation, frame)));
    }
    this._intentParticle.Texture = this._intent.GetTexture(this._targets, this._owner);
    MegaRichTextLabel valueLabel = this._valueLabel;
    string str;
    switch (this._intent)
    {
      case AttackIntent attackIntent:
        str = attackIntent.GetIntentLabel(this._targets, this._owner).GetFormattedText() ?? "";
        break;
      case StatusIntent _:
        str = this._intent.GetIntentLabel(this._targets, this._owner).GetFormattedText() ?? "";
        break;
      default:
        str = string.Empty;
        break;
    }
    valueLabel.Text = str;
  }

  public override void _Process(double delta)
  {
    this._intentHolder.Position = Vector2.op_Multiply(Vector2.Up, (float) ((double) Mathf.Sin((float) ((double) Time.GetTicksMsec() * (1.0 / 1000.0) * 3.1415927410125732) + this._timeOffset) * 10.0 + 8.0));
    if (this._animationFrames.Count <= 0)
      return;
    int index = (int) ((double) this._timeAccumulator * 15.0) % this._animationFrames.Count;
    int? animationFrame = this._animationFrame;
    int num = index;
    if (!(animationFrame.GetValueOrDefault() == num & animationFrame.HasValue))
    {
      this._animationFrame = new int?(index);
      this._intentSprite.Texture = this._animationFrames[index];
    }
    this._timeAccumulator += (float) delta;
  }

  public static NIntent Create(float startTime)
  {
    NIntent nintent = PreloadManager.Cache.GetScene("res://scenes/combat/intent.tscn").Instantiate<NIntent>((PackedScene.GenEditState) 0L);
    nintent._timeOffset = startTime;
    return nintent;
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      List<string> items = new List<string>();
      items.Add("res://scenes/combat/intent.tscn");
      items.AddRange(IntentAnimData.AssetPaths);
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyList<string>(items);
    }
  }

  public void PlayPerform() => this._intentParticle.Emitting = true;

  public void SetFrozen(bool isFrozen) => this._isFrozen = isFrozen;

  private void OnHovered()
  {
    if (!this._intent.HasIntentTip)
      return;
    // ISSUE: object of a compiler-generated type is created
    NCombatRoom.Instance?.GetCreatureNode(this._owner)?.ShowHoverTips((IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>((IHoverTip) this._intent.GetHoverTip(this._targets, this._owner)));
  }

  private void OnUnhovered() => NCombatRoom.Instance?.GetCreatureNode(this._owner)?.HideHoverTips();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NIntent.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NIntent.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NIntent.MethodName.DebugToggleVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NIntent.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NIntent.MethodName.UpdateVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NIntent.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NIntent.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("startTime"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NIntent.MethodName.PlayPerform, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NIntent.MethodName.SetFrozen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isFrozen"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NIntent.MethodName.OnHovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NIntent.MethodName.OnUnhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NIntent.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NIntent.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NIntent.MethodName.DebugToggleVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DebugToggleVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NIntent.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NIntent.MethodName.UpdateVisuals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateVisuals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NIntent.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NIntent.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NIntent nintent = NIntent.Create(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NIntent>(ref nintent);
      return true;
    }
    if (StringName.op_Equality(ref method, NIntent.MethodName.PlayPerform) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlayPerform();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NIntent.MethodName.SetFrozen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetFrozen(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NIntent.MethodName.OnHovered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnHovered();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NIntent.MethodName.OnUnhovered) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnUnhovered();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NIntent.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NIntent nintent = NIntent.Create(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NIntent>(ref nintent);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NIntent.MethodName._Ready) || StringName.op_Equality(ref method, NIntent.MethodName._EnterTree) || StringName.op_Equality(ref method, NIntent.MethodName.DebugToggleVisibility) || StringName.op_Equality(ref method, NIntent.MethodName._ExitTree) || StringName.op_Equality(ref method, NIntent.MethodName.UpdateVisuals) || StringName.op_Equality(ref method, NIntent.MethodName._Process) || StringName.op_Equality(ref method, NIntent.MethodName.Create) || StringName.op_Equality(ref method, NIntent.MethodName.PlayPerform) || StringName.op_Equality(ref method, NIntent.MethodName.SetFrozen) || StringName.op_Equality(ref method, NIntent.MethodName.OnHovered) || StringName.op_Equality(ref method, NIntent.MethodName.OnUnhovered) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NIntent.PropertyName._intentHolder))
    {
      this._intentHolder = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NIntent.PropertyName._intentSprite))
    {
      this._intentSprite = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NIntent.PropertyName._valueLabel))
    {
      this._valueLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NIntent.PropertyName._intentParticle))
    {
      this._intentParticle = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NIntent.PropertyName._timeOffset))
    {
      this._timeOffset = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NIntent.PropertyName._timeAccumulator))
    {
      this._timeAccumulator = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NIntent.PropertyName._isFrozen))
    {
      this._isFrozen = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NIntent.PropertyName._animationName))
    {
      this._animationName = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NIntent.PropertyName._combatRoom))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._combatRoom = VariantUtils.ConvertTo<NCombatRoom>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NIntent.PropertyName._intentHolder))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._intentHolder);
      return true;
    }
    if (StringName.op_Equality(ref name, NIntent.PropertyName._intentSprite))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._intentSprite);
      return true;
    }
    if (StringName.op_Equality(ref name, NIntent.PropertyName._valueLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._valueLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NIntent.PropertyName._intentParticle))
    {
      value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._intentParticle);
      return true;
    }
    if (StringName.op_Equality(ref name, NIntent.PropertyName._timeOffset))
    {
      value = VariantUtils.CreateFrom<float>(ref this._timeOffset);
      return true;
    }
    if (StringName.op_Equality(ref name, NIntent.PropertyName._timeAccumulator))
    {
      value = VariantUtils.CreateFrom<float>(ref this._timeAccumulator);
      return true;
    }
    if (StringName.op_Equality(ref name, NIntent.PropertyName._isFrozen))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isFrozen);
      return true;
    }
    if (StringName.op_Equality(ref name, NIntent.PropertyName._animationName))
    {
      value = VariantUtils.CreateFrom<string>(ref this._animationName);
      return true;
    }
    if (!StringName.op_Equality(ref name, NIntent.PropertyName._combatRoom))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NCombatRoom>(ref this._combatRoom);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NIntent.PropertyName._intentHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NIntent.PropertyName._intentSprite, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NIntent.PropertyName._valueLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NIntent.PropertyName._intentParticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NIntent.PropertyName._timeOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NIntent.PropertyName._timeAccumulator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NIntent.PropertyName._isFrozen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NIntent.PropertyName._animationName, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NIntent.PropertyName._combatRoom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NIntent.PropertyName._intentHolder, Variant.From<Control>(ref this._intentHolder));
    info.AddProperty(NIntent.PropertyName._intentSprite, Variant.From<Sprite2D>(ref this._intentSprite));
    info.AddProperty(NIntent.PropertyName._valueLabel, Variant.From<MegaRichTextLabel>(ref this._valueLabel));
    info.AddProperty(NIntent.PropertyName._intentParticle, Variant.From<CpuParticles2D>(ref this._intentParticle));
    info.AddProperty(NIntent.PropertyName._timeOffset, Variant.From<float>(ref this._timeOffset));
    info.AddProperty(NIntent.PropertyName._timeAccumulator, Variant.From<float>(ref this._timeAccumulator));
    info.AddProperty(NIntent.PropertyName._isFrozen, Variant.From<bool>(ref this._isFrozen));
    info.AddProperty(NIntent.PropertyName._animationName, Variant.From<string>(ref this._animationName));
    info.AddProperty(NIntent.PropertyName._combatRoom, Variant.From<NCombatRoom>(ref this._combatRoom));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NIntent.PropertyName._intentHolder, ref variant1))
      this._intentHolder = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NIntent.PropertyName._intentSprite, ref variant2))
      this._intentSprite = ((Variant) ref variant2).As<Sprite2D>();
    Variant variant3;
    if (info.TryGetProperty(NIntent.PropertyName._valueLabel, ref variant3))
      this._valueLabel = ((Variant) ref variant3).As<MegaRichTextLabel>();
    Variant variant4;
    if (info.TryGetProperty(NIntent.PropertyName._intentParticle, ref variant4))
      this._intentParticle = ((Variant) ref variant4).As<CpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NIntent.PropertyName._timeOffset, ref variant5))
      this._timeOffset = ((Variant) ref variant5).As<float>();
    Variant variant6;
    if (info.TryGetProperty(NIntent.PropertyName._timeAccumulator, ref variant6))
      this._timeAccumulator = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (info.TryGetProperty(NIntent.PropertyName._isFrozen, ref variant7))
      this._isFrozen = ((Variant) ref variant7).As<bool>();
    Variant variant8;
    if (info.TryGetProperty(NIntent.PropertyName._animationName, ref variant8))
      this._animationName = ((Variant) ref variant8).As<string>();
    Variant variant9;
    if (!info.TryGetProperty(NIntent.PropertyName._combatRoom, ref variant9))
      return;
    this._combatRoom = ((Variant) ref variant9).As<NCombatRoom>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName DebugToggleVisibility = StringName.op_Implicit(nameof (DebugToggleVisibility));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName UpdateVisuals = StringName.op_Implicit(nameof (UpdateVisuals));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName PlayPerform = StringName.op_Implicit(nameof (PlayPerform));
    public static readonly StringName SetFrozen = StringName.op_Implicit(nameof (SetFrozen));
    public static readonly StringName OnHovered = StringName.op_Implicit(nameof (OnHovered));
    public static readonly StringName OnUnhovered = StringName.op_Implicit(nameof (OnUnhovered));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _intentHolder = StringName.op_Implicit(nameof (_intentHolder));
    public static readonly StringName _intentSprite = StringName.op_Implicit(nameof (_intentSprite));
    public static readonly StringName _valueLabel = StringName.op_Implicit(nameof (_valueLabel));
    public static readonly StringName _intentParticle = StringName.op_Implicit(nameof (_intentParticle));
    public static readonly StringName _timeOffset = StringName.op_Implicit(nameof (_timeOffset));
    public static readonly StringName _timeAccumulator = StringName.op_Implicit(nameof (_timeAccumulator));
    public static readonly StringName _isFrozen = StringName.op_Implicit(nameof (_isFrozen));
    public static readonly StringName _animationName = StringName.op_Implicit(nameof (_animationName));
    public static readonly StringName _combatRoom = StringName.op_Implicit(nameof (_combatRoom));
  }

  public class SignalName : Control.SignalName
  {
  }
}
