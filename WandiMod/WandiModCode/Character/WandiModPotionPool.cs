using BaseLib.Abstracts;
using WandiMod.WandiModCode.Extensions;
using Godot;

namespace WandiMod.WandiModCode.Character;

public class WandiModPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => WandiMod.Color;
    

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}