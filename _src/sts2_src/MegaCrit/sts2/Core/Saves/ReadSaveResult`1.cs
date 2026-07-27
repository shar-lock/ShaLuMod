// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.ReadSaveResult`1
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class ReadSaveResult<T> where T : ISaveSchema
{
  public T? SaveData { get; }

  public ReadSaveStatus Status { get; }

  public bool Success
  {
    get
    {
      return this.Status == ReadSaveStatus.Success || this.Status == ReadSaveStatus.MigrationRequired || this.Status == ReadSaveStatus.JsonRepaired || this.Status == ReadSaveStatus.RecoveredWithDataLoss;
    }
  }

  public string? ErrorMessage { get; }

  public ReadSaveResult(T data)
  {
    this.SaveData = data;
    this.Status = ReadSaveStatus.Success;
  }

  public ReadSaveResult(ReadSaveStatus status, string? errorMessage = null)
  {
    this.Status = status;
    this.ErrorMessage = errorMessage;
  }

  public ReadSaveResult(T data, ReadSaveStatus status, string? errorMessage = null)
  {
    this.SaveData = data;
    this.Status = status;
    this.ErrorMessage = errorMessage;
  }
}
