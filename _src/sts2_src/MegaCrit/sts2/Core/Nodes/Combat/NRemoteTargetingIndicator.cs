// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NRemoteTargetingIndicator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NRemoteTargetingIndicator.cs")]
public class NRemoteTargetingIndicator : Node2D
{
  private const int _segmentCount = 100;
  private const float _defaultAlpha = 0.5f;
  private const float _targetingAlpha = 1f;
  private Player _player;
  private Vector2 _fromPosition;
  private Vector2 _toPosition;
  private Line2D _line;
  private Line2D _lineBack;
  private Tween? _tween;
  private bool _isTargetingCreature;

  public override void _Ready()
  {
    this._line = ((Node) this).GetNode<Line2D>(NodePath.op_Implicit("Line"));
    this._lineBack = ((Node) this).GetNode<Line2D>(NodePath.op_Implicit("LineBack"));
    for (int index = 0; index < 101; ++index)
    {
      this._line.AddPoint(Vector2.Zero, -1);
      this._lineBack.AddPoint(Vector2.Zero, -1);
    }
    this.StopDrawing();
  }

  public void Initialize(Player player)
  {
    this._player = player;
    CharacterModel character = player.Character;
    this._line.DefaultColor = character.RemoteTargetingLineColor;
    this._lineBack.DefaultColor = character.RemoteTargetingLineOutline;
    Gradient gradient1 = this._line.GetGradient();
    if (gradient1 != null)
    {
      for (int index = 0; index < gradient1.GetPointCount(); ++index)
        gradient1.SetColor(index, Color.op_Multiply(gradient1.GetColor(index), character.RemoteTargetingLineColor));
      this._line.SetGradient(gradient1);
    }
    Gradient gradient2 = this._lineBack.GetGradient();
    if (gradient2 == null)
      return;
    for (int index = 0; index < gradient2.GetPointCount(); ++index)
      gradient2.SetColor(index, Color.op_Multiply(gradient2.GetColor(index), character.RemoteTargetingLineOutline));
    this._lineBack.SetGradient(gradient1);
  }

  public override void _Process(double delta)
  {
    Vector2 zero = Vector2.Zero;
    zero.X = this._fromPosition.X + (float) (((double) this._toPosition.X - (double) this._fromPosition.X) * 0.5);
    zero.Y = this._fromPosition.Y - (float) (((double) this._toPosition.Y - (double) this._fromPosition.Y) * 0.5);
    for (int index = 0; index < 100; ++index)
    {
      Vector2 vector2 = MathHelper.BezierCurve(this._fromPosition, this._toPosition, zero, (float) index / 101f);
      this._line.SetPointPosition(index, vector2);
      this._lineBack.SetPointPosition(index, vector2);
    }
    this._line.SetPointPosition(100, this._toPosition);
    this._lineBack.SetPointPosition(100, this._toPosition);
    bool isTargetingCreature = false;
    ICombatState combatState = this._player.Creature.CombatState;
    foreach (Creature creature in (IEnumerable<Creature>) ((combatState != null ? (object) combatState.Enemies : (object) null) ?? (object) Array.Empty<Creature>()))
    {
      NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(creature);
      if (creatureNode != null)
      {
        Rect2 globalRect = creatureNode.Hitbox.GetGlobalRect();
        if (((Rect2) ref globalRect).HasPoint(Vector2.op_Addition(this.GlobalPosition, this._toPosition)))
        {
          isTargetingCreature = true;
          break;
        }
      }
    }
    this.DoTargetingCreatureTween(isTargetingCreature);
  }

  public void StartDrawingFrom(Vector2 from)
  {
    if (NCombatUi.IsDebugHideMpTargetingUi)
      return;
    this._fromPosition = from;
    ((CanvasItem) this).Visible = true;
    ((Node) this).ProcessMode = ((CanvasItem) this).Visible ? (Node.ProcessModeEnum) 0L : (Node.ProcessModeEnum) 4L;
  }

  public void StopDrawing()
  {
    ((CanvasItem) this).Visible = false;
    ((Node) this).ProcessMode = (Node.ProcessModeEnum) 4L;
    Color modulate = ((CanvasItem) this).Modulate;
    modulate.A = 0.5f;
    ((CanvasItem) this).Modulate = modulate;
  }

  public void UpdateDrawingTo(Vector2 position) => this._toPosition = position;

  private void DoTargetingCreatureTween(bool isTargetingCreature)
  {
    if (isTargetingCreature == this._isTargetingCreature)
      return;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    if (isTargetingCreature)
      this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.10000000149011612);
    else
      this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.5f), 0.25);
    this._isTargetingCreature = isTargetingCreature;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NRemoteTargetingIndicator.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRemoteTargetingIndicator.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteTargetingIndicator.MethodName.StartDrawingFrom, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("from"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteTargetingIndicator.MethodName.StopDrawing, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRemoteTargetingIndicator.MethodName.UpdateDrawingTo, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteTargetingIndicator.MethodName.DoTargetingCreatureTween, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isTargetingCreature"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRemoteTargetingIndicator.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteTargetingIndicator.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteTargetingIndicator.MethodName.StartDrawingFrom) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.StartDrawingFrom(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteTargetingIndicator.MethodName.StopDrawing) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopDrawing();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteTargetingIndicator.MethodName.UpdateDrawingTo) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateDrawingTo(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRemoteTargetingIndicator.MethodName.DoTargetingCreatureTween) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.DoTargetingCreatureTween(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRemoteTargetingIndicator.MethodName._Ready) || StringName.op_Equality(ref method, NRemoteTargetingIndicator.MethodName._Process) || StringName.op_Equality(ref method, NRemoteTargetingIndicator.MethodName.StartDrawingFrom) || StringName.op_Equality(ref method, NRemoteTargetingIndicator.MethodName.StopDrawing) || StringName.op_Equality(ref method, NRemoteTargetingIndicator.MethodName.UpdateDrawingTo) || StringName.op_Equality(ref method, NRemoteTargetingIndicator.MethodName.DoTargetingCreatureTween) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRemoteTargetingIndicator.PropertyName._fromPosition))
    {
      this._fromPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteTargetingIndicator.PropertyName._toPosition))
    {
      this._toPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteTargetingIndicator.PropertyName._line))
    {
      this._line = VariantUtils.ConvertTo<Line2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteTargetingIndicator.PropertyName._lineBack))
    {
      this._lineBack = VariantUtils.ConvertTo<Line2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteTargetingIndicator.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRemoteTargetingIndicator.PropertyName._isTargetingCreature))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isTargetingCreature = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRemoteTargetingIndicator.PropertyName._fromPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._fromPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteTargetingIndicator.PropertyName._toPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._toPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteTargetingIndicator.PropertyName._line))
    {
      value = VariantUtils.CreateFrom<Line2D>(ref this._line);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteTargetingIndicator.PropertyName._lineBack))
    {
      value = VariantUtils.CreateFrom<Line2D>(ref this._lineBack);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteTargetingIndicator.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRemoteTargetingIndicator.PropertyName._isTargetingCreature))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isTargetingCreature);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 5L, NRemoteTargetingIndicator.PropertyName._fromPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NRemoteTargetingIndicator.PropertyName._toPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteTargetingIndicator.PropertyName._line, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteTargetingIndicator.PropertyName._lineBack, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteTargetingIndicator.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRemoteTargetingIndicator.PropertyName._isTargetingCreature, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRemoteTargetingIndicator.PropertyName._fromPosition, Variant.From<Vector2>(ref this._fromPosition));
    info.AddProperty(NRemoteTargetingIndicator.PropertyName._toPosition, Variant.From<Vector2>(ref this._toPosition));
    info.AddProperty(NRemoteTargetingIndicator.PropertyName._line, Variant.From<Line2D>(ref this._line));
    info.AddProperty(NRemoteTargetingIndicator.PropertyName._lineBack, Variant.From<Line2D>(ref this._lineBack));
    info.AddProperty(NRemoteTargetingIndicator.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NRemoteTargetingIndicator.PropertyName._isTargetingCreature, Variant.From<bool>(ref this._isTargetingCreature));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRemoteTargetingIndicator.PropertyName._fromPosition, ref variant1))
      this._fromPosition = ((Variant) ref variant1).As<Vector2>();
    Variant variant2;
    if (info.TryGetProperty(NRemoteTargetingIndicator.PropertyName._toPosition, ref variant2))
      this._toPosition = ((Variant) ref variant2).As<Vector2>();
    Variant variant3;
    if (info.TryGetProperty(NRemoteTargetingIndicator.PropertyName._line, ref variant3))
      this._line = ((Variant) ref variant3).As<Line2D>();
    Variant variant4;
    if (info.TryGetProperty(NRemoteTargetingIndicator.PropertyName._lineBack, ref variant4))
      this._lineBack = ((Variant) ref variant4).As<Line2D>();
    Variant variant5;
    if (info.TryGetProperty(NRemoteTargetingIndicator.PropertyName._tween, ref variant5))
      this._tween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (!info.TryGetProperty(NRemoteTargetingIndicator.PropertyName._isTargetingCreature, ref variant6))
      return;
    this._isTargetingCreature = ((Variant) ref variant6).As<bool>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName StartDrawingFrom = StringName.op_Implicit(nameof (StartDrawingFrom));
    public static readonly StringName StopDrawing = StringName.op_Implicit(nameof (StopDrawing));
    public static readonly StringName UpdateDrawingTo = StringName.op_Implicit(nameof (UpdateDrawingTo));
    public static readonly StringName DoTargetingCreatureTween = StringName.op_Implicit(nameof (DoTargetingCreatureTween));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _fromPosition = StringName.op_Implicit(nameof (_fromPosition));
    public static readonly StringName _toPosition = StringName.op_Implicit(nameof (_toPosition));
    public static readonly StringName _line = StringName.op_Implicit(nameof (_line));
    public static readonly StringName _lineBack = StringName.op_Implicit(nameof (_lineBack));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _isTargetingCreature = StringName.op_Implicit(nameof (_isTargetingCreature));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
