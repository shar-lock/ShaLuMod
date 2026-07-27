// Decompiled with JetBrains decompiler
// Type: MegaCrit.sts2.Core.Nodes.TopBar.NTopBarGold
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.sts2.Core.Nodes.TopBar;

[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarGold.cs")]
public class NTopBarGold : NClickableControl
{
  private Player? _player;
  private MegaLabel _goldLabel;
  private MegaLabel _goldPopupLabel;
  private int _currentGold;
  private int _additionalGold;
  private bool _alreadyRunning;
  private CancellationTokenSource? _animCts;

  public override void _Ready()
  {
    this._goldLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%GoldLabel"));
    this._goldPopupLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%GoldPopup"));
    ((CanvasItem) this._goldPopupLabel).Modulate = Colors.Transparent;
    this.ConnectSignals();
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    this._animCts?.Cancel();
    if (this._player == null)
      return;
    this._player.GoldChanged -= new Action(this.UpdateGold);
  }

  public void Initialize(Player player)
  {
    this._player = player;
    this._currentGold = this._player.Gold;
    this._goldLabel.SetTextAutoSize($"{this._currentGold}");
    this._player.GoldChanged += new Action(this.UpdateGold);
  }

  private void UpdateGold() => TaskHelper.RunSafely(this.UpdateGoldAnim());

  private async Task UpdateGoldAnim()
  {
    CancellationToken ct;
    if (this._player == null)
    {
      ct = new CancellationToken();
    }
    else
    {
      this._additionalGold = this._currentGold = this._player.Gold - this._currentGold;
      this._currentGold = this._player.Gold;
      this._goldPopupLabel.SetTextAutoSize((this._additionalGold > 0 ? "+" : "") + this._additionalGold.ToString());
      if (this._alreadyRunning)
      {
        ct = new CancellationToken();
      }
      else
      {
        this._animCts = new CancellationTokenSource();
        ct = this._animCts.Token;
        this._alreadyRunning = true;
        try
        {
          Tween tween1 = ((Node) this).CreateTween().SetParallel(true);
          tween1.TweenProperty((GodotObject) this._goldPopupLabel, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.15000000596046448);
          tween1.TweenProperty((GodotObject) this._goldPopupLabel, NodePath.op_Implicit("position:y"), Variant.op_Implicit(((Control) this._goldPopupLabel).Position.Y + 30f), 0.25);
          await tween1.AwaitFinished(ct);
          await Task.Delay(150, ct);
          while (this._additionalGold != 0)
          {
            ct.ThrowIfCancellationRequested();
            int num = 1;
            if (Mathf.Abs(this._additionalGold) > 100)
              num = 75;
            else if (Mathf.Abs(this._additionalGold) > 50)
              num = 10;
            this._additionalGold = this._additionalGold > 0 ? this._additionalGold - num : this._additionalGold + num;
            this._goldPopupLabel.SetTextAutoSize((this._additionalGold >= 0 ? "+" : "") + this._additionalGold.ToString());
            this._goldLabel.SetTextAutoSize($"{this._player.Gold - this._additionalGold}");
            await Task.Delay((int) Mathf.Lerp(10f, 20f, (float) Mathf.Max(0, 10 - Mathf.Abs(this._additionalGold))), ct);
          }
          await Task.Delay(250, ct);
          Tween tween2 = ((Node) this).CreateTween().SetParallel(true);
          tween2.TweenProperty((GodotObject) this._goldPopupLabel, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.10000000149011612);
          tween2.TweenProperty((GodotObject) this._goldPopupLabel, NodePath.op_Implicit("position:y"), Variant.op_Implicit(((Control) this._goldPopupLabel).Position.Y - 30f), 0.25).FromCurrent();
          this._goldLabel.SetTextAutoSize($"{this._player.Gold}");
          ct = new CancellationToken();
        }
        catch (OperationCanceledException ex)
        {
          ct = new CancellationToken();
        }
        finally
        {
          this._alreadyRunning = false;
        }
      }
    }
  }

  protected override void OnFocus()
  {
    NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) new HoverTip(new LocString("static_hover_tips", "MONEY_POUCH.title"), new LocString("static_hover_tips", "MONEY_POUCH.description")))?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, new Vector2(0.0f, this.Size.Y + 20f)), false);
  }

  protected override void OnUnfocus() => NHoverTipSet.Remove((Control) this);

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NTopBarGold.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarGold.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarGold.MethodName.UpdateGold, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarGold.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarGold.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTopBarGold.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarGold.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarGold.MethodName.UpdateGold) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateGold();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarGold.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTopBarGold.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTopBarGold.MethodName._Ready) || StringName.op_Equality(ref method, NTopBarGold.MethodName._ExitTree) || StringName.op_Equality(ref method, NTopBarGold.MethodName.UpdateGold) || StringName.op_Equality(ref method, NTopBarGold.MethodName.OnFocus) || StringName.op_Equality(ref method, NTopBarGold.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTopBarGold.PropertyName._goldLabel))
    {
      this._goldLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarGold.PropertyName._goldPopupLabel))
    {
      this._goldPopupLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarGold.PropertyName._currentGold))
    {
      this._currentGold = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarGold.PropertyName._additionalGold))
    {
      this._additionalGold = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTopBarGold.PropertyName._alreadyRunning))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._alreadyRunning = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTopBarGold.PropertyName._goldLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._goldLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarGold.PropertyName._goldPopupLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._goldPopupLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarGold.PropertyName._currentGold))
    {
      value = VariantUtils.CreateFrom<int>(ref this._currentGold);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarGold.PropertyName._additionalGold))
    {
      value = VariantUtils.CreateFrom<int>(ref this._additionalGold);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTopBarGold.PropertyName._alreadyRunning))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._alreadyRunning);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NTopBarGold.PropertyName._goldLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBarGold.PropertyName._goldPopupLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NTopBarGold.PropertyName._currentGold, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NTopBarGold.PropertyName._additionalGold, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTopBarGold.PropertyName._alreadyRunning, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NTopBarGold.PropertyName._goldLabel, Variant.From<MegaLabel>(ref this._goldLabel));
    info.AddProperty(NTopBarGold.PropertyName._goldPopupLabel, Variant.From<MegaLabel>(ref this._goldPopupLabel));
    info.AddProperty(NTopBarGold.PropertyName._currentGold, Variant.From<int>(ref this._currentGold));
    info.AddProperty(NTopBarGold.PropertyName._additionalGold, Variant.From<int>(ref this._additionalGold));
    info.AddProperty(NTopBarGold.PropertyName._alreadyRunning, Variant.From<bool>(ref this._alreadyRunning));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTopBarGold.PropertyName._goldLabel, ref variant1))
      this._goldLabel = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NTopBarGold.PropertyName._goldPopupLabel, ref variant2))
      this._goldPopupLabel = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NTopBarGold.PropertyName._currentGold, ref variant3))
      this._currentGold = ((Variant) ref variant3).As<int>();
    Variant variant4;
    if (info.TryGetProperty(NTopBarGold.PropertyName._additionalGold, ref variant4))
      this._additionalGold = ((Variant) ref variant4).As<int>();
    Variant variant5;
    if (!info.TryGetProperty(NTopBarGold.PropertyName._alreadyRunning, ref variant5))
      return;
    this._alreadyRunning = ((Variant) ref variant5).As<bool>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName UpdateGold = StringName.op_Implicit(nameof (UpdateGold));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName _goldLabel = StringName.op_Implicit(nameof (_goldLabel));
    public static readonly StringName _goldPopupLabel = StringName.op_Implicit(nameof (_goldPopupLabel));
    public static readonly StringName _currentGold = StringName.op_Implicit(nameof (_currentGold));
    public static readonly StringName _additionalGold = StringName.op_Implicit(nameof (_additionalGold));
    public static readonly StringName _alreadyRunning = StringName.op_Implicit(nameof (_alreadyRunning));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
