// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Timeline.StoryModelSubtypes
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Timeline.Stories;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace MegaCrit.Sts2.Core.Timeline;

public static class StoryModelSubtypes
{
  [DynamicallyAccessedMembers]
  private static readonly Type _t0 = typeof (DefectStory);
  [DynamicallyAccessedMembers]
  private static readonly Type _t1 = typeof (IroncladStory);
  [DynamicallyAccessedMembers]
  private static readonly Type _t2 = typeof (MagnumOpusStory);
  [DynamicallyAccessedMembers]
  private static readonly Type _t3 = typeof (NecrobinderStory);
  [DynamicallyAccessedMembers]
  private static readonly Type _t4 = typeof (RegentStory);
  [DynamicallyAccessedMembers]
  private static readonly Type _t5 = typeof (SilentStory);
  [DynamicallyAccessedMembers]
  private static readonly Type _t6 = typeof (TalesFromTheSpireStory);
  [DynamicallyAccessedMembers]
  private static readonly Type _t7 = typeof (TheReopeningStory);
  private static readonly Type[] _subtypes = new Type[8]
  {
    StoryModelSubtypes._t0,
    StoryModelSubtypes._t1,
    StoryModelSubtypes._t2,
    StoryModelSubtypes._t3,
    StoryModelSubtypes._t4,
    StoryModelSubtypes._t5,
    StoryModelSubtypes._t6,
    StoryModelSubtypes._t7
  };

  public static int Count => 8;

  public static IReadOnlyList<Type> All => (IReadOnlyList<Type>) StoryModelSubtypes._subtypes;

  [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2063", Justification = "The list only contains types stored with the correct DynamicallyAccessedMembers attribute, enforced by source generation.")]
  [return: DynamicallyAccessedMembers]
  public static Type Get(int i) => StoryModelSubtypes._subtypes[i];
}
