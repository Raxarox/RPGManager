using RPGManager.System;

namespace RPGManager.Tests.System;

public class ValidationHelperTest
{
    [Fact]
    public void ValidatePositiveValue_Int_ValidValue_DoesNotThrow()
    {
        var exception = Record.Exception(() =>
            ValidationHelper.ValidatePositiveValue("TestItem", "ItemType", "PropertyName", 5));

        Assert.Null(exception);
    }

    [Fact]
    public void ValidatePositiveValue_Int_ZeroValue_DoesNotThrow()
    {
        var exception = Record.Exception(() =>
            ValidationHelper.ValidatePositiveValue("TestItem", "ItemType", "PropertyName", 0));

        Assert.Null(exception);
    }

    [Fact]
    public void ValidatePositiveValue_Int_NegativeValue_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ValidationHelper.ValidatePositiveValue("TestItem", "ItemType", "PropertyName", -1));
    }

    [Fact]
    public void ValidatePositiveValue_Decimal_ValidValue_DoesNotThrow()
    {
        var exception = Record.Exception(() =>
            ValidationHelper.ValidatePositiveValue("TestItem", "ItemType", "PropertyName", 5.5m));

        Assert.Null(exception);
    }

    [Fact]
    public void ValidatePositiveValue_Decimal_ZeroValue_DoesNotThrow()
    {
        var exception = Record.Exception(() =>
            ValidationHelper.ValidatePositiveValue("TestItem", "ItemType", "PropertyName", 0.0m));

        Assert.Null(exception);
    }

    [Fact]
    public void ValidatePositiveValue_Decimal_NegativeValue_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ValidationHelper.ValidatePositiveValue("TestItem", "ItemType", "PropertyName", -1.5m));
    }

    [Fact]
    public void ValidateRequiredString_ValidString_DoesNotThrow()
    {
        var exception = Record.Exception(() =>
            ValidationHelper.ValidateRequiredString("TestItem", "ItemType", "PropertyName", "ValidString"));

        Assert.Null(exception);
    }

    [Fact]
    public void ValidateRequiredString_NullString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            ValidationHelper.ValidateRequiredString("TestItem", "ItemType", "PropertyName", null));
    }

    [Fact]
    public void ValidateRequiredString_EmptyString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            ValidationHelper.ValidateRequiredString("TestItem", "ItemType", "PropertyName", ""));
    }

    [Fact]
    public void ValidateRequiredString_WhitespaceString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            ValidationHelper.ValidateRequiredString("TestItem", "ItemType", "PropertyName", "   "));
    }

    [Fact]
    public void ValidatePositiveInteger_ValidValue_DoesNotThrow()
    {
        var exception = Record.Exception(() =>
            ValidationHelper.ValidatePositiveInteger("TestItem", "ItemType", "PropertyName", 5));

        Assert.Null(exception);
    }

    [Fact]
    public void ValidatePositiveInteger_ZeroValue_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ValidationHelper.ValidatePositiveInteger("TestItem", "ItemType", "PropertyName", 0));
    }

    [Fact]
    public void ValidatePositiveInteger_NegativeValue_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ValidationHelper.ValidatePositiveInteger("TestItem", "ItemType", "PropertyName", -1));
    }

    [Fact]
    public void ValidationMessages_IncludeNameAndItemType()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ValidationHelper.ValidatePositiveValue("Sword", "Weapon", "Weight", -1));

        Assert.Contains("Weapon", exception.Message);
        Assert.Contains("Sword", exception.Message);
        Assert.Contains("Weight", exception.Message);
    }
}
