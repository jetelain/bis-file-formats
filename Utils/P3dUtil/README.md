# p3dutil

This tool can manipulate only MLOD files.

## Templating

This verb allows you to generate p3d files from a template file.

`p3dutil template <path to json file>`

### "per-texture" templating

This template mode allows you to generate a p3d for each paa file from a directory.

This can usefull to generated variations of a p3d file for road/town signs or any model that cannot use hiddenselection.

Sample for playing cards (as featured in one of my mods) :

`p3dutil template playingcards.json`

File `playingcards.json` (stored in `addons\playingcards`) :
```json
{
	"Mode": "per-texture",
	"TemplateFile": "data\clubs\1.p3d", // Template file
	"TextureBaseDirectory": "data", // Directory to search for paa files
	"TextureBaseGamePath": "z\gtd\addons\playingcards\data", // Engine path matching TextureBaseDirectory 
	"TexturePattern": "*_256.paa", // Pattern to use to search for paa files
	"TextureNameFilter": "_256.paa", // Used to generate target p3d file name
	"InitialTexture": "z\gtd\addons\playingcards\data\clubs\1_256.paa", // Texture in TemplateFile to replace
	"Backup": true // Generate ".bak" files if p3d already exists
}
```

## Path replace

This verb allows you to make a find and replace operation on texture and material paths of p3d file.

`p3dutil replace-path <p3d path> <old path> <new path>`

Sample : 
`p3dutil replace-path *.p3d "z\gtd\" "z\gtdi\"`
