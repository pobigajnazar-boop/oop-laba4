using System;
using Xunit;

public class UnitTest1
{
    /// <summary>
    /// Перевіряє, чи видаляються зайві таби і пробіли під час парсингу тексту.
    /// </summary>
    [Fact]
    public void Text_Parse_ShouldNormalizeSpacesAndTabs()
    {
        string input = "Слово1   Слово2\t \t Слово3.";
        Text text = Text.Parse(input);
        
        Assert.Equal("Слово1 Слово2 Слово3.", text.ToString());
    }

    /// <summary>
    /// Перевіряє коректність виконання заміни слів потрібної довжини (кастомний текст).
    /// </summary>
    [Fact]
    public void Text_ReplaceWordsOfLength_ShouldExecuteCorrectly()
    {
        // Arrange (Підготовка)
        Text text = Text.Parse("Зварив собі каву, дивлюсь у вікно на цей дощ і думаю: ну і де та весна ділась");
        
        // Act (Виконання)
        text.ReplaceWordsOfLength(5, "бургер");
        
        // Assert (Перевірка)
        Assert.Equal("Зварив собі каву, дивлюсь у бургер на цей дощ і бургер: ну і де та бургер ділась", text.ToString());
    }
}