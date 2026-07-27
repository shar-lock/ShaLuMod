// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.NAssetLoader
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes;

[ScriptPath("res://src/Core/Nodes/NAssetLoader.cs")]
public class NAssetLoader : Node
{
  private static NAssetLoader? _instance;
  private readonly ConcurrentQueue<AssetLoadingSession?> _sessions = new ConcurrentQueue<AssetLoadingSession>();
  private AssetLoadingSession? _currentSession;

  public static NAssetLoader Instance => NAssetLoader._instance ?? new NAssetLoader();

  public override void _Ready()
  {
    if (NAssetLoader._instance != null)
      return;
    NAssetLoader._instance = this;
  }

  public Task<bool> LoadInTheBackground(AssetLoadingSession session)
  {
    this._sessions.Enqueue(session);
    this.SetProcess(true);
    return session.Task;
  }

  public override void _Process(double delta)
  {
    if (this._currentSession == null || this._currentSession.IsCompleted)
    {
      AssetLoadingSession result;
      if (this._sessions.TryDequeue(out result))
        this._currentSession = result;
      else
        this.SetProcess(false);
    }
    else
      this._currentSession.Process();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NAssetLoader.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAssetLoader.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAssetLoader.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAssetLoader.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    base._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAssetLoader.MethodName._Ready) || StringName.op_Equality(ref method, NAssetLoader.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Node.PropertyName
  {
  }

  public class SignalName : Node.SignalName
  {
  }
}
