# README

Ken ga Kimi alr file regenerator for fan translation patches.
Works with the steam release of the game.

### NOTE:

This tool is still in development and may not work in all instances.  
When reporting issues please provide a detailed description and relevant files if applicable.

### What are alr files?

alr files pair in-game dialogue with an "already read" flag in the save data to keep track of what portions of the game have been read. If there is a mismatch between these files, the game will count all text as unread. This prevents the usage of the skip already read text option.

Whenever script text is modified, the alr files should be regenerated so they are kept in sync with the new text.

### How to use

- You'll need a copy of the game and [UABEA](https://github.com/nesrak1/UABEA/releases)
- Use UABEA to extract the bundle containing the scripts
- you should have a folder full of script files and their paired alr -
  Ex: `KEN_00_00_00` and `KEN_00_00_00_alr`
- Open the regenerator application
- Click the "Script and alr directory" button. select the folder with your script and alr pairs
- Click the "Output Directory" and select the folder to output the new alr files
- Use [UABEA](https://github.com/nesrak1/UABEA/releases) to reinsert the modified alr files into the bundle

[Video Tutorial](https://youtu.be/bsEN1GuCh_8)

### Where has this tool been used?

- Korean Patch: https://www.postype.com/en/@mowi123/post/18666538
- English Patch: Currently a WIP by the [Script Eating Monsters](https://x.com/kgk_sem)
