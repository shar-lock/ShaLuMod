This template includes a file template for cards.

## Creating the File

First, create a folder to put your cards in. Most commonly this is simply called `Cards`. If you want additional organization you can make folders for rarity and/or type.

Right click the folder you created and choose `Add`. You should see the option for `Custom Card`.

If this option doesn't show up, it may be because you did not check the box for `Put solution and project in same directory` when you made the project [step 4 of setup](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup) or you made a project using the empty template (instead of the character or content mod templates). You can fix those issues by recreating the project. If that doesn't help, you can proceed by creating a new class instead and filling in the code yourself (it's inconvenient but doesn't take that much longer).

<img width="518" height="194" alt="image" src="https://github.com/user-attachments/assets/e9e99460-7d62-4035-811a-7a8ad05ee8fd" />

Enter a name and create the file.

<img width="887" height="438" alt="image" src="https://github.com/user-attachments/assets/60f3c068-84e3-44bc-a733-da1a48f04c37" />

This should result in a class inheriting from the card model for your mod (e.g. `YourModNameCard`). There will be a few fields at the top highlighted. These are the "template" values. 
The first one is cost. You can just type a number, which will replace `int`. After you're done setting a field, press TAB to move to the next one.

<img width="1005" height="438" alt="image" src="https://github.com/user-attachments/assets/2ee8adec-b0ae-45a0-9591-6d95f390483e" />

The next 3 entries will open a dropdown. Use the up/down arrow keys and then press TAB after selecting the desired option, or just double-click on them.

You now have a card that has no effect. If using the character template, it should be inheriting an abstract named for your mod and will automatically be added to that character's card pool. Otherwise, you'll need to add it to a specific pool yourself.

## Card Pool

To add it to a card pool, add the `[Pool]` attribute above the class. If you are using the character template, you do not need to do this, as the base class is already marked with the pool attribute for the character.

<img width="237" height="48" alt="image" src="https://github.com/user-attachments/assets/25b160e5-759a-488c-9b6f-df1969ce5abd" />

If making an Ironclad card, it would look like this.

<img width="778" height="62" alt="image" src="https://github.com/user-attachments/assets/789b9567-13ce-406a-aef7-0a03d82d1031" />

## Localization

All localization will go in your `ModName` folder, which should contain `mod_image.png`.

<img width="182" height="52" alt="image" src="https://github.com/user-attachments/assets/07e2e397-83ba-42e9-b616-45571a0421f2" />

If it doesn't exist, create a `localization/eng` folder. (You can use a different language, but you will have to identify their folder names. It will be an [ISO 639-2](https://www.loc.gov/standards/iso639-2/php/code_list.php) code.) Inside that folder, create a `cards.json` file. This will hold the localization for all your cards.

<img width="195" height="220" alt="image" src="https://github.com/user-attachments/assets/c4d5b5cf-c091-45c9-9e28-2f514d1cb61e" />

Inside the `.json` file, simply add a pair of curly brackets if they are not present.

```
{

}
```

### Adding Localization for a Card

If you hover over your card's class name, you should see an error like this.

<img width="960" height="172" alt="image" src="https://github.com/user-attachments/assets/c59da329-ce14-45b3-ad91-0f10b05e1a56" />

This template uses a custom analyzer that will report missing localization, and can help with generating placeholder localization. Click the class name and press Alt+Enter. This should show a list of actions that can be taken.

<img width="464" height="94" alt="image" src="https://github.com/user-attachments/assets/83e9eca8-c2c6-4b95-ad91-06e486199077" />

Click the `Generate localization` option. This will generate some text above your class definition.

<img width="441" height="149" alt="image" src="https://github.com/user-attachments/assets/0badc0f6-f8a8-405c-8024-753298a4f634" />

Select it, cut it (ctrl+x), paste it into `cards.json`, and save the file.

<img width="379" height="66" alt="image" src="https://github.com/user-attachments/assets/65aace64-4eaa-415d-a549-43cf375c284a" />
<img width="353" height="72" alt="image" src="https://github.com/user-attachments/assets/27ccd045-0144-4963-b816-ba8688648c38" />

Your card should now have no errors.

## Art
Card art goes in the folder `YourModName/images/card_portraits/big/`. The image should be a 1000x760 PNG (or 606x852 for full art cards). Name it the same as the card's class, except all lowercase and with underscores between words; so for example, `CardName` becomes `card_name.png`. You can see an example image named `card.png` created by the template (which will be used as the default image for cards without their own images). It's okay if the image is smaller as long as you keep the same ratio; the image will be scaled up.

The one image in the `big` folder is all you need for a card to work. There are two other images you can add (that should be named the same but go in different folders):
* In the folder `YourModName/images/card_portraits/`, you can put a 250x190 (250x350 for full art) version of your card image. This will make things a bit more efficient for the game (but isn't necessary; the game can just scale down the big image).
* In the folder `YourModName/images/card_portraits/beta`, you can put a beta art version of your card image. This will be visible if people have beta art on (cards without beta art will use their normal art).

## Functionality

A tutorial for a basic card will be added eventually, but regardless you are recommended to [decompile base game cards](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling) and [look at their text](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text) to see how they implement their effects.

## Editing the card template
The character and content mod templates have an abstract class and file template already set up, but if you need to edmake your own, you can edit file templates at Ctrl+Alt+S (Settings) -> Editor -> File Templates -> C#.

<img width="954" height="735" alt="image" src="https://github.com/user-attachments/assets/13023046-13d1-46db-9758-19336864213a" />

Select the template you are using and change whatever you want (e.g., you could add a card pool attribute). Then, DO NOT click `Save` directly; click the dropdown arrow on the button and choose the `team-shared` option.

<img width="261" height="161" alt="image" src="https://github.com/user-attachments/assets/411103ac-3d8b-4267-81f9-c63b9d9628b3" />