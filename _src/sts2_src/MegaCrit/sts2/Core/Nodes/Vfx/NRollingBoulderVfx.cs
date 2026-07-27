// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NRollingBoulderVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NRollingBoulderVfx.cs")]
public class NRollingBoulderVfx : Node2D
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/vfx_rolling_boulder");
  private const float _maxScale = 1.5f;
  private const Decimal _halfScaleDamage = 40M;
  private const float _maxTimeToImpact = 0.5f;
  private const float _minTimeToImpact = 0.25f;
  private const float _minRotationSpeed = 600f;
  private const float _maxRotationSpeed = 1000f;
  private const float _minXOffset = 600f;
  private const float _maxXOffset = 1000f;
  private Sprite2D _boulder;
  private Sprite2D _shadow;
  private GpuParticles2D _slamBehind;
  private GpuParticles2D _slamFront;
  private List<Creature> _creatures;
  private Vector2? _debugFinalPosition;
  private Decimal _damage;
  private 
  #nullable disable
  NRollingBoulderVfx.HitCreatureEventHandler backing_HitCreature;
  private NRollingBoulderVfx.FinishedEventHandler backing_Finished;

  public static 
  #nullable enable
  string[] AssetPaths
  {
    get => new string[1]{ NRollingBoulderVfx._scenePath };
  }

  public static NRollingBoulderVfx? Create(
    IEnumerable<Creature> creatures,
    Decimal damage,
    Vector2? debugFinalPosition = null)
  {
    if (TestMode.IsOn)
      return (NRollingBoulderVfx) null;
    NRollingBoulderVfx nrollingBoulderVfx = PreloadManager.Cache.GetScene(NRollingBoulderVfx._scenePath).Instantiate<NRollingBoulderVfx>((PackedScene.GenEditState) 0L);
    nrollingBoulderVfx._creatures = creatures.ToList<Creature>();
    nrollingBoulderVfx._damage = damage;
    nrollingBoulderVfx._debugFinalPosition = debugFinalPosition;
    return nrollingBoulderVfx;
  }

  public override void _Ready()
  {
    this._boulder = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("Boulder"));
    this._shadow = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("Shadow"));
    this._slamBehind = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("SlamBehind"));
    this._slamFront = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("SlamFront"));
    float num1 = (float) (this._damage / (this._damage + 40M));
    float num2 = num1 * 1.5f;
    float timeToImpact = Mathf.Lerp(0.5f, 0.25f, num1);
    float rotationSpeed = Mathf.Lerp(600f, 1000f, num1);
    float xOffset = Mathf.Lerp(600f, 1000f, num1);
    this.Scale = Vector2.op_Multiply(Vector2.One, num2);
    foreach (GpuParticles2D gpuParticles2D in ((IEnumerable) ((Node) this).GetChildren(false)).OfType<GpuParticles2D>())
    {
      if (!gpuParticles2D.LocalCoords && gpuParticles2D.ProcessMaterial is ParticleProcessMaterial processMaterial)
        processMaterial.Scale = Vector2.op_Multiply(Vector2.One, num2);
    }
    TaskHelper.RunSafely(this.PlayAnim(timeToImpact, xOffset, rotationSpeed));
  }

  private async Task PlayAnim(float timeToImpact, float xOffset, float rotationSpeed)
  {
    List<Creature> creaturesHit;
    if (NCombatRoom.Instance == null || this._creatures.Count == 0)
    {
      this.CleanUpBeforeEarlyExit();
      creaturesHit = (List<Creature>) null;
    }
    else
    {
      Vector2 initialBoulderPosition = this.GlobalPosition;
      Vector2 initialShadowOffset = ((Node2D) this._shadow).Position;
      Vector2 initialShadowScale = ((Node2D) this._shadow).Scale;
      Vector2 vector2_1 = Vector2.Zero;
      creaturesHit = new List<Creature>();
      int num1 = 0;
      foreach (Creature creature in this._creatures)
      {
        NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(creature);
        if (creatureNode != null)
        {
          vector2_1 = Vector2.op_Addition(vector2_1, creatureNode.Visuals.GlobalPosition);
          ++num1;
        }
      }
      if (num1 == 0)
      {
        this.CleanUpBeforeEarlyExit();
        creaturesHit = (List<Creature>) null;
      }
      else
      {
        Vector2 vector2_2 = Vector2.op_Division(vector2_1, (float) num1);
        ((Node) this._shadow).Reparent((Node) NCombatRoom.Instance.BackCombatVfxContainer, true);
        if (Vector2.op_Equality(vector2_2, Vector2.Zero))
        {
          if (this._debugFinalPosition.HasValue)
          {
            vector2_2 = this._debugFinalPosition.Value;
          }
          else
          {
            this.CleanUpBeforeEarlyExit();
            creaturesHit = (List<Creature>) null;
            return;
          }
        }
        Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
        Vector2 impactPoint = new Vector2(((Rect2) ref viewportRect).Size.X * 0.5f, vector2_2.Y);
        Vector2 vector2_3;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2_3).\u002Ector(impactPoint.X - xOffset, -150f);
        float num2 = (impactPoint.X - vector2_3.X) / timeToImpact;
        float yAccel = (float) (2.0 * ((double) impactPoint.Y - (double) vector2_3.Y) / ((double) timeToImpact * (double) timeToImpact));
        ((Node2D) this._shadow).Scale = Vector2.Zero;
        this.GlobalPosition = vector2_3;
        Vector2 velocity = new Vector2(num2, 0.0f);
        float timer = 0.0f;
        bool firstImpact = false;
        while ((double) timer <= 1.0)
        {
          float num3 = await ((Node) this).AwaitProcessFrame();
          velocity.Y += yAccel * num3;
          this.GlobalPosition = Vector2.op_Addition(this.GlobalPosition, Vector2.op_Multiply(velocity, num3));
          Sprite2D boulder = this._boulder;
          ((Node2D) boulder).Rotation = ((Node2D) boulder).Rotation + Mathf.DegToRad(rotationSpeed) * num3;
          ((Node2D) this._shadow).Scale = Vector2.op_Multiply(Vector2.op_Multiply(initialShadowScale, this.Scale), Mathf.InverseLerp(initialBoulderPosition.Y, impactPoint.Y, this.GlobalPosition.Y));
          ((Node2D) this._shadow).GlobalPosition = Vector2.op_Addition(new Vector2(this.GlobalPosition.X, impactPoint.Y), Vector2.op_Multiply(initialShadowOffset, this.Scale));
          foreach (Creature creature in this._creatures)
          {
            if (!creaturesHit.Contains(creature))
            {
              NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(creature);
              if (creatureNode == null)
                creaturesHit.Add(creature);
              else if ((double) this.GlobalPosition.X >= (double) (creatureNode.Visuals.GlobalPosition.X - creatureNode.Visuals.Bounds.Size.X * 0.5f))
              {
                this.EmitSignalHitCreature(creatureNode);
                creaturesHit.Add(creature);
              }
            }
          }
          if (this._creatures.Count == creaturesHit.Count)
            timer += num3;
          if ((double) this.GlobalPosition.Y >= (double) impactPoint.Y)
          {
            if (!firstImpact)
            {
              ((Node2D) this._slamBehind).GlobalPosition = impactPoint;
              ((Node2D) this._slamFront).GlobalPosition = impactPoint;
              this._slamBehind.Emitting = true;
              this._slamFront.Emitting = true;
              firstImpact = true;
            }
            velocity.Y = (float) (-(double) velocity.Y * 0.33000001311302185);
            if ((double) Mathf.Abs(velocity.Y) < 1.0)
            {
              velocity.Y = 0.0f;
              yAccel = 0.0f;
            }
            Vector2 globalPosition = this.GlobalPosition;
            globalPosition.Y = impactPoint.Y;
            this.GlobalPosition = globalPosition;
          }
        }
        ((GodotObject) this).EmitSignal(NRollingBoulderVfx.SignalName.Finished, Array.Empty<Variant>());
        ((Node) this._shadow).QueueFreeSafely();
        ((Node) this).QueueFreeSafely();
        creaturesHit = (List<Creature>) null;
      }
    }
  }

  private void CleanUpBeforeEarlyExit()
  {
    Log.Warn("Rolling boulder VFX spawned with no targets, disabling");
    ((GodotObject) this).EmitSignal(NRollingBoulderVfx.SignalName.Finished, Array.Empty<Variant>());
    ((Node) this._shadow).QueueFreeSafely();
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NRollingBoulderVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRollingBoulderVfx.MethodName.CleanUpBeforeEarlyExit, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRollingBoulderVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRollingBoulderVfx.MethodName.CleanUpBeforeEarlyExit) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.CleanUpBeforeEarlyExit();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRollingBoulderVfx.MethodName._Ready) || StringName.op_Equality(ref method, NRollingBoulderVfx.MethodName.CleanUpBeforeEarlyExit) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRollingBoulderVfx.PropertyName._boulder))
    {
      this._boulder = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRollingBoulderVfx.PropertyName._shadow))
    {
      this._shadow = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRollingBoulderVfx.PropertyName._slamBehind))
    {
      this._slamBehind = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRollingBoulderVfx.PropertyName._slamFront))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._slamFront = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRollingBoulderVfx.PropertyName._boulder))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._boulder);
      return true;
    }
    if (StringName.op_Equality(ref name, NRollingBoulderVfx.PropertyName._shadow))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._shadow);
      return true;
    }
    if (StringName.op_Equality(ref name, NRollingBoulderVfx.PropertyName._slamBehind))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._slamBehind);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRollingBoulderVfx.PropertyName._slamFront))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._slamFront);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRollingBoulderVfx.PropertyName._boulder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRollingBoulderVfx.PropertyName._shadow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRollingBoulderVfx.PropertyName._slamBehind, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRollingBoulderVfx.PropertyName._slamFront, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRollingBoulderVfx.PropertyName._boulder, Variant.From<Sprite2D>(ref this._boulder));
    info.AddProperty(NRollingBoulderVfx.PropertyName._shadow, Variant.From<Sprite2D>(ref this._shadow));
    info.AddProperty(NRollingBoulderVfx.PropertyName._slamBehind, Variant.From<GpuParticles2D>(ref this._slamBehind));
    info.AddProperty(NRollingBoulderVfx.PropertyName._slamFront, Variant.From<GpuParticles2D>(ref this._slamFront));
    info.AddSignalEventDelegate(NRollingBoulderVfx.SignalName.HitCreature, (Delegate) this.backing_HitCreature);
    info.AddSignalEventDelegate(NRollingBoulderVfx.SignalName.Finished, (Delegate) this.backing_Finished);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRollingBoulderVfx.PropertyName._boulder, ref variant1))
      this._boulder = ((Variant) ref variant1).As<Sprite2D>();
    Variant variant2;
    if (info.TryGetProperty(NRollingBoulderVfx.PropertyName._shadow, ref variant2))
      this._shadow = ((Variant) ref variant2).As<Sprite2D>();
    Variant variant3;
    if (info.TryGetProperty(NRollingBoulderVfx.PropertyName._slamBehind, ref variant3))
      this._slamBehind = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NRollingBoulderVfx.PropertyName._slamFront, ref variant4))
      this._slamFront = ((Variant) ref variant4).As<GpuParticles2D>();
    NRollingBoulderVfx.HitCreatureEventHandler creatureEventHandler;
    if (info.TryGetSignalEventDelegate<NRollingBoulderVfx.HitCreatureEventHandler>(NRollingBoulderVfx.SignalName.HitCreature, ref creatureEventHandler))
      this.backing_HitCreature = creatureEventHandler;
    NRollingBoulderVfx.FinishedEventHandler finishedEventHandler;
    if (!info.TryGetSignalEventDelegate<NRollingBoulderVfx.FinishedEventHandler>(NRollingBoulderVfx.SignalName.Finished, ref finishedEventHandler))
      return;
    this.backing_Finished = finishedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NRollingBoulderVfx.SignalName.HitCreature, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("creature"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRollingBoulderVfx.SignalName.Finished, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NRollingBoulderVfx.HitCreatureEventHandler HitCreature
  {
    add => this.backing_HitCreature += value;
    remove => this.backing_HitCreature -= value;
  }

  protected void EmitSignalHitCreature(NCreature creature)
  {
    ((GodotObject) this).EmitSignal(NRollingBoulderVfx.SignalName.HitCreature, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) creature)
    });
  }

  public event NRollingBoulderVfx.FinishedEventHandler Finished
  {
    add => this.backing_Finished += value;
    remove => this.backing_Finished -= value;
  }

  protected void EmitSignalFinished()
  {
    ((GodotObject) this).EmitSignal(NRollingBoulderVfx.SignalName.Finished, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NRollingBoulderVfx.SignalName.HitCreature) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NRollingBoulderVfx.HitCreatureEventHandler backingHitCreature = this.backing_HitCreature;
      if (backingHitCreature == null)
        return;
      backingHitCreature(VariantUtils.ConvertTo<NCreature>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else if (StringName.op_Equality(ref signal, NRollingBoulderVfx.SignalName.Finished) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NRollingBoulderVfx.FinishedEventHandler backingFinished = this.backing_Finished;
      if (backingFinished == null)
        return;
      backingFinished();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NRollingBoulderVfx.SignalName.HitCreature) || StringName.op_Equality(ref signal, NRollingBoulderVfx.SignalName.Finished) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void HitCreatureEventHandler(
  #nullable enable
  NCreature creature);

  [Signal]
  public delegate void FinishedEventHandler();

  public class MethodName : Node2D.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName CleanUpBeforeEarlyExit = StringName.op_Implicit(nameof (CleanUpBeforeEarlyExit));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _boulder = StringName.op_Implicit(nameof (_boulder));
    public static readonly StringName _shadow = StringName.op_Implicit(nameof (_shadow));
    public static readonly StringName _slamBehind = StringName.op_Implicit(nameof (_slamBehind));
    public static readonly StringName _slamFront = StringName.op_Implicit(nameof (_slamFront));
  }

  public class SignalName : Node2D.SignalName
  {
    public static readonly StringName HitCreature = StringName.op_Implicit(nameof (HitCreature));
    public static readonly StringName Finished = StringName.op_Implicit(nameof (Finished));
  }
}
