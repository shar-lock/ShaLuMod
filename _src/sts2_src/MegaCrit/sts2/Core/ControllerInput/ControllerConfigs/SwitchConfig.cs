// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs.SwitchConfig
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Helpers;

#nullable enable
namespace MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs;

public class SwitchConfig : ControllerConfig
{
  protected override string FolderPath => "atlases/controller_atlas.sprites/switch";

  public override ControllerMappingType ControllerMappingType
  {
    get => ControllerMappingType.NintendoSwitch;
  }

  protected override string FaceButtonSouthGlyph
  {
    get => ImageHelper.GetImagePath(this.FolderPath + "/b.tres");
  }

  protected override string FaceButtonEastGlyph
  {
    get => ImageHelper.GetImagePath(this.FolderPath + "/a.tres");
  }

  protected override string FaceButtonNorthGlyph
  {
    get => ImageHelper.GetImagePath(this.FolderPath + "/x.tres");
  }

  protected override string FaceButtonWestGlyph
  {
    get => ImageHelper.GetImagePath(this.FolderPath + "/y.tres");
  }
}
