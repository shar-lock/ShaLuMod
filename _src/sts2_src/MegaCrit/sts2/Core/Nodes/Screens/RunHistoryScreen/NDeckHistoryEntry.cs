// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.NDeckHistoryEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;

[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NDeckHistoryEntry.cs")]
public class NDeckHistoryEntry : NButton
{
  private MegaLabel _titleLabel;
  private NTinyCard _cardImage;
  private TextureRect _enchantmentImage;
  private MarginContainer _labelContainer;
  private Tween? _scaleTween;
  private int _amount;
  private 
  #nullable disable
  NDeckHistoryEntry.ClickedEventHandler backing_Clicked;

  private static 
  #nullable enable
  string ScenePath => SceneHelper.GetScenePath("screens/run_history_screen/deck_history_entry");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NDeckHistoryEntry.ScenePath);
    }
  }

  public IEnumerable<int> FloorsAddedToDeck { get; private set; }

  public CardModel Card { get; private set; }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._titleLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
    this._labelContainer = ((Node) this._titleLabel).GetParent<MarginContainer>();
    this._cardImage = ((Node) this).GetNode<NTinyCard>(NodePath.op_Implicit("%Card"));
    this._enchantmentImage = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Enchantment"));
    this._cardImage.PivotOffset = Vector2.op_Multiply(this._cardImage.Size, 0.5f);
    this.Reload();
  }

  public static NDeckHistoryEntry Create(CardModel card, int amount)
  {
    return NDeckHistoryEntry.Create(card, amount, (IEnumerable<int>) Array.Empty<int>());
  }

  public static NDeckHistoryEntry Create(CardModel card, int amount, IEnumerable<int> floorsAdded)
  {
    NDeckHistoryEntry ndeckHistoryEntry = PreloadManager.Cache.GetScene(NDeckHistoryEntry.ScenePath).Instantiate<NDeckHistoryEntry>((PackedScene.GenEditState) 0L);
    ndeckHistoryEntry.Card = card;
    ndeckHistoryEntry._amount = amount;
    ndeckHistoryEntry.FloorsAddedToDeck = floorsAdded;
    return ndeckHistoryEntry;
  }

  private void Reload()
  {
    this._titleLabel.SetTextAutoSize(this.Card.Title);
    bool flag1 = this.Card.CurrentUpgradeLevel >= 1;
    bool flag2 = this.Card.Enchantment != null;
    string text = this.Card.Title;
    if (this._amount > 1)
      text = $"{this._amount}x {text}";
    this._titleLabel.SetTextAutoSize(text);
    if (flag2)
      ((Control) this._titleLabel).AddThemeColorOverride(ThemeConstants.Label.FontColor, StsColors.purple);
    else if (flag1)
      ((Control) this._titleLabel).AddThemeColorOverride(ThemeConstants.Label.FontColor, StsColors.green);
    this._cardImage.SetCard(this.Card);
    if (this.Card.Enchantment != null)
      this._enchantmentImage.Texture = (Texture2D) this.Card.Enchantment.Icon;
    ((CanvasItem) this._enchantmentImage).Visible = this.Card.Enchantment != null;
    this.Size = new Vector2((float) ((double) this._cardImage.Size.X + (double) ((Control) this._titleLabel).Size.X + 10.0), this.Size.Y);
  }

  protected override void OnFocus()
  {
    Tween scaleTween = this._scaleTween;
    if (scaleTween != null)
      scaleTween.FastForwardToCompletion();
    this._scaleTween = ((Node) this).CreateTween().SetParallel(true);
    this._scaleTween.TweenProperty((GodotObject) this._cardImage, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.5f)), 0.05);
    this._scaleTween.TweenProperty((GodotObject) this._labelContainer, NodePath.op_Implicit("position:x"), Variant.op_Implicit(((Control) this._labelContainer).Position.X + 8f), 0.05);
  }

  protected override void OnUnfocus()
  {
    Tween scaleTween = this._scaleTween;
    if (scaleTween != null)
      scaleTween.FastForwardToCompletion();
    this._scaleTween = ((Node) this).CreateTween().SetParallel(true);
    this._scaleTween.TweenProperty((GodotObject) this._cardImage, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 1L);
    this._scaleTween.TweenProperty((GodotObject) this._labelContainer, NodePath.op_Implicit("position:x"), Variant.op_Implicit(((Control) this._labelContainer).Position.X - 8f), 0.5).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 1L);
  }

  protected override void OnRelease()
  {
    ((GodotObject) this).EmitSignal(NDeckHistoryEntry.SignalName.Clicked, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NDeckHistoryEntry.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckHistoryEntry.MethodName.Reload, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckHistoryEntry.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckHistoryEntry.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeckHistoryEntry.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDeckHistoryEntry.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckHistoryEntry.MethodName.Reload) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Reload();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckHistoryEntry.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeckHistoryEntry.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDeckHistoryEntry.MethodName.OnRelease) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnRelease();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDeckHistoryEntry.MethodName._Ready) || StringName.op_Equality(ref method, NDeckHistoryEntry.MethodName.Reload) || StringName.op_Equality(ref method, NDeckHistoryEntry.MethodName.OnFocus) || StringName.op_Equality(ref method, NDeckHistoryEntry.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NDeckHistoryEntry.MethodName.OnRelease) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeckHistoryEntry.PropertyName._titleLabel))
    {
      this._titleLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckHistoryEntry.PropertyName._cardImage))
    {
      this._cardImage = VariantUtils.ConvertTo<NTinyCard>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckHistoryEntry.PropertyName._enchantmentImage))
    {
      this._enchantmentImage = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckHistoryEntry.PropertyName._labelContainer))
    {
      this._labelContainer = VariantUtils.ConvertTo<MarginContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckHistoryEntry.PropertyName._scaleTween))
    {
      this._scaleTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeckHistoryEntry.PropertyName._amount))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._amount = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeckHistoryEntry.PropertyName._titleLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._titleLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckHistoryEntry.PropertyName._cardImage))
    {
      value = VariantUtils.CreateFrom<NTinyCard>(ref this._cardImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckHistoryEntry.PropertyName._enchantmentImage))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._enchantmentImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckHistoryEntry.PropertyName._labelContainer))
    {
      value = VariantUtils.CreateFrom<MarginContainer>(ref this._labelContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeckHistoryEntry.PropertyName._scaleTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._scaleTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeckHistoryEntry.PropertyName._amount))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<int>(ref this._amount);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDeckHistoryEntry.PropertyName._titleLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckHistoryEntry.PropertyName._cardImage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckHistoryEntry.PropertyName._enchantmentImage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckHistoryEntry.PropertyName._labelContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeckHistoryEntry.PropertyName._scaleTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NDeckHistoryEntry.PropertyName._amount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NDeckHistoryEntry.PropertyName._titleLabel, Variant.From<MegaLabel>(ref this._titleLabel));
    info.AddProperty(NDeckHistoryEntry.PropertyName._cardImage, Variant.From<NTinyCard>(ref this._cardImage));
    info.AddProperty(NDeckHistoryEntry.PropertyName._enchantmentImage, Variant.From<TextureRect>(ref this._enchantmentImage));
    info.AddProperty(NDeckHistoryEntry.PropertyName._labelContainer, Variant.From<MarginContainer>(ref this._labelContainer));
    info.AddProperty(NDeckHistoryEntry.PropertyName._scaleTween, Variant.From<Tween>(ref this._scaleTween));
    info.AddProperty(NDeckHistoryEntry.PropertyName._amount, Variant.From<int>(ref this._amount));
    info.AddSignalEventDelegate(NDeckHistoryEntry.SignalName.Clicked, (Delegate) this.backing_Clicked);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDeckHistoryEntry.PropertyName._titleLabel, ref variant1))
      this._titleLabel = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NDeckHistoryEntry.PropertyName._cardImage, ref variant2))
      this._cardImage = ((Variant) ref variant2).As<NTinyCard>();
    Variant variant3;
    if (info.TryGetProperty(NDeckHistoryEntry.PropertyName._enchantmentImage, ref variant3))
      this._enchantmentImage = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NDeckHistoryEntry.PropertyName._labelContainer, ref variant4))
      this._labelContainer = ((Variant) ref variant4).As<MarginContainer>();
    Variant variant5;
    if (info.TryGetProperty(NDeckHistoryEntry.PropertyName._scaleTween, ref variant5))
      this._scaleTween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (info.TryGetProperty(NDeckHistoryEntry.PropertyName._amount, ref variant6))
      this._amount = ((Variant) ref variant6).As<int>();
    NDeckHistoryEntry.ClickedEventHandler clickedEventHandler;
    if (!info.TryGetSignalEventDelegate<NDeckHistoryEntry.ClickedEventHandler>(NDeckHistoryEntry.SignalName.Clicked, ref clickedEventHandler))
      return;
    this.backing_Clicked = clickedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NDeckHistoryEntry.SignalName.Clicked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("entry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NDeckHistoryEntry.ClickedEventHandler Clicked
  {
    add => this.backing_Clicked += value;
    remove => this.backing_Clicked -= value;
  }

  protected void EmitSignalClicked(NDeckHistoryEntry entry)
  {
    ((GodotObject) this).EmitSignal(NDeckHistoryEntry.SignalName.Clicked, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) entry)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NDeckHistoryEntry.SignalName.Clicked) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NDeckHistoryEntry.ClickedEventHandler backingClicked = this.backing_Clicked;
      if (backingClicked == null)
        return;
      backingClicked(VariantUtils.ConvertTo<NDeckHistoryEntry>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      base.RaiseGodotClassSignalCallbacks(in signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NDeckHistoryEntry.SignalName.Clicked) || base.HasGodotClassSignal(in signal);
  }

  [Signal]
  public delegate void ClickedEventHandler(
  #nullable enable
  NDeckHistoryEntry entry);

  public new class MethodName : NButton.MethodName
  {
    public new static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Reload = StringName.op_Implicit(nameof (Reload));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _titleLabel = StringName.op_Implicit(nameof (_titleLabel));
    public static readonly StringName _cardImage = StringName.op_Implicit(nameof (_cardImage));
    public static readonly StringName _enchantmentImage = StringName.op_Implicit(nameof (_enchantmentImage));
    public static readonly StringName _labelContainer = StringName.op_Implicit(nameof (_labelContainer));
    public static readonly StringName _scaleTween = StringName.op_Implicit(nameof (_scaleTween));
    public static readonly StringName _amount = StringName.op_Implicit(nameof (_amount));
  }

  public new class SignalName : NButton.SignalName
  {
    public static readonly StringName Clicked = StringName.op_Implicit(nameof (Clicked));
  }
}
