using RPGManager.GameData.Characters;
using RPGManager.System;

namespace RPGManager.Tests.System;

public class AssetLoaderTest
{
    [Fact]
    public void LoadClasses_ValidDirectory_ReturnsDictionary()
    {
        var result = AssetLoader.LoadClasses("Assets/DefaultData/");

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }
}
