// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.PlatformBranchExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable enable
namespace MegaCrit.Sts2.Core.Platform;

public static class PlatformBranchExtensions
{
  public static string ToName(this PlatformBranch branch)
  {
    string name;
    switch (branch)
    {
      case PlatformBranch.None:
        name = "none";
        break;
      case PlatformBranch.Production:
        name = "public";
        break;
      case PlatformBranch.PublicBeta:
        name = "public-beta";
        break;
      case PlatformBranch.PrivateBeta:
        name = "private-beta";
        break;
      case PlatformBranch.DevTest:
        name = "dev-test";
        break;
      default:
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) branch);
        break;
    }
    return name;
  }

  public static PlatformBranch? FromName(string name)
  {
    switch (name)
    {
      case "dev-test":
        return new PlatformBranch?(PlatformBranch.DevTest);
      case "private-beta":
        return new PlatformBranch?(PlatformBranch.PrivateBeta);
      case "public-beta":
        return new PlatformBranch?(PlatformBranch.PublicBeta);
      case "public":
        return new PlatformBranch?(PlatformBranch.Production);
      default:
        return new PlatformBranch?();
    }
  }
}
