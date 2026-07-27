// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.CmdResult
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole;

public struct CmdResult
{
  public readonly bool success;
  public readonly string msg;
  public readonly Task? task;

  public CmdResult(bool success, string? msg = null)
  {
    this.success = success;
    this.msg = msg ?? string.Empty;
    this.task = (Task) null;
  }

  public CmdResult(Task task, bool success, string? msg = null)
  {
    this.task = task;
    this.success = success;
    this.msg = msg ?? string.Empty;
  }
}
