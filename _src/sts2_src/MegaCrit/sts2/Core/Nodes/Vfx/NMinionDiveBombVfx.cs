// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NMinionDiveBombVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NMinionDiveBombVfx.cs")]
public class NMinionDiveBombVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_minion_dive_bomb");
  [Export]
  private Sprite2D? _minionSprite;
  [Export]
  private Array<Texture2D>? _minionTextures;
  [Export]
  private AnimationPlayer? _minionAnimator;
  [Export]
  private Array<string>? _minionAnimations;
  [Export]
  private Array<NParticlesContainer>? _minionVfx;
  [Export]
  private Node2D? _fallingTrail;
  [Export]
  private NParticlesContainer? _fallingVfx;
  [Export]
  private NParticlesContainer? _impactVfx;
  [Export]
  private float _flightTime;
  [Export]
  private float _fallingVfxEntryTime;
  [Export]
  private Curve? _horizontalCurve;
  [Export]
  private Curve? _verticalCurve;
  [Export]
  private Curve? _textureCurve;
  [Export]
  private float _maxHeight;
  [Export]
  private Vector2 _sourceOffset;
  [Export]
  private Vector2 _destinationOffset;
  private int _previousIndex = -1;
  private Vector2 _sourcePosition;
  private CancellationTokenSource? _cts;
  private Vector2 _destinationPosition;

  public override void _ExitTree() => this._cts?.Cancel();

  private Vector2 SourceFinalPosition
  {
    get => Vector2.op_Addition(this._sourcePosition, this._sourceOffset);
  }

  private Vector2 DestinationFinalPosition
  {
    get => Vector2.op_Addition(this._destinationPosition, this._destinationOffset);
  }

  public static NMinionDiveBombVfx? Create(Creature owner, Creature target)
  {
    if (TestMode.IsOn)
      return (NMinionDiveBombVfx) null;
    NCreature creatureNode1 = NCombatRoom.Instance?.GetCreatureNode(owner);
    NCreature creatureNode2 = NCombatRoom.Instance?.GetCreatureNode(target);
    return creatureNode2 != null && creatureNode1 != null ? NMinionDiveBombVfx.Create(creatureNode1.VfxSpawnPosition, creatureNode2.GetBottomOfHitbox()) : (NMinionDiveBombVfx) null;
  }

  public static NMinionDiveBombVfx? Create(
    Vector2 playerCenterPosition,
    Vector2 targetFloorPosition)
  {
    if (TestMode.IsOn)
      return (NMinionDiveBombVfx) null;
    NMinionDiveBombVfx nminionDiveBombVfx = PreloadManager.Cache.GetScene(NMinionDiveBombVfx.scenePath).Instantiate<NMinionDiveBombVfx>((PackedScene.GenEditState) 0L);
    nminionDiveBombVfx.Initialize(playerCenterPosition, targetFloorPosition);
    return nminionDiveBombVfx;
  }

  private void Initialize(Vector2 sourcePosition, Vector2 destinationPosition)
  {
    this._sourcePosition = sourcePosition;
    this._destinationPosition = destinationPosition;
    ((CanvasItem) this._fallingTrail).Visible = true;
    this.GlobalPosition = sourcePosition;
    for (int index = 0; index < this._minionVfx.Count; ++index)
      this._minionVfx[index].SetEmitting(false);
  }

  public override void _Ready() => TaskHelper.RunSafely(this.PlaySequence());

  private void SetMinionVisible(bool visible)
  {
    ((CanvasItem) this._minionSprite).SelfModulate = visible ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0.0f);
  }

  private void UpdateMinionSprite(int index)
  {
    if (this._previousIndex == index)
      return;
    this._previousIndex = index;
    this._minionSprite.Texture = this._minionTextures[Mathf.Clamp(index, 0, this._minionTextures.Count - 1)];
    string minionAnimation = this._minionAnimations[Mathf.Clamp(index, 0, this._minionAnimations.Count - 1)];
    if (!this._minionAnimator.CurrentAnimation.Equals(minionAnimation))
      this._minionAnimator.Play(StringName.op_Implicit(minionAnimation), -1.0, 1f, false);
    for (int index1 = 0; index1 < this._minionVfx.Count; ++index1)
    {
      if (index1 == index)
        this._minionVfx[index1].Restart();
    }
  }

  private async Task PlaySequence()
  {
    this._cts = new CancellationTokenSource();
    Vector2 startPos = this.SourceFinalPosition;
    Vector2 endPos = this.DestinationFinalPosition;
    this.UpdateMinionSprite(0);
    ((Node2D) this._minionSprite).GlobalPosition = startPos;
    this.SetMinionVisible(true);
    double timer = 0.0;
    bool isPlayingFallingVfx = false;
    while (timer < (double) this._flightTime)
    {
      float num1 = (float) timer / this._flightTime;
      float num2 = this._horizontalCurve.Sample(num1);
      float num3 = this._verticalCurve.Sample(num1);
      this.UpdateMinionSprite(Mathf.FloorToInt(this._textureCurve.Sample(num1)));
      Vector2 vector2 = Vector2.op_Addition(((Vector2) ref startPos).Lerp(endPos, num2), Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Up, num3), this._maxHeight));
      ((Node2D) this._minionSprite).GlobalPosition = vector2;
      this._fallingVfx.GlobalPosition = vector2;
      if (timer >= (double) this._fallingVfxEntryTime && !isPlayingFallingVfx)
      {
        this._fallingVfx.Restart();
        ((CanvasItem) this._fallingTrail).Visible = true;
        isPlayingFallingVfx = true;
      }
      timer += ((Node) this).GetProcessDeltaTime();
      double num4 = (double) await ((Node) this).AwaitProcessFrame();
    }
    this.SetMinionVisible(false);
    NGame.Instance?.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short);
    this._impactVfx.GlobalPosition = this._destinationPosition;
    this._impactVfx.Restart();
    ((CanvasItem) this._fallingTrail).Visible = false;
    this._fallingVfx.SetEmitting(false);
    await Cmd.Wait(2f, this._cts.Token);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NMinionDiveBombVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMinionDiveBombVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("playerCenterPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetFloorPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMinionDiveBombVfx.MethodName.Initialize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("sourcePosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("destinationPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMinionDiveBombVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMinionDiveBombVfx.MethodName.SetMinionVisible, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("visible"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMinionDiveBombVfx.MethodName.UpdateMinionSprite, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMinionDiveBombVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMinionDiveBombVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NMinionDiveBombVfx nminionDiveBombVfx = NMinionDiveBombVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NMinionDiveBombVfx>(ref nminionDiveBombVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NMinionDiveBombVfx.MethodName.Initialize) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.Initialize(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMinionDiveBombVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMinionDiveBombVfx.MethodName.SetMinionVisible) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetMinionVisible(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMinionDiveBombVfx.MethodName.UpdateMinionSprite) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdateMinionSprite(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMinionDiveBombVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NMinionDiveBombVfx nminionDiveBombVfx = NMinionDiveBombVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NMinionDiveBombVfx>(ref nminionDiveBombVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMinionDiveBombVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NMinionDiveBombVfx.MethodName.Create) || StringName.op_Equality(ref method, NMinionDiveBombVfx.MethodName.Initialize) || StringName.op_Equality(ref method, NMinionDiveBombVfx.MethodName._Ready) || StringName.op_Equality(ref method, NMinionDiveBombVfx.MethodName.SetMinionVisible) || StringName.op_Equality(ref method, NMinionDiveBombVfx.MethodName.UpdateMinionSprite) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._minionSprite))
    {
      this._minionSprite = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._minionTextures))
    {
      this._minionTextures = VariantUtils.ConvertToArray<Texture2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._minionAnimator))
    {
      this._minionAnimator = VariantUtils.ConvertTo<AnimationPlayer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._minionAnimations))
    {
      this._minionAnimations = VariantUtils.ConvertToArray<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._minionVfx))
    {
      this._minionVfx = VariantUtils.ConvertToArray<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._fallingTrail))
    {
      this._fallingTrail = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._fallingVfx))
    {
      this._fallingVfx = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._impactVfx))
    {
      this._impactVfx = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._flightTime))
    {
      this._flightTime = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._fallingVfxEntryTime))
    {
      this._fallingVfxEntryTime = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._horizontalCurve))
    {
      this._horizontalCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._verticalCurve))
    {
      this._verticalCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._textureCurve))
    {
      this._textureCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._maxHeight))
    {
      this._maxHeight = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._sourceOffset))
    {
      this._sourceOffset = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._destinationOffset))
    {
      this._destinationOffset = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._previousIndex))
    {
      this._previousIndex = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._sourcePosition))
    {
      this._sourcePosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._destinationPosition))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._destinationPosition = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName.SourceFinalPosition))
    {
      ref godot_variant local = ref value;
      Vector2 sourceFinalPosition = this.SourceFinalPosition;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref sourceFinalPosition);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName.DestinationFinalPosition))
    {
      ref godot_variant local = ref value;
      Vector2 destinationFinalPosition = this.DestinationFinalPosition;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref destinationFinalPosition);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._minionSprite))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._minionSprite);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._minionTextures))
    {
      value = VariantUtils.CreateFromArray<Texture2D>(this._minionTextures);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._minionAnimator))
    {
      value = VariantUtils.CreateFrom<AnimationPlayer>(ref this._minionAnimator);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._minionAnimations))
    {
      value = VariantUtils.CreateFromArray<string>(this._minionAnimations);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._minionVfx))
    {
      value = VariantUtils.CreateFromArray<NParticlesContainer>(this._minionVfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._fallingTrail))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._fallingTrail);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._fallingVfx))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._fallingVfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._impactVfx))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._impactVfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._flightTime))
    {
      value = VariantUtils.CreateFrom<float>(ref this._flightTime);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._fallingVfxEntryTime))
    {
      value = VariantUtils.CreateFrom<float>(ref this._fallingVfxEntryTime);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._horizontalCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._horizontalCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._verticalCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._verticalCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._textureCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._textureCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._maxHeight))
    {
      value = VariantUtils.CreateFrom<float>(ref this._maxHeight);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._sourceOffset))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._sourceOffset);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._destinationOffset))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._destinationOffset);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._previousIndex))
    {
      value = VariantUtils.CreateFrom<int>(ref this._previousIndex);
      return true;
    }
    if (StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._sourcePosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._sourcePosition);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMinionDiveBombVfx.PropertyName._destinationPosition))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._destinationPosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMinionDiveBombVfx.PropertyName._minionSprite, (PropertyHint) 34L, "Sprite2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NMinionDiveBombVfx.PropertyName._minionTextures, (PropertyHint) 23L, "24/17:Texture2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NMinionDiveBombVfx.PropertyName._minionAnimator, (PropertyHint) 34L, "AnimationPlayer", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NMinionDiveBombVfx.PropertyName._minionAnimations, (PropertyHint) 23L, "4/0:", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NMinionDiveBombVfx.PropertyName._minionVfx, (PropertyHint) 23L, "24/34:Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NMinionDiveBombVfx.PropertyName._fallingTrail, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NMinionDiveBombVfx.PropertyName._fallingVfx, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NMinionDiveBombVfx.PropertyName._impactVfx, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NMinionDiveBombVfx.PropertyName._flightTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NMinionDiveBombVfx.PropertyName._fallingVfxEntryTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NMinionDiveBombVfx.PropertyName._horizontalCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NMinionDiveBombVfx.PropertyName._verticalCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NMinionDiveBombVfx.PropertyName._textureCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NMinionDiveBombVfx.PropertyName._maxHeight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 5L, NMinionDiveBombVfx.PropertyName._sourceOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 5L, NMinionDiveBombVfx.PropertyName._destinationOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 2L, NMinionDiveBombVfx.PropertyName._previousIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMinionDiveBombVfx.PropertyName._sourcePosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMinionDiveBombVfx.PropertyName._destinationPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMinionDiveBombVfx.PropertyName.SourceFinalPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMinionDiveBombVfx.PropertyName.DestinationFinalPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMinionDiveBombVfx.PropertyName._minionSprite, Variant.From<Sprite2D>(ref this._minionSprite));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._minionTextures, Variant.CreateFrom<Texture2D>(this._minionTextures));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._minionAnimator, Variant.From<AnimationPlayer>(ref this._minionAnimator));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._minionAnimations, Variant.CreateFrom<string>(this._minionAnimations));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._minionVfx, Variant.CreateFrom<NParticlesContainer>(this._minionVfx));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._fallingTrail, Variant.From<Node2D>(ref this._fallingTrail));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._fallingVfx, Variant.From<NParticlesContainer>(ref this._fallingVfx));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._impactVfx, Variant.From<NParticlesContainer>(ref this._impactVfx));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._flightTime, Variant.From<float>(ref this._flightTime));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._fallingVfxEntryTime, Variant.From<float>(ref this._fallingVfxEntryTime));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._horizontalCurve, Variant.From<Curve>(ref this._horizontalCurve));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._verticalCurve, Variant.From<Curve>(ref this._verticalCurve));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._textureCurve, Variant.From<Curve>(ref this._textureCurve));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._maxHeight, Variant.From<float>(ref this._maxHeight));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._sourceOffset, Variant.From<Vector2>(ref this._sourceOffset));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._destinationOffset, Variant.From<Vector2>(ref this._destinationOffset));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._previousIndex, Variant.From<int>(ref this._previousIndex));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._sourcePosition, Variant.From<Vector2>(ref this._sourcePosition));
    info.AddProperty(NMinionDiveBombVfx.PropertyName._destinationPosition, Variant.From<Vector2>(ref this._destinationPosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._minionSprite, ref variant1))
      this._minionSprite = ((Variant) ref variant1).As<Sprite2D>();
    Variant variant2;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._minionTextures, ref variant2))
      this._minionTextures = ((Variant) ref variant2).AsGodotArray<Texture2D>();
    Variant variant3;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._minionAnimator, ref variant3))
      this._minionAnimator = ((Variant) ref variant3).As<AnimationPlayer>();
    Variant variant4;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._minionAnimations, ref variant4))
      this._minionAnimations = ((Variant) ref variant4).AsGodotArray<string>();
    Variant variant5;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._minionVfx, ref variant5))
      this._minionVfx = ((Variant) ref variant5).AsGodotArray<NParticlesContainer>();
    Variant variant6;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._fallingTrail, ref variant6))
      this._fallingTrail = ((Variant) ref variant6).As<Node2D>();
    Variant variant7;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._fallingVfx, ref variant7))
      this._fallingVfx = ((Variant) ref variant7).As<NParticlesContainer>();
    Variant variant8;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._impactVfx, ref variant8))
      this._impactVfx = ((Variant) ref variant8).As<NParticlesContainer>();
    Variant variant9;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._flightTime, ref variant9))
      this._flightTime = ((Variant) ref variant9).As<float>();
    Variant variant10;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._fallingVfxEntryTime, ref variant10))
      this._fallingVfxEntryTime = ((Variant) ref variant10).As<float>();
    Variant variant11;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._horizontalCurve, ref variant11))
      this._horizontalCurve = ((Variant) ref variant11).As<Curve>();
    Variant variant12;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._verticalCurve, ref variant12))
      this._verticalCurve = ((Variant) ref variant12).As<Curve>();
    Variant variant13;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._textureCurve, ref variant13))
      this._textureCurve = ((Variant) ref variant13).As<Curve>();
    Variant variant14;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._maxHeight, ref variant14))
      this._maxHeight = ((Variant) ref variant14).As<float>();
    Variant variant15;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._sourceOffset, ref variant15))
      this._sourceOffset = ((Variant) ref variant15).As<Vector2>();
    Variant variant16;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._destinationOffset, ref variant16))
      this._destinationOffset = ((Variant) ref variant16).As<Vector2>();
    Variant variant17;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._previousIndex, ref variant17))
      this._previousIndex = ((Variant) ref variant17).As<int>();
    Variant variant18;
    if (info.TryGetProperty(NMinionDiveBombVfx.PropertyName._sourcePosition, ref variant18))
      this._sourcePosition = ((Variant) ref variant18).As<Vector2>();
    Variant variant19;
    if (!info.TryGetProperty(NMinionDiveBombVfx.PropertyName._destinationPosition, ref variant19))
      return;
    this._destinationPosition = ((Variant) ref variant19).As<Vector2>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName Initialize = StringName.op_Implicit(nameof (Initialize));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetMinionVisible = StringName.op_Implicit(nameof (SetMinionVisible));
    public static readonly StringName UpdateMinionSprite = StringName.op_Implicit(nameof (UpdateMinionSprite));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName SourceFinalPosition = StringName.op_Implicit(nameof (SourceFinalPosition));
    public static readonly StringName DestinationFinalPosition = StringName.op_Implicit(nameof (DestinationFinalPosition));
    public static readonly StringName _minionSprite = StringName.op_Implicit(nameof (_minionSprite));
    public static readonly StringName _minionTextures = StringName.op_Implicit(nameof (_minionTextures));
    public static readonly StringName _minionAnimator = StringName.op_Implicit(nameof (_minionAnimator));
    public static readonly StringName _minionAnimations = StringName.op_Implicit(nameof (_minionAnimations));
    public static readonly StringName _minionVfx = StringName.op_Implicit(nameof (_minionVfx));
    public static readonly StringName _fallingTrail = StringName.op_Implicit(nameof (_fallingTrail));
    public static readonly StringName _fallingVfx = StringName.op_Implicit(nameof (_fallingVfx));
    public static readonly StringName _impactVfx = StringName.op_Implicit(nameof (_impactVfx));
    public static readonly StringName _flightTime = StringName.op_Implicit(nameof (_flightTime));
    public static readonly StringName _fallingVfxEntryTime = StringName.op_Implicit(nameof (_fallingVfxEntryTime));
    public static readonly StringName _horizontalCurve = StringName.op_Implicit(nameof (_horizontalCurve));
    public static readonly StringName _verticalCurve = StringName.op_Implicit(nameof (_verticalCurve));
    public static readonly StringName _textureCurve = StringName.op_Implicit(nameof (_textureCurve));
    public static readonly StringName _maxHeight = StringName.op_Implicit(nameof (_maxHeight));
    public static readonly StringName _sourceOffset = StringName.op_Implicit(nameof (_sourceOffset));
    public static readonly StringName _destinationOffset = StringName.op_Implicit(nameof (_destinationOffset));
    public static readonly StringName _previousIndex = StringName.op_Implicit(nameof (_previousIndex));
    public static readonly StringName _sourcePosition = StringName.op_Implicit(nameof (_sourcePosition));
    public static readonly StringName _destinationPosition = StringName.op_Implicit(nameof (_destinationPosition));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
