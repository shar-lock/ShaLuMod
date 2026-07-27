## Launch Option
You can launch the game with no mods by passing the`-nomods` launch option in steam
- Right click the game in steam > `Properties...` 
- Enter `-nomods` in the `Launch Options` box
<img width="850" alt="nomods" src="https://github.com/user-attachments/assets/58a84811-a3b0-44a2-a6ed-98dbaa218c74" />

<hr/>

If you want to easily switch between modded and unmodded without editing launch options each time, keep reading
## Separate Steam Entry
1. Create a file named `steam_appid.txt` in your sts2 install directory with the contents `2868840`
2. In the bottom-left of steam click on `Add a Game` > `Add a non-Steam Game...`
3. In the window that pops up, click `Browse...` in the bottom left
4. Navigate to your sts2 install directory and choose `SlayTheSpire2.exe` (windows) or `SlayTheSpire2` (Linux). <br/>
If you don't know where that is, right click on the game in steam and go to `Manage` > `Browse local files`
5. Click `Add Selected Programs` in the bottom right. A game should be added to your steam library with the title `SlayTheSpire2.exe` or `SlayTheSpire2` and no thumbnail image
6. (Optional) Right click on the newly added game, go to `Properties...` and customize the title and thumbnail image

Now, you can follow the instructions at the top of this page to add the `-nomods` launch option to one instance of the game, and the other instance can be used for mods
