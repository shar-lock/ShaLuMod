// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.TopBar.NTopBarDeckButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.TopBar;

[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarDeckButton.cs")]
public class NTopBarDeckButton : NTopBarButton
{
  private static readonly StringName _v = new StringName("v");
  private float _elapsedTime;
  private const float _rockSpeed = 4f;
  private const float _rockDist = 0.12f;
  private float _rockBaseRotation;
  private const float _defaultV = 0.9f;
  private Player _player;
  private CardPile _pile;
  private MegaLabel _countLabel;
  private float _count;
  private Tween? _bumpTween;

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.viewDeckAndTabLeft)
      };
    }
  }

  public override void _Ready()
  {
    this.InitTopBarButton();
    this._countLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("DeckCardCount"));
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._pile.CardAddFinished -= new Action(this.OnPileContentsChanged);
    this._pile.CardRemoveFinished -= new Action(this.OnPileContentsChanged);
  }

  public void Initialize(Player player)
  {
    this._player = player;
    this._pile = PileType.Deck.GetPile(player);
    this._pile.CardAddFinished += new Action(this.OnPileContentsChanged);
    this._pile.CardRemoveFinished += new Action(this.OnPileContentsChanged);
    this.OnPileContentsChanged();
  }

  private void OnPileContentsChanged()
  {
    int count = this._pile.Cards.Count;
    if ((double) count > (double) this._count)
    {
      this._bumpTween?.Kill();
      this._bumpTween = ((Node) this).CreateTween();
      this._bumpTween.TweenProperty((GodotObject) this._countLabel, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).From(Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.5f))).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      ((Control) this._countLabel).PivotOffset = Vector2.op_Multiply(((Control) this._countLabel).Size, 0.5f);
      this._count = (float) count;
    }
    this._countLabel.SetTextAutoSize(count.ToString());
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    if (this.IsOpen())
      NCapstoneContainer.Instance.Close();
    else
      NDeckViewScreen.ShowScreen(this._player);
    this.UpdateScreenOpen();
    this._hsv?.SetShaderParameter(NTopBarDeckButton._v, Variant.op_Implicit(0.9f));
  }

  protected override bool IsOpen()
  {
    return NCapstoneContainer.Instance.CurrentCapstoneScreen is NDeckViewScreen;
  }

  public override void _Process(double delta)
  {
    if (!this.IsScreenOpen)
      return;
    this._elapsedTime += (float) delta * 4f;
    this._icon.Rotation = this._rockBaseRotation + 0.12f * Mathf.Sin(this._elapsedTime);
    this._rockBaseRotation = (float) Mathf.Lerp((double) this._rockBaseRotation, 0.0, delta);
  }

  public void ToggleAnimState() => this.UpdateScreenOpen();

  protected override void OnFocus()
  {
    base.OnFocus();
    LocString title = new LocString("static_hover_tips", "DECK.title");
    title.Add("Hotkey", NInputManager.Instance.GetShortcutKey(MegaInput.viewDeckAndTabLeft).ToString());
    NHoverTipSet andShow = NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) new HoverTip(title, new LocString("static_hover_tips", "DECK.description")));
    andShow?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, new Vector2(this.Size.X - andShow.Size.X, this.Size.Y + 20f)), false);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    NHoverTipSet.Remove((Control) this);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NTopBarDeckButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarDeckButton.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarDeckButton.MethodName.OnPileContentsChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarDeckButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarDeckButton.MethodName.IsOpen, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarDeckButton.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTopBarDeckButton.MethodName.ToggleAnimState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarDeckButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarDeckButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTopBarDeckButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarDeckButton.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarDeckButton.MethodName.OnPileContentsChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPileContentsChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarDeckButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarDeckButton.MethodName.IsOpen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.IsOpen();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarDeckButton.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarDeckButton.MethodName.ToggleAnimState) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ToggleAnimState();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarDeckButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTopBarDeckButton.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTopBarDeckButton.MethodName._Ready) || StringName.op_Equality(ref method, NTopBarDeckButton.MethodName._ExitTree) || StringName.op_Equality(ref method, NTopBarDeckButton.MethodName.OnPileContentsChanged) || StringName.op_Equality(ref method, NTopBarDeckButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NTopBarDeckButton.MethodName.IsOpen) || StringName.op_Equality(ref method, NTopBarDeckButton.MethodName._Process) || StringName.op_Equality(ref method, NTopBarDeckButton.MethodName.ToggleAnimState) || StringName.op_Equality(ref method, NTopBarDeckButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NTopBarDeckButton.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTopBarDeckButton.PropertyName._elapsedTime))
    {
      this._elapsedTime = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarDeckButton.PropertyName._rockBaseRotation))
    {
      this._rockBaseRotation = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarDeckButton.PropertyName._countLabel))
    {
      this._countLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarDeckButton.PropertyName._count))
    {
      this._count = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTopBarDeckButton.PropertyName._bumpTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._bumpTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTopBarDeckButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarDeckButton.PropertyName._elapsedTime))
    {
      value = VariantUtils.CreateFrom<float>(ref this._elapsedTime);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarDeckButton.PropertyName._rockBaseRotation))
    {
      value = VariantUtils.CreateFrom<float>(ref this._rockBaseRotation);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarDeckButton.PropertyName._countLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._countLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarDeckButton.PropertyName._count))
    {
      value = VariantUtils.CreateFrom<float>(ref this._count);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTopBarDeckButton.PropertyName._bumpTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._bumpTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 34L, NTopBarDeckButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NTopBarDeckButton.PropertyName._elapsedTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NTopBarDeckButton.PropertyName._rockBaseRotation, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBarDeckButton.PropertyName._countLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NTopBarDeckButton.PropertyName._count, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBarDeckButton.PropertyName._bumpTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NTopBarDeckButton.PropertyName._elapsedTime, Variant.From<float>(ref this._elapsedTime));
    info.AddProperty(NTopBarDeckButton.PropertyName._rockBaseRotation, Variant.From<float>(ref this._rockBaseRotation));
    info.AddProperty(NTopBarDeckButton.PropertyName._countLabel, Variant.From<MegaLabel>(ref this._countLabel));
    info.AddProperty(NTopBarDeckButton.PropertyName._count, Variant.From<float>(ref this._count));
    info.AddProperty(NTopBarDeckButton.PropertyName._bumpTween, Variant.From<Tween>(ref this._bumpTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTopBarDeckButton.PropertyName._elapsedTime, ref variant1))
      this._elapsedTime = ((Variant) ref variant1).As<float>();
    Variant variant2;
    if (info.TryGetProperty(NTopBarDeckButton.PropertyName._rockBaseRotation, ref variant2))
      this._rockBaseRotation = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (info.TryGetProperty(NTopBarDeckButton.PropertyName._countLabel, ref variant3))
      this._countLabel = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NTopBarDeckButton.PropertyName._count, ref variant4))
      this._count = ((Variant) ref variant4).As<float>();
    Variant variant5;
    if (!info.TryGetProperty(NTopBarDeckButton.PropertyName._bumpTween, ref variant5))
      return;
    this._bumpTween = ((Variant) ref variant5).As<Tween>();
  }

  public new class MethodName : NTopBarButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnPileContentsChanged = StringName.op_Implicit(nameof (OnPileContentsChanged));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName IsOpen = StringName.op_Implicit(nameof (IsOpen));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName ToggleAnimState = StringName.op_Implicit(nameof (ToggleAnimState));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NTopBarButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _elapsedTime = StringName.op_Implicit(nameof (_elapsedTime));
    public static readonly StringName _rockBaseRotation = StringName.op_Implicit(nameof (_rockBaseRotation));
    public static readonly StringName _countLabel = StringName.op_Implicit(nameof (_countLabel));
    public static readonly StringName _count = StringName.op_Implicit(nameof (_count));
    public static readonly StringName _bumpTween = StringName.op_Implicit(nameof (_bumpTween));
  }

  public new class SignalName : NTopBarButton.SignalName
  {
  }
}
