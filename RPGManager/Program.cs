using RPGManager.System;
using RPGManager.UI;

var classRegistry = AssetLoader.LoadClasses("Data/Classes/");
var itemRegistry = AssetLoader.LoadItems("Data/Items/");
var assetRegistry = new GameAssetRegistry(classRegistry, itemRegistry);
GameRunner.RunGame(assetRegistry);