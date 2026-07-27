// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Migrations.MigrationManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Migrations;

public class MigrationManager
{
  private readonly Dictionary<Type, int> _latestVersions = new Dictionary<Type, int>();
  private readonly Dictionary<Type, int> _minimumSupportedVersions = new Dictionary<Type, int>();
  private readonly MigrationRegistry _registry = new MigrationRegistry();
  private readonly ISaveStore _saveStore;

  public MigrationManager(ISaveStore saveStore)
  {
    this._saveStore = saveStore;
    this.Initialize();
  }

  private void Initialize()
  {
    this._registry.RegisterAllMigrations(this);
    this.DeriveAndSetLatestVersions();
    this.ValidateMigrationPaths();
    this.SetMinimumSupportedVersion<SerializableRun>(13);
    this.SetMinimumSupportedVersion<SerializableProgress>(21);
    this.SetMinimumSupportedVersion<SettingsSave>(4);
    this.SetMinimumSupportedVersion<RunHistory>(8);
    this.SetMinimumSupportedVersion<PrefsSave>(2);
    this.SetMinimumSupportedVersion<ProfileSave>(2);
  }

  private void DeriveAndSetLatestVersions()
  {
    foreach (Type key in this._registry.Migrations.Keys)
    {
      if (this._registry.Migrations[key].Count == 0)
      {
        this._latestVersions[key] = 1;
      }
      else
      {
        int num = this._registry.Migrations[key].Select<IMigration, int>((Func<IMigration, int>) (m => m.ToVersion)).Max();
        this._latestVersions[key] = num;
      }
    }
    Log.Info("Current save versions: " + string.Join("; ", this._latestVersions.Select<KeyValuePair<Type, int>, string>((Func<KeyValuePair<Type, int>, string>) (p => $"{p.Key.Name} v{p.Value}"))));
    this.EnsureVersionSet<SerializableRun>();
    this.EnsureVersionSet<SerializableProgress>();
    this.EnsureVersionSet<SettingsSave>();
    this.EnsureVersionSet<RunHistory>();
    this.EnsureVersionSet<PrefsSave>();
    this.EnsureVersionSet<ProfileSave>();
  }

  private void EnsureVersionSet<T>() where T : ISaveSchema
  {
    this._latestVersions.TryAdd(typeof (T), 1);
  }

  private void ValidateMigrationPaths()
  {
    foreach (Type key in this._registry.Migrations.Keys)
      this.IsMigrationPathValid(key);
  }

  public int GetLatestVersion<T>()
  {
    Type key = typeof (T);
    int latestVersion;
    if (this._latestVersions.TryGetValue(key, out latestVersion))
      return latestVersion;
    Log.Warn($"No version found for {key.Name}, defaulting to 1");
    return 1;
  }

  private List<int> GetGapsInMigrationPath(Type saveType)
  {
    List<int> gapsInMigrationPath = new List<int>();
    List<IMigration> source;
    if (!this._registry.Migrations.TryGetValue(saveType, out source))
      return gapsInMigrationPath;
    HashSet<int> intSet1 = new HashSet<int>(source.Select<IMigration, int>((Func<IMigration, int>) (m => m.FromVersion)));
    HashSet<int> intSet2 = new HashSet<int>(source.Select<IMigration, int>((Func<IMigration, int>) (m => m.ToVersion)));
    int num1;
    bool flag = this._latestVersions.TryGetValue(saveType, out num1);
    foreach (int num2 in intSet2)
    {
      if ((!flag || num2 != num1) && !intSet1.Contains(num2))
        gapsInMigrationPath.Add(num2);
    }
    return gapsInMigrationPath;
  }

  private List<int> GetDuplicateMigrationSources(Type saveType)
  {
    List<IMigration> source;
    return !this._registry.Migrations.TryGetValue(saveType, out source) ? new List<int>() : source.GroupBy<IMigration, int>((Func<IMigration, int>) (m => m.FromVersion)).Where<IGrouping<int, IMigration>>((Func<IGrouping<int, IMigration>, bool>) (g => g.Count<IMigration>() > 1)).Select<IGrouping<int, IMigration>, int>((Func<IGrouping<int, IMigration>, int>) (g => g.Key)).ToList<int>();
  }

  private void IsMigrationPathValid(Type saveType)
  {
    List<int> migrationSources = this.GetDuplicateMigrationSources(saveType);
    if (migrationSources.Count > 0)
      throw new DuplicateMigrationException($"Multiple migrations from version {migrationSources[0]} for {saveType.Name}");
    List<int> gapsInMigrationPath = this.GetGapsInMigrationPath(saveType);
    if (gapsInMigrationPath.Count > 0)
    {
      int num = gapsInMigrationPath[0];
      throw new MigrationPathGapException($"Gap in migration path for {saveType.Name}: version {num} is a target but not a source in any migration");
    }
  }

  public void RegisterMigration(IMigration migration)
  {
    Type saveType = migration.SaveType;
    List<IMigration> migrationList;
    if (!this._registry.Migrations.TryGetValue(saveType, out migrationList))
    {
      migrationList = new List<IMigration>();
      this._registry.Migrations[saveType] = migrationList;
    }
    migrationList.Add(migration);
  }

  private void SetMinimumSupportedVersion<T>(int version) where T : ISaveSchema
  {
    this._minimumSupportedVersions[typeof (T)] = version;
  }

  private int GetCurrentVersion<T>() where T : ISaveSchema
  {
    Type key = typeof (T);
    int currentVersion;
    if (!this._latestVersions.TryGetValue(key, out currentVersion))
      throw new MigrationException("No migrations found for " + key.Name);
    return currentVersion;
  }

  private int GetMinimumSupportedVersion<T>() where T : ISaveSchema
  {
    Type key = typeof (T);
    int supportedVersion;
    if (!this._minimumSupportedVersions.TryGetValue(key, out supportedVersion))
      throw new InvalidOperationException($"No minimum supported version found for {key.Name}. Each save schema type should have a minimum supported version set and a base migration implemented.");
    return supportedVersion;
  }

  private IMigration? GetMigration<T>(int fromVersion, int toVersion) where T : ISaveSchema
  {
    List<IMigration> source;
    return !this._registry.Migrations.TryGetValue(typeof (T), out source) ? (IMigration) null : source.FirstOrDefault<IMigration>((Func<IMigration, bool>) (m => m.FromVersion == fromVersion && m.ToVersion == toVersion));
  }

  private int? GetNextVersion<T>(int currentVersion) where T : ISaveSchema
  {
    List<IMigration> source;
    if (!this._registry.Migrations.TryGetValue(typeof (T), out source))
      return new int?();
    int versionToFind = currentVersion;
    return source.FirstOrDefault<IMigration>((Func<IMigration, bool>) (m => m.FromVersion == versionToFind))?.ToVersion;
  }

  public IEnumerable<Type> GetRegisteredSaveTypes()
  {
    return (IEnumerable<Type>) this._registry.Migrations.Keys;
  }

  public IEnumerable<IMigration> GetMigrationsForType(Type saveType)
  {
    List<IMigration> migrationList;
    return !this._registry.Migrations.TryGetValue(saveType, out migrationList) ? (IEnumerable<IMigration>) Array.Empty<IMigration>() : (IEnumerable<IMigration>) migrationList;
  }

  private static int ExtractSchemaVersion(MigratingData json)
  {
    return json.Has("schema_version") ? json.GetInt("schema_version") : throw new MissingSchemaVersionException($"Schema version not found in JSON: {json}");
  }

  public T CreateNewSave<T>() where T : ISaveSchema, new()
  {
    T newSave = new T();
    ref T local = ref newSave;
    if ((object) default (T) == null)
    {
      T obj = local;
      local = ref obj;
    }
    int latestVersion = this.GetLatestVersion<T>();
    local.SchemaVersion = latestVersion;
    return newSave;
  }

  private void PreserveCorruptFile(string savePath, ReadSaveStatus status)
  {
    try
    {
      if (savePath.EndsWith(".corrupt"))
      {
        Log.Warn($"File '{savePath}' is already marked as corrupt, skipping rename");
      }
      else
      {
        string corruptFilePath = CorruptFileHandler.GenerateCorruptFilePath(savePath, status);
        this._saveStore.RenameFile(savePath, corruptFilePath);
        Log.Error($"Corrupt save detected ({status}): Renamed '{savePath}' to '{corruptFilePath}'");
      }
    }
    catch (Exception ex)
    {
      Log.Warn($"Failed to preserve corrupt save file '{savePath}': {ex.Message}");
    }
  }

  private static bool ShouldPreserveCorrupt(ReadSaveStatus status)
  {
    return status != ReadSaveStatus.FileNotFound && status != ReadSaveStatus.Success && status != ReadSaveStatus.MigrationRequired;
  }

  public ReadSaveResult<T> LoadSave<[DynamicallyAccessedMembers] T>(string filePath) where T : ISaveSchema, new()
  {
    ReadSaveResult<T> readSaveResult1 = this.LoadSaveFromPath<T>(filePath);
    if (readSaveResult1.Success)
      return readSaveResult1;
    string str = filePath + ".backup";
    if (this._saveStore.FileExists(str))
    {
      Log.Warn($"Primary save failed ({readSaveResult1.Status}), attempting .backup fallback: {str}");
      ReadSaveResult<T> readSaveResult2 = this.LoadSaveFromPath<T>(str);
      if (readSaveResult2.Success)
        return readSaveResult2;
    }
    return readSaveResult1;
  }

  private ReadSaveResult<T> LoadSaveFromPath<[DynamicallyAccessedMembers] T>(string filePath) where T : ISaveSchema, new()
  {
    if (!this._saveStore.FileExists(filePath))
      return new ReadSaveResult<T>(ReadSaveStatus.FileNotFound);
    try
    {
      string content = this._saveStore.ReadFile(filePath);
      if (!string.IsNullOrWhiteSpace(content))
        return this.LoadWithAggressiveRecovery<T>(filePath, content);
      Log.Warn("Empty save file found at " + filePath);
      this.PreserveCorruptFile(filePath, ReadSaveStatus.FileEmpty);
      return new ReadSaveResult<T>(ReadSaveStatus.FileEmpty);
    }
    catch (Exception ex)
    {
      Log.Error($"File access error loading {filePath}: {ex.Message}");
      this.PreserveCorruptFile(filePath, ReadSaveStatus.FileAccessError);
      return new ReadSaveResult<T>(ReadSaveStatus.FileAccessError, ex.Message);
    }
  }

  private ReadSaveResult<T> LoadWithAggressiveRecovery<[DynamicallyAccessedMembers] T>(
    string filePath,
    string content)
    where T : ISaveSchema, new()
  {
    try
    {
      using (JsonDocument document = JsonDocument.Parse(content, new JsonDocumentOptions()))
      {
        MigratingData migratingData = new MigratingData(document);
        bool flag = false;
        int num;
        try
        {
          num = MigrationManager.ExtractSchemaVersion(migratingData);
        }
        catch (MissingSchemaVersionException ex)
        {
          Log.Warn($"Missing schema version in {filePath}, attempting to infer...");
          flag = true;
          num = this.InferSchemaVersionFromStructure<T>(migratingData).GetValueOrDefault();
          migratingData.Set<int>("schema_version", num);
        }
        int currentVersion = this.GetCurrentVersion<T>();
        int supportedVersion = this.GetMinimumSupportedVersion<T>();
        if (num > currentVersion)
        {
          Log.Warn($"Save version {num} is newer than current {currentVersion}, attempting recovery...");
          T data = this.RecoverPartialDataFromCorruptSave<T>(migratingData);
          if ((object) data != null)
          {
            Log.Info($"Successfully recovered data from future save version {num}");
            return new ReadSaveResult<T>(data, ReadSaveStatus.RecoveredWithDataLoss, $"Data recovered from future version {num} but newer fields were discarded");
          }
          string errorMessage = $"Save file version {num} is newer than current version {currentVersion}";
          Log.Error($"{errorMessage}: {filePath}");
          this.PreserveCorruptFile(filePath, ReadSaveStatus.FutureVersion);
          return new ReadSaveResult<T>(ReadSaveStatus.FutureVersion, errorMessage);
        }
        if (num < supportedVersion)
        {
          Log.Warn($"Save version {num} is below minimum {supportedVersion}, attempting data scavenging...");
          T data = this.RecoverPartialDataFromCorruptSave<T>(migratingData);
          if ((object) data != null)
          {
            Log.Info($"Successfully scavenged data from old save version {num} (recovery data not persisted)");
            return new ReadSaveResult<T>(data, ReadSaveStatus.RecoveredWithDataLoss, $"Data recovered from version {num} but some information may be lost");
          }
          string errorMessage = $"Save file version {num} is too old and couldn't be scavenged";
          Log.Error($"{errorMessage}: {filePath}");
          this.PreserveCorruptFile(filePath, ReadSaveStatus.VersionTooOld);
          return new ReadSaveResult<T>(ReadSaveStatus.VersionTooOld, errorMessage);
        }
        if (num < currentVersion)
        {
          try
          {
            T data = this.MigrateDataSequentially<T>(migratingData).ToObject<T>();
            Log.Info($"Successfully migrated {typeof (T).Name} from v{num} to v{data.SchemaVersion} (migration not persisted)");
            return new ReadSaveResult<T>(data, ReadSaveStatus.MigrationRequired, $"Save was migrated from version {num} to {data.SchemaVersion}");
          }
          catch (Exception ex)
          {
            Log.Error($"Migration failed for {filePath} with exception: {ex}");
            T data = this.RecoverPartialDataFromCorruptSave<T>(migratingData);
            if ((object) data != null)
            {
              Log.Info("Migration failed but data scavenging succeeded");
              return new ReadSaveResult<T>(data, ReadSaveStatus.RecoveredWithDataLoss, $"Migration failed, recovered partial data from version {num}");
            }
            this.PreserveCorruptFile(filePath, ReadSaveStatus.MigrationFailed);
            return new ReadSaveResult<T>(ReadSaveStatus.MigrationFailed, ex.Message);
          }
        }
        else
        {
          ReadSaveResult<T> readSaveResult = JsonSerializationUtility.FromJson<T>(content);
          if (readSaveResult.Success && (object) readSaveResult.SaveData != null)
            return readSaveResult;
          Log.Error($"Failed to deserialize {filePath}: {readSaveResult.ErrorMessage}");
          T data = this.RecoverPartialDataFromCorruptSave<T>(migratingData);
          if ((object) data != null)
          {
            Log.Info("Deserialization failed but data scavenging succeeded");
            return new ReadSaveResult<T>(data, ReadSaveStatus.RecoveredWithDataLoss, "Save file was corrupt but partial data was recovered");
          }
          if (flag)
          {
            this.PreserveCorruptFile(filePath, ReadSaveStatus.MissingSchemaVersion);
            return new ReadSaveResult<T>(ReadSaveStatus.MissingSchemaVersion, "Save file is missing schema version and cannot be deserialized");
          }
          if (MigrationManager.ShouldPreserveCorrupt(readSaveResult.Status))
            this.PreserveCorruptFile(filePath, readSaveResult.Status);
          return new ReadSaveResult<T>(readSaveResult.Status, readSaveResult.ErrorMessage);
        }
      }
    }
    catch (JsonException ex)
    {
      string str = ex.Path ?? "unknown";
      Log.Error($"JSON parse error in {filePath} at path={str}, line={ex.LineNumber}: {((Exception) ex).Message}");
      string content1 = this.RepairCommonJsonErrors(content);
      if (content1 != null)
      {
        Log.Info("JSON repair succeeded, retrying load...");
        ReadSaveResult<T> readSaveResult = this.LoadWithAggressiveRecovery<T>(filePath, content1);
        if (!readSaveResult.Success)
          return readSaveResult;
        this._saveStore.WriteFile(filePath + ".pre-repair", content);
        return new ReadSaveResult<T>(readSaveResult.SaveData, ReadSaveStatus.JsonRepaired, "Save file had JSON errors that were automatically repaired");
      }
      this.PreserveCorruptFile(filePath, ReadSaveStatus.JsonParseError);
      return new ReadSaveResult<T>(ReadSaveStatus.JsonParseError, $"JSON error at {str} (line {ex.LineNumber}): {((Exception) ex).Message}");
    }
    catch (Exception ex)
    {
      Log.Error($"Unexpected error loading {filePath}: {ex.Message}");
      this.PreserveCorruptFile(filePath, ReadSaveStatus.Unrecoverable);
      return new ReadSaveResult<T>(ReadSaveStatus.Unrecoverable, ex.Message);
    }
  }

  private int? InferSchemaVersionFromStructure<T>(MigratingData data) where T : ISaveSchema
  {
    return new int?();
  }

  private T? RecoverPartialDataFromCorruptSave<T>(MigratingData data) where T : ISaveSchema, new()
  {
    if (typeof (T) == typeof (SerializableRun))
      return default (T);
    int currentVersion = this.GetCurrentVersion<T>();
    try
    {
      data.Set<int>("schema_version", currentVersion);
      T obj = data.ToObject<T>();
      obj.SchemaVersion = currentVersion;
      Log.Info("Data scavenging succeeded for " + typeof (T).Name);
      return obj;
    }
    catch (Exception ex)
    {
      Log.Warn($"Data scavenging failed for {typeof (T).Name}: {ex.Message}");
      return default (T);
    }
  }

  private string? RepairCommonJsonErrors(string json)
  {
    if (string.IsNullOrWhiteSpace(json))
      return (string) null;
    try
    {
      string source = Regex.Replace(json, ",\\s*([}\\]])", "$1");
      if (!source.EndsWith("}") && !source.EndsWith("]"))
      {
        int num1 = source.Count<char>((Func<char, bool>) (c => c == '{')) - source.Count<char>((Func<char, bool>) (c => c == '}'));
        int num2 = source.Count<char>((Func<char, bool>) (c => c == '[')) - source.Count<char>((Func<char, bool>) (c => c == ']'));
        if (num1 == 1 && num2 == 0)
          source += "}";
        else if (num2 == 1 && num1 == 0)
          source += "]";
      }
      using (JsonDocument.Parse(source, new JsonDocumentOptions()))
        return source;
    }
    catch
    {
      return (string) null;
    }
  }

  private MigratingData MigrateDataSequentially<T>(MigratingData jsonObj) where T : ISaveSchema
  {
    int num = MigrationManager.ExtractSchemaVersion(jsonObj);
    int currentVersion = this.GetCurrentVersion<T>();
    while (num < currentVersion)
    {
      int toVersion = (this.GetNextVersion<T>(num) ?? throw new MigrationException($"Missing migration path for {typeof (T).Name} from v{num} to latest version")).Value;
      IMigration migration = this.GetMigration<T>(num, toVersion);
      if (migration == null)
        throw new MigrationException($"Missing migration implementation for {typeof (T).Name} from v{num} to v{toVersion}");
      Log.Info($"Migrating {typeof (T).Name} from v{num} to v{toVersion}");
      try
      {
        jsonObj = migration.Migrate(jsonObj);
        num = toVersion;
      }
      catch (Exception ex) when (!(ex is MigrationException))
      {
        throw new MigrationException($"Error migrating {typeof (T).Name} from v{num} to v{toVersion}: {ex.Message}", ex);
      }
    }
    return jsonObj;
  }
}
