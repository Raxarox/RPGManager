using RPGManager.GameData.Characters;
using Xunit;

namespace RPGManager.Tests;

public class AbilityScoreTest
{
    [Fact]
    public void AbilityScore_DefaultValues_ShouldBeTen()
    {
        var scores = new AbilityScore();
        Assert.Equal(10, scores.Strength);
        Assert.Equal(10, scores.Dexterity);
        Assert.Equal(10, scores.Constitution);
        Assert.Equal(10, scores.Intelligence);
        Assert.Equal(10, scores.Wisdom);
        Assert.Equal(10, scores.Charisma);
    }
    
    [Theory]
    [InlineData(15, true, 15)]
    [InlineData(1, true, 1)]
    [InlineData(0, false, 10)]
    [InlineData(-3, false, 10)]
    public void SetStrength_ValidatesAndUpdatesState(int input, bool expectedSuccess, int expectedStrength)
    {
        var scores = new AbilityScore();
        var success = scores.SetStrength(input);
        Assert.Equal(expectedSuccess, success);
        Assert.Equal(expectedStrength, scores.Strength);
    }
    
    public class AbilityScoreTests
    {
        [Fact]
        public void Equals_SameValues_ReturnsTrueAndMatchesHashCodes()
        {
            var score1 = new AbilityScore(10, 12, 14, 16, 8, 10);
            var score2 = new AbilityScore(10, 12, 14, 16, 8, 10);
            Assert.True(score1.Equals(score2));
            Assert.True(score2.Equals(score1));
            Assert.True(score1.Equals((object)score2));
            Assert.Equal(score1.GetHashCode(), score2.GetHashCode());
        }

        [Theory]
        [InlineData(11, 12, 14, 16, 8, 10)] 
        [InlineData(10, 10, 14, 16, 8, 10)] 
        public void Equals_DifferentValues_ReturnsFalse(int str, int dex, int con, int intl, int wis, int cha)
        {
            var baseline = new AbilityScore(10, 12, 14, 16, 8, 10);
            var modified = new AbilityScore(str, dex, con, intl, wis, cha);

            Assert.False(baseline.Equals(modified));
            Assert.False(modified.Equals(baseline));
        }

        [Fact]
        public void Equals_NullOrWrongType_ReturnsFalse()
        {
            var score = new AbilityScore(10, 12, 14, 16, 8, 10);

            Assert.False(score.Equals(null));
            Assert.False(score.Equals("NotAnAbilityScore"));
        }
    }
}