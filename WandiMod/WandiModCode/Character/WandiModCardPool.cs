using BaseLib.Abstracts;
using WandiMod.WandiModCode.Extensions;
using Godot;

namespace WandiMod.WandiModCode.Character;

public class WandiModCardPool : CustomCardPoolModel
{
    public override string Title => WandiMod.CharacterId; //This is not a display name.
    
    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();


    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    // 万敌主色：血红色（红 hue + 高饱和 + 压低亮度 = 深血红）。V 可上下调深浅，需游戏内微调。
    public override float H => 0f;   //Hue：0=红
    public override float S => 1f;   //Saturation：满饱和
    public override float V => 0.6f; //Brightness：压低 → 深血红（1=亮红/接近战士原色，越低越深）

    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load WandiMod/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/

    //Color of small card icons（牌组列表小卡牌图标）—— 同步血红色
    public override Color DeckEntryCardColor => new("B71C1C");
    
    public override bool IsColorless => false;
}