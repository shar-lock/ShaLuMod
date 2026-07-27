// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NCustomRunModifiersList
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Modifiers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NCustomRunModifiersList.cs")]
public class NCustomRunModifiersList : Control
{
  private readonly List<NRunModifierTickbox> _modifierTickboxes = new List<NRunModifierTickbox>();
  private Control _container;
  private MultiplayerUiMode _mode;
  private 
  #nullable disable
  NCustomRunModifiersList.ModifiersChangedEventHandler backing_ModifiersChanged;

  public override void _Ready()
  {
    this._container = ((Node) this).GetNode<Control>(NodePath.op_Implicit("ScrollContainer/Mask/Content"));
    foreach (Node child in ((Node) this._container).GetChildren(false))
      child.QueueFreeSafely();
    foreach (ModifierModel allModifier in this.GetAllModifiers())
    {
      NRunModifierTickbox child = NRunModifierTickbox.Create(allModifier);
      ((Node) this._container).AddChildSafely((Node) child);
      this._modifierTickboxes.Add(child);
      ((GodotObject) child).Connect(NTickbox.SignalName.Toggled, Callable.From<NRunModifierTickbox>(new Action<NRunModifierTickbox>(this.AfterModifiersChanged)), 0U);
    }
  }

  public void Initialize(MultiplayerUiMode mode)
  {
    this._mode = mode;
    bool flag;
    switch (mode)
    {
      case MultiplayerUiMode.Client:
      case MultiplayerUiMode.Load:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag)
      return;
    foreach (NClickableControl modifierTickbox in this._modifierTickboxes)
      modifierTickbox.Disable();
  }

  public void SyncModifierList(
  #nullable enable
  IReadOnlyCollection<ModifierModel> modifiers)
  {
    bool flag;
    switch (this._mode)
    {
      case MultiplayerUiMode.Singleplayer:
      case MultiplayerUiMode.Host:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag)
      throw new InvalidOperationException("This should only be called in client or load mode!");
    foreach (NRunModifierTickbox modifierTickbox in this._modifierTickboxes)
    {
      NRunModifierTickbox tickbox = modifierTickbox;
      tickbox.IsTicked = modifiers.FirstOrDefault<ModifierModel>((Func<ModifierModel, bool>) (m => m.IsEquivalent(tickbox.Modifier))) != null;
    }
  }

  public void SetTickedModifiers(IReadOnlyCollection<ModifierModel> modifiers)
  {
    bool flag;
    switch (this._mode)
    {
      case MultiplayerUiMode.Client:
      case MultiplayerUiMode.Load:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag)
      throw new InvalidOperationException("This should only be called in host or singleplayer mode!");
    foreach (NRunModifierTickbox modifierTickbox in this._modifierTickboxes)
    {
      NRunModifierTickbox tickbox = modifierTickbox;
      tickbox.IsTicked = modifiers.Any<ModifierModel>((Func<ModifierModel, bool>) (m => m.IsEquivalent(tickbox.Modifier)));
    }
    ((GodotObject) this).EmitSignal(NCustomRunModifiersList.SignalName.ModifiersChanged, Array.Empty<Variant>());
  }

  private IEnumerable<ModifierModel> GetAllModifiers()
  {
    foreach (ModifierModel modifierModel in ModelDb.GoodModifiers.Concat<ModifierModel>((IEnumerable<ModifierModel>) ModelDb.BadModifiers))
    {
      if (modifierModel is CharacterCards canonicalCharacterCardsModifier)
      {
        IEnumerator<CharacterModel> enumerator = ModelDb.AllCharacters.GetEnumerator();
        while (enumerator.MoveNext())
        {
          CharacterModel current = enumerator.Current;
          CharacterCards mutable = (CharacterCards) canonicalCharacterCardsModifier.ToMutable();
          mutable.CharacterModel = current.Id;
          yield return (ModifierModel) mutable;
        }
        enumerator = (IEnumerator<CharacterModel>) null;
      }
      else
        yield return modifierModel.ToMutable();
      canonicalCharacterCardsModifier = (CharacterCards) null;
    }
  }

  private void UntickMutuallyExclusiveModifiersForTickbox(NRunModifierTickbox tickbox)
  {
    if (!tickbox.IsTicked)
      return;
    IReadOnlySet<ModifierModel> source = ModelDb.MutuallyExclusiveModifiers.FirstOrDefault<IReadOnlySet<ModifierModel>>((Func<IReadOnlySet<ModifierModel>, bool>) (s => ((IEnumerable<ModifierModel>) s).Any<ModifierModel>((Func<ModifierModel, bool>) (m => m.GetType() == tickbox.Modifier.GetType()))));
    if (source == null)
      return;
    foreach (NRunModifierTickbox modifierTickbox in this._modifierTickboxes)
    {
      NRunModifierTickbox otherTickbox = modifierTickbox;
      if (!(otherTickbox.Modifier.GetType() == tickbox.Modifier.GetType()) && ((IEnumerable<ModifierModel>) source).Any<ModifierModel>((Func<ModifierModel, bool>) (m => m.GetType() == otherTickbox.Modifier.GetType())))
        otherTickbox.IsTicked = false;
    }
  }

  private void AfterModifiersChanged(NRunModifierTickbox tickbox)
  {
    this.UntickMutuallyExclusiveModifiersForTickbox(tickbox);
    ((GodotObject) this).EmitSignal(NCustomRunModifiersList.SignalName.ModifiersChanged, Array.Empty<Variant>());
  }

  public List<ModifierModel> GetModifiersTickedOn()
  {
    return this._modifierTickboxes.Where<NRunModifierTickbox>((Func<NRunModifierTickbox, bool>) (t => t.IsTicked)).Select<NRunModifierTickbox, ModifierModel>((Func<NRunModifierTickbox, ModifierModel>) (t => t.Modifier)).ToList<ModifierModel>();
  }

  public Control? DefaultFocusedControl
  {
    get => (Control) this._modifierTickboxes.FirstOrDefault<NRunModifierTickbox>();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NCustomRunModifiersList.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunModifiersList.MethodName.Initialize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("mode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunModifiersList.MethodName.UntickMutuallyExclusiveModifiersForTickbox, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunModifiersList.MethodName.AfterModifiersChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCustomRunModifiersList.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunModifiersList.MethodName.Initialize) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.Initialize(VariantUtils.ConvertTo<MultiplayerUiMode>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunModifiersList.MethodName.UntickMutuallyExclusiveModifiersForTickbox) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UntickMutuallyExclusiveModifiersForTickbox(VariantUtils.ConvertTo<NRunModifierTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCustomRunModifiersList.MethodName.AfterModifiersChanged) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AfterModifiersChanged(VariantUtils.ConvertTo<NRunModifierTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCustomRunModifiersList.MethodName._Ready) || StringName.op_Equality(ref method, NCustomRunModifiersList.MethodName.Initialize) || StringName.op_Equality(ref method, NCustomRunModifiersList.MethodName.UntickMutuallyExclusiveModifiersForTickbox) || StringName.op_Equality(ref method, NCustomRunModifiersList.MethodName.AfterModifiersChanged) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCustomRunModifiersList.PropertyName._container))
    {
      this._container = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCustomRunModifiersList.PropertyName._mode))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._mode = VariantUtils.ConvertTo<MultiplayerUiMode>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCustomRunModifiersList.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunModifiersList.PropertyName._container))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._container);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCustomRunModifiersList.PropertyName._mode))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<MultiplayerUiMode>(ref this._mode);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCustomRunModifiersList.PropertyName._container, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCustomRunModifiersList.PropertyName._mode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunModifiersList.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCustomRunModifiersList.PropertyName._container, Variant.From<Control>(ref this._container));
    info.AddProperty(NCustomRunModifiersList.PropertyName._mode, Variant.From<MultiplayerUiMode>(ref this._mode));
    info.AddSignalEventDelegate(NCustomRunModifiersList.SignalName.ModifiersChanged, (Delegate) this.backing_ModifiersChanged);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCustomRunModifiersList.PropertyName._container, ref variant1))
      this._container = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NCustomRunModifiersList.PropertyName._mode, ref variant2))
      this._mode = ((Variant) ref variant2).As<MultiplayerUiMode>();
    NCustomRunModifiersList.ModifiersChangedEventHandler changedEventHandler;
    if (!info.TryGetSignalEventDelegate<NCustomRunModifiersList.ModifiersChangedEventHandler>(NCustomRunModifiersList.SignalName.ModifiersChanged, ref changedEventHandler))
      return;
    this.backing_ModifiersChanged = changedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NCustomRunModifiersList.SignalName.ModifiersChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NCustomRunModifiersList.ModifiersChangedEventHandler ModifiersChanged
  {
    add => this.backing_ModifiersChanged += value;
    remove => this.backing_ModifiersChanged -= value;
  }

  protected void EmitSignalModifiersChanged()
  {
    ((GodotObject) this).EmitSignal(NCustomRunModifiersList.SignalName.ModifiersChanged, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NCustomRunModifiersList.SignalName.ModifiersChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCustomRunModifiersList.ModifiersChangedEventHandler modifiersChanged = this.backing_ModifiersChanged;
      if (modifiersChanged == null)
        return;
      modifiersChanged();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NCustomRunModifiersList.SignalName.ModifiersChanged) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void ModifiersChangedEventHandler();

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Initialize = StringName.op_Implicit(nameof (Initialize));
    public static readonly StringName UntickMutuallyExclusiveModifiersForTickbox = StringName.op_Implicit(nameof (UntickMutuallyExclusiveModifiersForTickbox));
    public static readonly StringName AfterModifiersChanged = StringName.op_Implicit(nameof (AfterModifiersChanged));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _container = StringName.op_Implicit(nameof (_container));
    public static readonly StringName _mode = StringName.op_Implicit(nameof (_mode));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName ModifiersChanged = StringName.op_Implicit(nameof (ModifiersChanged));
  }
}
