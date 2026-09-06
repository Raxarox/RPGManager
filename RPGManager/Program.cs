using RPGManager.System;
using RPGManager.UI;

var classRegistry = AssetLoader.LoadClasses("Assets/DefaultData/");
var itemRegistry = AssetLoader.LoadItems("Assets/DefaultData/");
var archetypeRegistry = AssetLoader.LoadArchetypes("Assets/DefaultData/");
var assetRegistry = new GameAssetRegistry(classRegistry, itemRegistry, archetypeRegistry);
GameRunner.RunGame(assetRegistry);