using System;
using Xunit;

/// <summary>
/// Набір тестів для перевірки логіки парсингу та кастомних методів маніпуляції текстом.
/// </summary>
public class UnitTest1
{
    /// <summary>
    /// Перевіряє, чи статичний метод Parse коректно трансформує String та видаляє зайві пробіли.
    /// </summary>
    [Fact]
    public void Text_Parse_ShouldRemoveExtraSpacesAndTabs()
    {
        string input = "Академія\t\tмистецтв   та    наук";
        Text text = Text.Parse(input);
        
        Assert.Equal("Академія мистецтв та наук", text.GetText());
    }

    /// <summary>
    /// Перевіряє роботу вбудованого методу заміни слів визначеної довжини.
    /// </summary>
    [Fact]
    public void Text_ReplaceWordsOfLength_ShouldExecuteCustomReplacement()
    {
        Text text = Text.Parse("Це перше речення! А це друге речення.");
        text.ReplaceWordsOfLength(5, "***");
        
        Assert.Equal("Це *** речення! А це *** речення.", text.GetText());
    }

    /// <summary>
    /// Перевіряє успішний пошук об'єкта, що використовує спарсений тип Text.
    /// </summary>
    [Fact]
    public void FindInstitution_ShouldLocateElement_WithParsedText()
    {
        var array = new[] {
            new EducationalInstitution(Text.Parse("КНУ"), 4, 26000, 1834, Text.Parse("Київ")),
            new EducationalInstitution(Text.Parse("КПІ"), 4, 25000, 1898, Text.Parse("Київ"))
        };
        var target = new EducationalInstitution(Text.Parse("КПІ"), 4, 25000, 1898, Text.Parse("Київ"));

        int index = Lab4.FindInstitution(array, target);

        Assert.Equal(1, index);
    }
}