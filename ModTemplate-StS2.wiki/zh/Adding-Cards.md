本模板包含一个用于卡牌的文件模板。

## 创建文件

首先，创建一个用于放置卡牌的文件夹。通常它就直接叫做 `Cards`。如果你想要进一步组织，可以按稀有度和/或类型再创建文件夹。

右键点击你创建的文件夹并选择 `Add`。你应该会看到 `Custom Card` 选项。

如果该选项没有出现，可能是因为你在创建项目时没有勾选 `Put solution and project in same directory`（参见 [Setup 的第 4 步](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup)），或者你使用的是空模板（而不是角色或内容模组模板）创建的项目。你可以通过重新创建项目来解决这些问题。如果这样做也没用，你可以改为手动创建一个新类并自行填写代码（不太方便，但也不会多花太多时间）。

<img width="518" height="194" alt="图片" src="https://github.com/user-attachments/assets/e9e99460-7d62-4035-811a-7a8ad05ee8fd" />

输入名称并创建文件。

<img width="887" height="438" alt="图片" src="https://github.com/user-attachments/assets/60f3c068-84e3-44bc-a733-da1a48f04c37" />

这会生成一个继承自你模组卡牌模型（例如 `YourModNameCard`）的类。顶部会有几个高亮的字段，这些就是"模板"值。
第一个是费用。你可以直接输入一个数字来替换 `int`。设置完一个字段后，按 TAB 键跳到下一个。

<img width="1005" height="438" alt="图片" src="https://github.com/user-attachments/assets/2ee8adec-b0ae-45a0-9591-6d95f390483e" />

接下来的 3 个条目会打开下拉菜单。使用上下方向键选择所需选项后按 TAB 键，或者直接双击即可选中。

现在你已经有了一张没有任何效果的卡牌。如果使用的是角色模板，它应该继承自你模组中对应的抽象类，并会自动添加到该角色的卡牌池中。否则，你需要自行将其添加到某个特定的池中。

## 卡牌池

要将其添加到某个卡牌池中，请在类上方添加 `[Pool]` 特性。如果你使用的是角色模板，则不需要这样做，因为基类已经标上了该角色的池特性。

<img width="237" height="48" alt="图片" src="https://github.com/user-attachments/assets/25b160e5-759a-488c-9b6f-df1969ce5abd" />

如果是制作一张 Ironclad 卡牌，它会是这样。

<img width="778" height="62" alt="图片" src="https://github.com/user-attachments/assets/789b9567-13ce-406a-aef7-0a03d82d1031" />

## 本地化

所有本地化内容都会放在你的 `ModName` 文件夹中，该文件夹中应该包含 `mod_image.png`。

<img width="182" height="52" alt="图片" src="https://github.com/user-attachments/assets/07e2e397-83ba-42e9-b616-45571a0421f2" />

如果该文件夹不存在，请创建一个 `localization/eng` 文件夹。（你也可以使用其他语言，但需要自行确定它们的文件夹名称。它将是一个 [ISO 639-2](https://www.loc.gov/standards/iso639-2/php/code_list.php) 代码。）在该文件夹中，创建一个 `cards.json` 文件。这个文件将保存你所有卡牌的本地化内容。

<img width="195" height="220" alt="图片" src="https://github.com/user-attachments/assets/c4d5b5cf-c091-45c9-9e28-2f514d1cb61e" />

如果 `.json` 文件中没有花括号，只需添加一对花括号即可：

```
{

}
```

### 为卡牌添加本地化

如果你将鼠标悬停在卡牌的类名上，应该会看到类似这样的错误。

<img width="960" height="172" alt="图片" src="https://github.com/user-attachments/assets/c59da329-ce14-45b3-ad91-0f10b05e1a56" />

本模板使用了一个自定义分析器，它会报告缺失的本地化，并能帮助生成占位用的本地化内容。点击类名并按 Alt+Enter。这会显示一个可执行操作的列表。

<img width="464" height="94" alt="图片" src="https://github.com/user-attachments/assets/83e9eca8-c2c6-4b95-ad91-06e486199077" />

点击 `Generate localization` 选项。这会在你的类定义上方生成一些文本。

<img width="441" height="149" alt="图片" src="https://github.com/user-attachments/assets/0badc0f6-f8a8-405c-8024-753298a4f634" />

选中它，剪切（ctrl+x），粘贴到 `cards.json` 中，然后保存文件。

<img width="379" height="66" alt="图片" src="https://github.com/user-attachments/assets/65aace64-4eaa-415d-a549-43cf375c284a" />
<img width="353" height="72" alt="图片" src="https://github.com/user-attachments/assets/27ccd045-0144-4963-b816-ba8688648c38" />

你的卡牌现在应该不再报错了。

## 美术

卡牌美术放在 `YourModName/images/card_portraits/big/` 文件夹中。图片应为 1000x760 的 PNG（全画幅卡牌则为 606x852）。命名与卡牌的类名相同，但全部小写并在单词之间用下划线连接；例如，`CardName` 应命名为 `card_name.png`。你可以看到模板创建了一个名为 `card.png` 的示例图片（它将作为没有自己图片的卡牌的默认图片）。如果图片较小也可以，只要保持相同的比例即可；图片会被放大。

`big` 文件夹中的一张图片就是让卡牌正常工作所需的全部内容。你还可以添加另外两种图片（命名应相同，但放在不同的文件夹中）：
* 在 `YourModName/images/card_portraits/` 文件夹中，你可以放置一张 250x190（全画幅为 250x350）版本的卡牌图片。这会让游戏运行得更高效一些（但并非必需；游戏可以直接将大图缩小）。
* 在 `YourModName/images/card_portraits/beta` 文件夹中，你可以放置一张 beta 美术版本的卡牌图片。当玩家开启了 beta 美术时，它会显示出来（没有 beta 美术的卡牌会使用其普通美术）。

## 功能

基础卡牌的教程以后会补充，但无论如何，建议你 [反编译基础游戏卡牌](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling) 并 [查看它们的文本](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text)，以了解它们是如何实现各自效果的。

## 编辑卡牌模板

角色和内容模组模板已经预设了抽象类和文件模板，但如果你需要自己制作，可以在 Ctrl+Alt+S（Settings）-> Editor -> File Templates -> C# 中编辑文件模板。

<img width="954" height="735" alt="图片" src="https://github.com/user-attachments/assets/13023046-13d1-46db-9758-19336864213a" />

选择你正在使用的模板并按需修改（例如，你可以添加一个卡牌池特性）。然后，**不要**直接点击 `Save`；点击该按钮上的下拉箭头，并选择 `team-shared` 选项。

<img width="261" height="161" alt="图片" src="https://github.com/user-attachments/assets/411103ac-3d8b-4267-81f9-c63b9d9628b3" />
