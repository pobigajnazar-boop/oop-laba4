using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Спільний інтерфейс для елементів речення (слів та розділових знаків).
/// </summary>
public interface ISentenceElement : IEquatable<ISentenceElement>
{
    string GetText();
}

/// <summary>
/// Клас, що представляє окрему літеру.
/// </summary>
public class Letter : IEquatable<Letter>
{
    public char Symbol { get; }

    public Letter(char symbol)
    {
        Symbol = symbol;
    }

    public bool Equals(Letter other) => other != null && Symbol == other.Symbol;
    public override bool Equals(object obj) => Equals(obj as Letter);
    public override int GetHashCode() => Symbol.GetHashCode();
}

/// <summary>
/// Клас, що представляє слово. Складається з масиву літер.
/// </summary>
public class Word : ISentenceElement
{
    private readonly Letter[] _letters;

    public int Length => _letters.Length;

    public Word(string wordText)
    {
        _letters = new Letter[wordText.Length];
        for (int i = 0; i < wordText.Length; i++)
        {
            _letters[i] = new Letter(wordText[i]);
        }
    }

    public string GetText() => string.Join("", _letters.Select(l => l.Symbol));

    public bool Equals(ISentenceElement other)
    {
        if (other is Word w)
        {
            if (_letters.Length != w._letters.Length) return false;
            for (int i = 0; i < _letters.Length; i++)
            {
                if (!_letters[i].Equals(w._letters[i])) return false;
            }
            return true;
        }
        return false;
    }
}

/// <summary>
/// Клас, що представляє розділовий знак або пробіл.
/// </summary>
public class Punctuation : ISentenceElement
{
    public char Symbol { get; }

    public Punctuation(char symbol)
    {
        Symbol = symbol;
    }

    public string GetText() => Symbol.ToString();

    public bool Equals(ISentenceElement other) => other is Punctuation p && Symbol == p.Symbol;
}

/// <summary>
/// Клас, що представляє речення. Складається з масиву елементів.
/// </summary>
public class Sentence : IEquatable<Sentence>
{
    private readonly ISentenceElement[] _elements;

    public Sentence(string sentenceText)
    {
        var elementsList = new List<ISentenceElement>();
        string currentWord = "";

        foreach (char c in sentenceText)
        {
            if (char.IsLetterOrDigit(c) || c == '\'')
            {
                currentWord += c;
            }
            else
            {
                if (currentWord.Length > 0)
                {
                    elementsList.Add(new Word(currentWord));
                    currentWord = "";
                }
                elementsList.Add(new Punctuation(c));
            }
        }

        if (currentWord.Length > 0)
        {
            elementsList.Add(new Word(currentWord));
        }

        _elements = elementsList.ToArray();
    }

    /// <summary>
    /// Кастомний метод для операції заміни об'єктів Word заданої довжини.
    /// </summary>
    public void ReplaceWordsOfLength(int targetLength, string replacement)
    {
        for (int i = 0; i < _elements.Length; i++)
        {
            if (_elements[i] is Word word && word.Length == targetLength)
            {
                _elements[i] = new Word(replacement);
            }
        }
    }

    public string GetText() => string.Join("", _elements.Select(e => e.GetText()));

    public bool Equals(Sentence other)
    {
        if (other == null || _elements.Length != other._elements.Length) return false;
        for (int i = 0; i < _elements.Length; i++)
        {
            if (!_elements[i].Equals(other._elements[i])) return false;
        }
        return true;
    }
}

/// <summary>
/// Клас, що представляє текст. Складається з масиву речень.
/// </summary>
public class Text : IEquatable<Text>
{
    private readonly Sentence[] _sentences;

    public Text(string rawText)
    {
        string cleanedText = Regex.Replace(rawText, @"[\t ]+", " ").Trim();
        var sentencesList = new List<Sentence>();
        string currentSentence = "";
        
        foreach (char c in cleanedText)
        {
            currentSentence += c;
            if (c == '.' || c == '!' || c == '?')
            {
                sentencesList.Add(new Sentence(currentSentence.TrimStart()));
                currentSentence = "";
            }
        }

        if (currentSentence.Trim().Length > 0)
        {
            sentencesList.Add(new Sentence(currentSentence.TrimStart()));
        }

        _sentences = sentencesList.ToArray();
    }

    /// <summary>
    /// Метод парсингу рядка тексту типу String у створений клас Text.
    /// </summary>
    public static Text Parse(string rawText)
    {
        return new Text(rawText);
    }

    /// <summary>
    /// Власна реалізація операції заміни слів заданої довжини в тексті.
    /// </summary>
    public void ReplaceWordsOfLength(int targetLength, string replacement)
    {
        foreach (var sentence in _sentences)
        {
            sentence.ReplaceWordsOfLength(targetLength, replacement);
        }
    }

    public string GetText() => string.Join(" ", _sentences.Select(s => s.GetText())).Trim();

    public bool Equals(Text other)
    {
        if (other == null || _sentences.Length != other._sentences.Length) return false;
        for (int i = 0; i < _sentences.Length; i++)
        {
            if (!_sentences[i].Equals(other._sentences[i])) return false;
        }
        return true;
    }

    public override bool Equals(object obj) => Equals(obj as Text);
    public override int GetHashCode() => GetText().GetHashCode();
    public override string ToString() => GetText();
}

/// <summary>
/// Клас навчального закладу.
/// </summary>
public class EducationalInstitution : IEquatable<EducationalInstitution>
{
    public Text Name { get; set; }
    public int AccreditationLevel { get; set; }
    public int StudentCount { get; set; }
    public int FoundationYear { get; set; }
    public Text City { get; set; }

    public EducationalInstitution(Text name, int accreditationLevel, int studentCount, int foundationYear, Text city)
    {
        Name = name;
        AccreditationLevel = accreditationLevel;
        StudentCount = studentCount;
        FoundationYear = foundationYear;
        City = city;
    }

    public bool Equals(EducationalInstitution other)
    {
        if (other == null) return false;
        return Name.Equals(other.Name) &&
               AccreditationLevel == other.AccreditationLevel &&
               StudentCount == other.StudentCount &&
               FoundationYear == other.FoundationYear &&
               City.Equals(other.City);
    }

    public override bool Equals(object obj) => Equals(obj as EducationalInstitution);
    public override int GetHashCode() => HashCode.Combine(Name, AccreditationLevel, StudentCount, FoundationYear, City);

    public override string ToString()
    {
        return $"[{AccreditationLevel} рівень] {Name.GetText()} (Місто: {City.GetText()}, Студентів: {StudentCount}, Рік: {FoundationYear})";
    }
}

public class Lab4
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        EducationalInstitution[] institutions = {
            new EducationalInstitution(Text.Parse("КПІ"), 4, 25000, 1898, Text.Parse("Київ")),
            new EducationalInstitution(Text.Parse("КНУ"), 4, 26000, 1834, Text.Parse("Київ")),
            new EducationalInstitution(Text.Parse("ЛНУ"), 4, 20000, 1661, Text.Parse("Львів")),
            new EducationalInstitution(Text.Parse("Коледж    зв'язку"), 2, 1500, 1921, Text.Parse("Київ")),
            new EducationalInstitution(Text.Parse("Будівельний\tтехнікум"), 2, 1800, 1944, Text.Parse("Одеса"))
        };

        Console.WriteLine("--- Початковий масив навчальних закладів ---");
        PrintArray(institutions);

        EducationalInstitution[] sortedInstitutions = SortInstitutions(institutions);

        Console.WriteLine("\n--- Відсортований масив ---");
        PrintArray(sortedInstitutions);

        EducationalInstitution target = new EducationalInstitution(Text.Parse("КПІ"), 4, 25000, 1898, Text.Parse("Київ"));
        Console.WriteLine($"\nШукаємо: {target}");

        int foundIndex = FindInstitution(sortedInstitutions, target);
        
        if (foundIndex != -1)
        {
            Console.WriteLine($"Об'єкт знайдено! Індекс у відсортованому масиві: {foundIndex}");
        }
        else
        {
            Console.WriteLine("Об'єкт не знайдено.");
        }
    }

    public static EducationalInstitution[] SortInstitutions(EducationalInstitution[] array)
    {
        return array
            .OrderBy(inst => inst.AccreditationLevel)
            .ThenByDescending(inst => inst.StudentCount)
            .ToArray();
    }

    public static int FindInstitution(EducationalInstitution[] array, EducationalInstitution target)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Equals(target)) return i;
        }
        return -1;
    }

    private static void PrintArray(EducationalInstitution[] array)
    {
        foreach (var item in array)
        {
            Console.WriteLine(item);
        }
    }
}