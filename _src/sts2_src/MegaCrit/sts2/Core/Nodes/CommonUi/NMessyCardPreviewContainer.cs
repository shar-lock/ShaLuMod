// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NMessyCardPreviewContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NMessyCardPreviewContainer.cs")]
public class NMessyCardPreviewContainer : Control
{
  private const float _spacing = 150f;
  private const int _resetNewCardMsec = 2000;
  private ulong _resetNewCardTimer;
  private float _currentMaxPosition;
  private IEnumerator<Vector2>? _samples;

  public override void _Ready()
  {
    ((GodotObject) this).Connect(Node.SignalName.ChildEnteredTree, Callable.From<Node>(new Action<Node>(this.PositionNewChild)), 0U);
  }

  private void PositionNewChild(Node node)
  {
    if (Time.GetTicksMsec() > this._resetNewCardTimer)
    {
      this._currentMaxPosition = 0.0f;
      this.ResetSamples();
    }
    this._resetNewCardTimer = Time.GetTicksMsec() + 2000UL;
    if (!this._samples.MoveNext())
      this.ResetSamples();
    Vector2 current = this._samples.Current;
    switch (node)
    {
      case Control control:
        control.Position = current;
        break;
      case Node2D node2D:
        node2D.Position = current;
        break;
    }
  }

  private void ResetSamples()
  {
    this._samples = new NMessyCardPreviewContainer.PoissonDiscSampler(this.Size.X, this.Size.Y, 150f).Samples();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NMessyCardPreviewContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMessyCardPreviewContainer.MethodName.PositionNewChild, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMessyCardPreviewContainer.MethodName.ResetSamples, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMessyCardPreviewContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMessyCardPreviewContainer.MethodName.PositionNewChild) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PositionNewChild(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMessyCardPreviewContainer.MethodName.ResetSamples) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ResetSamples();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMessyCardPreviewContainer.MethodName._Ready) || StringName.op_Equality(ref method, NMessyCardPreviewContainer.MethodName.PositionNewChild) || StringName.op_Equality(ref method, NMessyCardPreviewContainer.MethodName.ResetSamples) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMessyCardPreviewContainer.PropertyName._resetNewCardTimer))
    {
      this._resetNewCardTimer = VariantUtils.ConvertTo<ulong>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMessyCardPreviewContainer.PropertyName._currentMaxPosition))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._currentMaxPosition = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMessyCardPreviewContainer.PropertyName._resetNewCardTimer))
    {
      value = VariantUtils.CreateFrom<ulong>(ref this._resetNewCardTimer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMessyCardPreviewContainer.PropertyName._currentMaxPosition))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._currentMaxPosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NMessyCardPreviewContainer.PropertyName._resetNewCardTimer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMessyCardPreviewContainer.PropertyName._currentMaxPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMessyCardPreviewContainer.PropertyName._resetNewCardTimer, Variant.From<ulong>(ref this._resetNewCardTimer));
    info.AddProperty(NMessyCardPreviewContainer.PropertyName._currentMaxPosition, Variant.From<float>(ref this._currentMaxPosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMessyCardPreviewContainer.PropertyName._resetNewCardTimer, ref variant1))
      this._resetNewCardTimer = ((Variant) ref variant1).As<ulong>();
    Variant variant2;
    if (!info.TryGetProperty(NMessyCardPreviewContainer.PropertyName._currentMaxPosition, ref variant2))
      return;
    this._currentMaxPosition = ((Variant) ref variant2).As<float>();
  }

  public class PoissonDiscSampler
  {
    private const int _maxAttempts = 30;
    private readonly Rect2 _rect;
    private readonly float _radius2;
    private readonly float _cellSize;
    private readonly 
    #nullable enable
    Vector2[,] _grid;
    private readonly List<Vector2> _activeSamples = new List<Vector2>();

    public PoissonDiscSampler(float width, float height, float radius)
    {
      this._rect = new Rect2(0.0f, 0.0f, width, height);
      this._radius2 = radius * radius;
      this._cellSize = radius / Mathf.Sqrt(2f);
      this._grid = new Vector2[Mathf.CeilToInt(width / this._cellSize), Mathf.CeilToInt(height / this._cellSize)];
    }

    public IEnumerator<Vector2> Samples()
    {
      yield return this.AddSample(Vector2.op_Division(((Rect2) ref this._rect).Size, 2f));
      while (this._activeSamples.Count > 0)
      {
        int i = (int) ((double) Rng.Chaotic.NextFloat() * (double) this._activeSamples.Count);
        Vector2 activeSample = this._activeSamples[i];
        bool found = false;
        for (int index = 0; index < 30; ++index)
        {
          float num1 = 6.28318548f * Rng.Chaotic.NextFloat();
          float num2 = Mathf.Sqrt(Rng.Chaotic.NextFloat() * 3f * this._radius2 + this._radius2);
          Vector2 sample = Vector2.op_Addition(activeSample, Vector2.op_Multiply(num2, new Vector2(Mathf.Cos(num1), Mathf.Sin(num1))));
          if (((Rect2) ref this._rect).HasPoint(sample) && this.IsFarEnough(sample))
          {
            found = true;
            yield return this.AddSample(sample);
            break;
          }
        }
        if (!found)
        {
          this._activeSamples[i] = this._activeSamples[this._activeSamples.Count - 1];
          this._activeSamples.RemoveAt(this._activeSamples.Count - 1);
        }
      }
    }

    private bool IsFarEnough(Vector2 sample)
    {
      NMessyCardPreviewContainer.PoissonDiscSampler.GridPos gridPos = new NMessyCardPreviewContainer.PoissonDiscSampler.GridPos(sample, this._cellSize);
      int num1 = Mathf.Max(gridPos.x - 2, 0);
      int num2 = Mathf.Max(gridPos.y - 2, 0);
      int num3 = Mathf.Min(gridPos.x + 2, this._grid.GetLength(0) - 1);
      int num4 = Mathf.Min(gridPos.y + 2, this._grid.GetLength(1) - 1);
      for (int index1 = num2; index1 <= num4; ++index1)
      {
        for (int index2 = num1; index2 <= num3; ++index2)
        {
          Vector2 vector2_1 = this._grid[index2, index1];
          if (Vector2.op_Inequality(vector2_1, Vector2.Zero))
          {
            Vector2 vector2_2 = Vector2.op_Subtraction(vector2_1, sample);
            if ((double) vector2_2.X * (double) vector2_2.X + (double) vector2_2.Y * (double) vector2_2.Y < (double) this._radius2)
              return false;
          }
        }
      }
      return true;
    }

    private Vector2 AddSample(Vector2 sample)
    {
      this._activeSamples.Add(sample);
      NMessyCardPreviewContainer.PoissonDiscSampler.GridPos gridPos = new NMessyCardPreviewContainer.PoissonDiscSampler.GridPos(sample, this._cellSize);
      this._grid[gridPos.x, gridPos.y] = sample;
      return sample;
    }

    private struct GridPos
    {
      public readonly int x;
      public readonly int y;

      public GridPos(Vector2 sample, float cellSize)
      {
        this.x = (int) ((double) sample.X / (double) cellSize);
        this.y = (int) ((double) sample.Y / (double) cellSize);
      }
    }
  }

  public class MethodName : Control.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName PositionNewChild = StringName.op_Implicit(nameof (PositionNewChild));
    public static readonly StringName ResetSamples = StringName.op_Implicit(nameof (ResetSamples));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _resetNewCardTimer = StringName.op_Implicit(nameof (_resetNewCardTimer));
    public static readonly StringName _currentMaxPosition = StringName.op_Implicit(nameof (_currentMaxPosition));
  }

  public class SignalName : Control.SignalName
  {
  }
}
