// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Orbs.NOrbManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Orbs;

[ScriptPath("res://src/Core/Nodes/Orbs/NOrbManager.cs")]
public class NOrbManager : Control
{
  private Control _orbContainer;
  private readonly List<NOrb> _orbs = new List<NOrb>();
  private NCreature _creatureNode;
  private const float _minRadius = 225f;
  private const float _maxRadius = 300f;
  private const float _range = 150f;
  private const float _angleOffset = -25f;
  private const float _tweenSpeed = 0.45f;
  private Tween? _curTween;

  private static string ScenePath => SceneHelper.GetScenePath("/orbs/orb_manager");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NOrbManager.ScenePath);
    }
  }

  public bool IsLocal { get; private set; }

  private Player Player => this._creatureNode.Entity.Player;

  public override void _Ready()
  {
    this._orbContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Orbs"));
  }

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    CombatManager.Instance.StateTracker.CombatStateChanged += new Action<CombatState>(this.OnCombatStateChanged);
    CombatManager.Instance.CombatSetUp += new Action<CombatState>(this.OnCombatSetup);
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    CombatManager.Instance.StateTracker.CombatStateChanged -= new Action<CombatState>(this.OnCombatStateChanged);
    CombatManager.Instance.CombatSetUp -= new Action<CombatState>(this.OnCombatSetup);
  }

  public static NOrbManager Create(NCreature creature, bool isLocal)
  {
    if (creature.Entity.Player == null)
      throw new InvalidOperationException("NOrbManager can only be applied to player creatures");
    NOrbManager norbManager = PreloadManager.Cache.GetScene(NOrbManager.ScenePath).Instantiate<NOrbManager>((PackedScene.GenEditState) 0L);
    norbManager._creatureNode = creature;
    norbManager.IsLocal = isLocal;
    return norbManager;
  }

  private void OnCombatSetup(CombatState _)
  {
    if (!this.Player.Creature.IsAlive || this.Player.PlayerCombatState == null)
      return;
    this.AddSlotAnim(this.Player.PlayerCombatState.OrbQueue.Capacity);
  }

  public void RemoveSlotAnim(int amount)
  {
    if (amount > this._orbs.Count)
      throw new InvalidOperationException("There are not enough slots to remove.");
    for (int index = 0; index < amount; ++index)
    {
      NOrb norb = this._orbs.Last<NOrb>();
      ((Node) norb).QueueFreeSafely();
      this._orbs.Remove(norb);
      if (norb.HasFocus())
        this._creatureNode.Hitbox.TryGrabFocus();
    }
    this.TweenLayout();
    this.UpdateControllerNavigation();
  }

  public void AddSlotAnim(int amount)
  {
    for (int index = 0; index < amount; ++index)
    {
      NOrb child = NOrb.Create(LocalContext.IsMe(this.Player));
      ((Node) this._orbContainer).AddChildSafely((Node) child);
      this._orbs.Add(child);
      child.Position = Vector2.Zero;
    }
    this.TweenLayout();
    this.UpdateControllerNavigation();
  }

  public void ReplaceOrb(OrbModel oldOrb, OrbModel newOrb)
  {
    for (int index = 0; index < this._orbs.Count; ++index)
    {
      if (this._orbs[index].Model == oldOrb)
        this._orbs[index].ReplaceOrb(newOrb);
    }
    this.UpdateControllerNavigation();
  }

  public void AddOrbAnim()
  {
    OrbModel model = this.Player.PlayerCombatState.OrbQueue.Orbs.LastOrDefault<OrbModel>();
    NOrb norb = this._orbs.FirstOrDefault<NOrb>((Func<NOrb, bool>) (node => node.Model == null));
    if (norb == null)
    {
      this.EvokeOrbAnim(this._orbs.First<NOrb>((Func<NOrb, bool>) (node => node.Model != null)).Model);
      norb = (NOrb) ((IEnumerable<Node>) ((Node) this._orbContainer).GetChildren(false)).First<Node>((Func<Node, bool>) (node => ((NOrb) node).Model == null));
    }
    NOrb child = NOrb.Create(LocalContext.IsMe(this.Player), model);
    ((Node) norb).AddSiblingSafely((Node) child);
    this._orbs.Insert(this._orbs.IndexOf(norb), child);
    child.Position = norb.Position;
    ((Node) this._orbContainer).RemoveChildSafely((Node) norb);
    this._orbs.Remove(norb);
    ((Node) norb).QueueFreeSafely();
    this.TweenLayout();
    this.UpdateControllerNavigation();
  }

  public void EvokeOrbAnim(OrbModel orb)
  {
    NOrb norb = this._orbs.Last<NOrb>((Func<NOrb, bool>) (node => node.Model == orb));
    Tween tween = ((Node) this).CreateTween();
    this._orbs.Remove(norb);
    tween.TweenProperty((GodotObject) norb, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0), 0.25);
    tween.Chain().TweenCallback(Callable.From(new Action(((GodotTreeExtensions) norb).QueueFreeSafely)));
    NOrb child = NOrb.Create(LocalContext.IsMe(this.Player));
    ((Node) this._orbContainer).AddChildSafely((Node) child);
    this._orbs.Add(child);
    child.Position = Vector2.Zero;
    if (norb.HasFocus())
      this._creatureNode.Hitbox.TryGrabFocus();
    this.TweenLayout();
    this.UpdateControllerNavigation();
  }

  private void UpdateControllerNavigation()
  {
    for (int index = 0; index < this._orbs.Count; ++index)
    {
      NOrb orb = this._orbs[index];
      NodePath path;
      if (index <= 0)
      {
        List<NOrb> orbs = this._orbs;
        path = ((Node) orbs[orbs.Count - 1]).GetPath();
      }
      else
        path = ((Node) this._orbs[index - 1]).GetPath();
      orb.FocusNeighborRight = path;
      this._orbs[index].FocusNeighborLeft = index < this._orbs.Count - 1 ? ((Node) this._orbs[index + 1]).GetPath() : ((Node) this._orbs[0]).GetPath();
      this._orbs[index].FocusNeighborTop = ((Node) this._orbs[index]).GetPath();
      this._orbs[index].FocusNeighborBottom = ((Node) this._creatureNode.Hitbox).GetPath();
    }
    this._creatureNode.UpdateNavigation();
  }

  private void TweenLayout()
  {
    int capacity = this.Player.PlayerCombatState.OrbQueue.Capacity;
    if (capacity == 0)
      return;
    float num1 = 125f;
    float num2 = num1 / (float) (capacity - 1);
    float num3 = Mathf.Lerp(225f, 300f, (float) (((double) capacity - 3.0) / 7.0));
    if (!this.IsLocal)
      num3 *= 0.75f;
    this._curTween?.Kill();
    this._curTween = ((Node) this).CreateTween().SetParallel(true);
    for (int index = 0; index < capacity; ++index)
    {
      float radians = float.DegreesToRadians(-25f - num1);
      Vector2 vector2 = Vector2.op_Multiply(new Vector2(-Mathf.Cos(radians), Mathf.Sin(radians)), num3);
      this._curTween.TweenProperty((GodotObject) this._orbs[index], NodePath.op_Implicit("position"), Variant.op_Implicit(vector2), 0.44999998807907104).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 1L);
      num1 -= num2;
    }
  }

  private void OnCombatStateChanged(CombatState _) => this.UpdateVisuals(OrbEvokeType.None);

  public void UpdateVisuals(OrbEvokeType evokeType)
  {
    foreach (NOrb orb in this._orbs)
      orb.UpdateVisuals(false);
    switch (evokeType)
    {
      case OrbEvokeType.Front:
        this._orbs.FirstOrDefault<NOrb>()?.UpdateVisuals(true);
        break;
      case OrbEvokeType.All:
        using (List<NOrb>.Enumerator enumerator = this._orbs.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.UpdateVisuals(true);
          break;
        }
    }
  }

  public void ClearOrbs()
  {
    this._curTween?.Kill();
    if (this._orbs.Count == 0)
      return;
    this._curTween = ((Node) this).CreateTween();
    foreach (NOrb orb in this._orbs)
    {
      this._curTween.Parallel().TweenProperty((GodotObject) orb, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.Zero), 1.0).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 1L);
      this._curTween.Parallel().TweenProperty((GodotObject) orb, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0), 0.25);
    }
    foreach (NOrb orb in this._orbs)
      this._curTween.Chain().TweenCallback(Callable.From(new Action(((GodotTreeExtensions) orb).QueueFreeSafely)));
    this._orbs.Clear();
  }

  public Control DefaultFocusOwner
  {
    get => this._orbs.Count <= 0 ? this._creatureNode.Hitbox : (Control) this._orbs.First<NOrb>();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NOrbManager.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrbManager.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrbManager.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrbManager.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("creature"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isLocal"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NOrbManager.MethodName.RemoveSlotAnim, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("amount"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NOrbManager.MethodName.AddSlotAnim, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("amount"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NOrbManager.MethodName.AddOrbAnim, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrbManager.MethodName.UpdateControllerNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrbManager.MethodName.TweenLayout, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrbManager.MethodName.UpdateVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("evokeType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NOrbManager.MethodName.ClearOrbs, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NOrbManager.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbManager.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbManager.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbManager.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NOrbManager norbManager = NOrbManager.Create(VariantUtils.ConvertTo<NCreature>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NOrbManager>(ref norbManager);
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbManager.MethodName.RemoveSlotAnim) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RemoveSlotAnim(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbManager.MethodName.AddSlotAnim) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AddSlotAnim(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbManager.MethodName.AddOrbAnim) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AddOrbAnim();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbManager.MethodName.UpdateControllerNavigation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateControllerNavigation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbManager.MethodName.TweenLayout) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TweenLayout();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbManager.MethodName.UpdateVisuals) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateVisuals(VariantUtils.ConvertTo<OrbEvokeType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NOrbManager.MethodName.ClearOrbs) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ClearOrbs();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NOrbManager.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NOrbManager norbManager = NOrbManager.Create(VariantUtils.ConvertTo<NCreature>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NOrbManager>(ref norbManager);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NOrbManager.MethodName._Ready) || StringName.op_Equality(ref method, NOrbManager.MethodName._EnterTree) || StringName.op_Equality(ref method, NOrbManager.MethodName._ExitTree) || StringName.op_Equality(ref method, NOrbManager.MethodName.Create) || StringName.op_Equality(ref method, NOrbManager.MethodName.RemoveSlotAnim) || StringName.op_Equality(ref method, NOrbManager.MethodName.AddSlotAnim) || StringName.op_Equality(ref method, NOrbManager.MethodName.AddOrbAnim) || StringName.op_Equality(ref method, NOrbManager.MethodName.UpdateControllerNavigation) || StringName.op_Equality(ref method, NOrbManager.MethodName.TweenLayout) || StringName.op_Equality(ref method, NOrbManager.MethodName.UpdateVisuals) || StringName.op_Equality(ref method, NOrbManager.MethodName.ClearOrbs) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NOrbManager.PropertyName.IsLocal))
    {
      this.IsLocal = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbManager.PropertyName._orbContainer))
    {
      this._orbContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbManager.PropertyName._creatureNode))
    {
      this._creatureNode = VariantUtils.ConvertTo<NCreature>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NOrbManager.PropertyName._curTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._curTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NOrbManager.PropertyName.IsLocal))
    {
      ref godot_variant local = ref value;
      bool isLocal = this.IsLocal;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isLocal);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbManager.PropertyName.DefaultFocusOwner))
    {
      ref godot_variant local = ref value;
      Control defaultFocusOwner = this.DefaultFocusOwner;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusOwner);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbManager.PropertyName._orbContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._orbContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbManager.PropertyName._creatureNode))
    {
      value = VariantUtils.CreateFrom<NCreature>(ref this._creatureNode);
      return true;
    }
    if (!StringName.op_Equality(ref name, NOrbManager.PropertyName._curTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._curTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NOrbManager.PropertyName.IsLocal, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrbManager.PropertyName._orbContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrbManager.PropertyName._creatureNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrbManager.PropertyName._curTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrbManager.PropertyName.DefaultFocusOwner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isLocal1 = NOrbManager.PropertyName.IsLocal;
    bool isLocal2 = this.IsLocal;
    Variant variant = Variant.From<bool>(ref isLocal2);
    serializationInfo.AddProperty(isLocal1, variant);
    info.AddProperty(NOrbManager.PropertyName._orbContainer, Variant.From<Control>(ref this._orbContainer));
    info.AddProperty(NOrbManager.PropertyName._creatureNode, Variant.From<NCreature>(ref this._creatureNode));
    info.AddProperty(NOrbManager.PropertyName._curTween, Variant.From<Tween>(ref this._curTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NOrbManager.PropertyName.IsLocal, ref variant1))
      this.IsLocal = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NOrbManager.PropertyName._orbContainer, ref variant2))
      this._orbContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NOrbManager.PropertyName._creatureNode, ref variant3))
      this._creatureNode = ((Variant) ref variant3).As<NCreature>();
    Variant variant4;
    if (!info.TryGetProperty(NOrbManager.PropertyName._curTween, ref variant4))
      return;
    this._curTween = ((Variant) ref variant4).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName RemoveSlotAnim = StringName.op_Implicit(nameof (RemoveSlotAnim));
    public static readonly StringName AddSlotAnim = StringName.op_Implicit(nameof (AddSlotAnim));
    public static readonly StringName AddOrbAnim = StringName.op_Implicit(nameof (AddOrbAnim));
    public static readonly StringName UpdateControllerNavigation = StringName.op_Implicit(nameof (UpdateControllerNavigation));
    public static readonly StringName TweenLayout = StringName.op_Implicit(nameof (TweenLayout));
    public static readonly StringName UpdateVisuals = StringName.op_Implicit(nameof (UpdateVisuals));
    public static readonly StringName ClearOrbs = StringName.op_Implicit(nameof (ClearOrbs));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName IsLocal = StringName.op_Implicit(nameof (IsLocal));
    public static readonly StringName DefaultFocusOwner = StringName.op_Implicit(nameof (DefaultFocusOwner));
    public static readonly StringName _orbContainer = StringName.op_Implicit(nameof (_orbContainer));
    public static readonly StringName _creatureNode = StringName.op_Implicit(nameof (_creatureNode));
    public static readonly StringName _curTween = StringName.op_Implicit(nameof (_curTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
