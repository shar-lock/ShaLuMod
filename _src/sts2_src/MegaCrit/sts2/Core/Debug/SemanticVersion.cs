// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.SemanticVersion
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Debug;

public class SemanticVersion : IComparable<SemanticVersion>, IEquatable<SemanticVersion>
{
  public int Major { get; }

  public int Minor { get; }

  public int Patch { get; }

  public List<string>? Prerelease { get; }

  public string? Metadata { get; }

  public SemanticVersion(
    int major,
    int minor,
    int patch,
    string? metadata = null,
    List<string>? prerelease = null)
  {
    this.Major = major;
    this.Minor = minor;
    this.Patch = patch;
    this.Metadata = metadata;
    this.Prerelease = prerelease;
  }

  public static bool TryFromString(string str, out SemanticVersion? version)
  {
    try
    {
      version = SemanticVersion.FromString(str);
      return true;
    }
    catch (Exception ex) when (
    {
      // ISSUE: unable to correctly present filter
      bool flag;
      switch (ex)
      {
        case InvalidOperationException _:
        case FormatException _:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (flag)
      {
        SuccessfulFiltering;
      }
      else
        throw;
    }
    )
    {
      version = (SemanticVersion) null;
      return false;
    }
  }

  public static SemanticVersion FromString(string version)
  {
    int major = 0;
    int minor = 0;
    int patch = 0;
    List<string> prerelease = (List<string>) null;
    string metadata = (string) null;
    int num1 = 0;
    SemanticVersion.ParseState parseState = SemanticVersion.ParseState.None;
    for (int index = 0; index < version.Length; ++index)
    {
      if (index == 0 && version[index] == 'v')
        ++num1;
      else if (version[index] == '.')
      {
        switch (parseState)
        {
          case SemanticVersion.ParseState.Major:
            string str1 = version;
            int num2 = num1;
            int startIndex1 = num2;
            int length1 = index - num2;
            major = int.Parse(str1.Substring(startIndex1, length1));
            parseState = SemanticVersion.ParseState.Minor;
            break;
          case SemanticVersion.ParseState.Minor:
            string str2 = version;
            int num3 = num1;
            int startIndex2 = num3;
            int length2 = index - num3;
            minor = int.Parse(str2.Substring(startIndex2, length2));
            parseState = SemanticVersion.ParseState.Patch;
            break;
          case SemanticVersion.ParseState.Patch:
            throw new InvalidOperationException($"Version {version} has a . in an invalid place! Parse state is {parseState}, index is {index}");
          case SemanticVersion.ParseState.Prerelease:
            List<string> stringList = prerelease;
            string str3 = version;
            int num4 = num1;
            int startIndex3 = num4;
            int length3 = index - num4;
            string str4 = str3.Substring(startIndex3, length3);
            stringList.Add(str4);
            break;
        }
        num1 = index + 1;
      }
      else if (version[index] == '-')
      {
        if (parseState != SemanticVersion.ParseState.Patch)
          throw new InvalidOperationException($"Version {version} has a - in an invalid place! Parse state is {parseState}, index is {index}");
        string str = version;
        int num5 = num1;
        int startIndex = num5;
        int length = index - num5;
        patch = int.Parse(str.Substring(startIndex, length));
        parseState = SemanticVersion.ParseState.Prerelease;
        prerelease = new List<string>();
        num1 = index + 1;
      }
      else
      {
        if (version[index] == '+')
        {
          switch (parseState)
          {
            case SemanticVersion.ParseState.Patch:
              string str5 = version;
              int num6 = num1;
              int startIndex4 = num6;
              int length4 = index - num6;
              patch = int.Parse(str5.Substring(startIndex4, length4));
              break;
            case SemanticVersion.ParseState.Prerelease:
              List<string> stringList = prerelease;
              string str6 = version;
              int num7 = num1;
              int startIndex5 = num7;
              int length5 = index - num7;
              string str7 = str6.Substring(startIndex5, length5);
              stringList.Add(str7);
              break;
            default:
              throw new InvalidOperationException($"Version {version} has a + in an invalid place! Parse state is {parseState}, index is {index}");
          }
          string str8 = version;
          int startIndex6 = index + 1;
          metadata = str8.Substring(startIndex6, str8.Length - startIndex6);
          parseState = SemanticVersion.ParseState.Metadata;
          break;
        }
        if (parseState == SemanticVersion.ParseState.None)
          parseState = SemanticVersion.ParseState.Major;
      }
    }
    switch (parseState)
    {
      case SemanticVersion.ParseState.Patch:
        string str9 = version;
        int startIndex7 = num1;
        patch = int.Parse(str9.Substring(startIndex7, str9.Length - startIndex7));
        goto case SemanticVersion.ParseState.Metadata;
      case SemanticVersion.ParseState.Prerelease:
        List<string> stringList1 = prerelease;
        string str10 = version;
        int startIndex8 = num1;
        string str11 = str10.Substring(startIndex8, str10.Length - startIndex8);
        stringList1.Add(str11);
        goto case SemanticVersion.ParseState.Metadata;
      case SemanticVersion.ParseState.Metadata:
        return new SemanticVersion(major, minor, patch, metadata, prerelease);
      default:
        throw new InvalidOperationException($"Version terminated in an invalid place! Parse state is {parseState}");
    }
  }

  public int CompareTo(SemanticVersion? other)
  {
    if (this == other)
      return 0;
    if (other == null)
      return 1;
    int num1 = this.Major.CompareTo(other.Major);
    if (num1 != 0)
      return num1;
    int num2 = this.Minor.CompareTo(other.Minor);
    if (num2 != 0)
      return num2;
    int num3 = this.Patch.CompareTo(other.Patch);
    if (num3 != 0)
      return num3;
    List<string> prerelease1 = this.Prerelease;
    // ISSUE: explicit non-virtual call
    int count1 = prerelease1 != null ? __nonvirtual (prerelease1.Count) : 0;
    List<string> prerelease2 = other.Prerelease;
    // ISSUE: explicit non-virtual call
    int count2 = prerelease2 != null ? __nonvirtual (prerelease2.Count) : 0;
    if (count1 == 0 && count2 == 0)
      return 0;
    if (count1 > 0 && count2 == 0)
      return -1;
    if (count1 == 0 && count2 > 0)
      return 1;
    for (int index = 0; index < Math.Min(count1, count2); ++index)
    {
      if (!(this.Prerelease[index] == other.Prerelease[index]))
      {
        int result1;
        bool flag1 = int.TryParse(this.Prerelease[index], out result1);
        int result2;
        bool flag2 = int.TryParse(other.Prerelease[index], out result2);
        if (!flag1 && !flag2)
          return string.Compare(this.Prerelease[index], other.Prerelease[index], StringComparison.Ordinal);
        if (flag1 && !flag2)
          return -1;
        if (!flag1 & flag2)
          return 1;
        if (flag1 & flag2)
          return result1.CompareTo(result2);
      }
    }
    return count1.CompareTo(count2);
  }

  public bool Equals(SemanticVersion? other) => this.CompareTo(other) == 0;

  public override string ToString()
  {
    string str = $"v{this.Major}.{this.Minor}.{this.Patch}";
    if (this.Prerelease != null)
      str = $"{str}-{string.Join(".", (IEnumerable<string>) this.Prerelease)}";
    if (this.Metadata != null)
      str = $"{str}+{this.Metadata}";
    return str;
  }

  private enum ParseState
  {
    None,
    Major,
    Minor,
    Patch,
    Prerelease,
    Metadata,
  }
}
