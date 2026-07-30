using BaseLib.Abstracts;
using WandiMod.WandiModCode.Extensions;
using Godot;

namespace WandiMod.WandiModCode.Character;

public class WandiModRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => WandiMod.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();

    /// <summary>
    /// 图鉴默认可见：true = 本池所有遗物在图鉴中以全彩显示（无需先拾取过）。
    /// false（BaseLib 默认）时未拾取的遗物显示为暗色「未见过」图标，看起来就像「图鉴里没有万敌遗物」。
    /// </summary>
    public override bool SeenByDefault => true;
}