// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.TimelineInfoDumper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Timeline;

[ScriptPath("res://src/Core/Timeline/TimelineInfoDumper.cs")]
public class TimelineInfoDumper : Node
{
  public static void Dump()
  {
    List<EpochModel> allEpochs = TimelineInfoDumper.GetAllEpochs();
    Console.Out.WriteLine("START TIMELINE INFO DUMPER");
    Console.Out.WriteLine("START TIMELINE INFO DUMPER");
    Console.Out.WriteLine("START TIMELINE INFO DUMPER");
    foreach (EpochModel epochModel in allEpochs)
      Console.Out.WriteLine($"\"{epochModel.Id}\", \"{epochModel.Era}\", \"{(int) epochModel.Era}.{epochModel.EraPosition}\", \"{epochModel.Title}\", \"{epochModel.Description.Replace("\r", "").Replace("\n", "")}\", \"{epochModel.UnlockText}\", \"{epochModel.ResolvedPortraitPath}\"");
    Console.Out.WriteLine("END TIMELINE INFO DUMPER");
    Console.Out.WriteLine("END TIMELINE INFO DUMPER");
    Console.Out.WriteLine("END TIMELINE INFO DUMPER");
  }

  public static List<EpochModel> GetAllEpochs()
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return EpochModel.AllEpochIds.Select<string, EpochModel>(TimelineInfoDumper.\u003C\u003EO.\u003C0\u003E__Get ?? (TimelineInfoDumper.\u003C\u003EO.\u003C0\u003E__Get = new Func<string, EpochModel>(EpochModel.Get))).ToList<EpochModel>();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(TimelineInfoDumper.MethodName.Dump, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, TimelineInfoDumper.MethodName.Dump) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    TimelineInfoDumper.Dump();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, TimelineInfoDumper.MethodName.Dump) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      TimelineInfoDumper.Dump();
      ret = new godot_variant();
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, TimelineInfoDumper.MethodName.Dump) || base.HasGodotClassMethod(ref method);
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
    public static readonly StringName Dump = StringName.op_Implicit(nameof (Dump));
  }

  public class PropertyName : Node.PropertyName
  {
  }

  public class SignalName : Node.SignalName
  {
  }
}
