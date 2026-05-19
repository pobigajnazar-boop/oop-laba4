using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Спільний інтерфейс для елементів речення (слів та розділових знаків).
/// </summary>
public interface ISentenceElement
{
    string GetText();
}

/// <summary>
/// Клас, що представляє окрему літеру.
/// </summary>
public class Letter
{
    public char Symbol { get; }

    public Letter(char symbol)
    {
        Symbol = symbol;
    }
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
}

/// <summary>
/// Клас, що представляє речення. Складається з масиву елементів.
/// </summary>
public class Sentence
{
    private readonly List<ISentenceElement> _elements = new List<ISentenceElement>();

    public Sentence(string sentenceText)
    {
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
                    _elements.Add(new Word(currentWord));
                    currentWord = "";
                }
                _elements.Add(new Punctuation(c));
            }
        }

        if (currentWord.Length > 0)
        {
            _elements.Add(new Word(currentWord));
        }
    }

    /// <summary>
    /// Виконує заміну слів заданої довжини на новий об'єкт Word.
    /// </summary>
    public void ReplaceWordsOfLength(int targetLength, string replacement)
    {
        for (int i = 0; i < _elements.Count; i++)
        {
            if (_elements[i] is Word word && word.Length == targetLength)
            {
                _elements[i] = new Word(replacement);
            }
        }
    }

    public string GetText() => string.Join("", _elements.Select(e => e.GetText()));
}

/// <summary>
/// Клас, що представляє текст. Складається з масиву речень.
/// </summary>
public class Text
{
    private readonly List<Sentence> _sentences = new List<Sentence>();

    public Text(string rawText)
    {
        string cleanedText = Regex.Replace(rawText, @"[\t ]+", " ").Trim();
        string currentSentence = "";

        foreach (char c in cleanedText)
        {
            currentSentence += c;
            if (c == '.' || c == '!' || c == '?')
            {
                _sentences.Add(new Sentence(currentSentence.TrimStart()));
                currentSentence = "";
            }
        }

        if (currentSentence.Trim().Length > 0)
        {
            _sentences.Add(new Sentence(currentSentence.TrimStart()));
        }
    }

    /// <summary>
    /// Парсить звичайний рядок типу String у створений клас Text.
    /// </summary>
    public static Text Parse(string rawText)
    {
        return new Text(rawText);
    }

    /// <summary>
    /// Делегує команду заміни слів кожному реченню в тексті.
    /// </summary>
    public void ReplaceWordsOfLength(int targetLength, string replacement)
    {
        foreach (var sentence in _sentences)
        {
            sentence.ReplaceWordsOfLength(targetLength, replacement);
        }
    }

    public override string ToString() => string.Join(" ", _sentences.Select(s => s.GetText())).Trim();
}

public class Lab4
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string inputText = "Зварив собі каву, дивлюсь у вікно на цей дощ і думаю: ну і де та весна ділась";
        int targetLength = 5;
        string replacement = "бургер";

        try
        {
            Console.WriteLine("--- Оригінальний текст ---");
            Console.WriteLine(inputText);

            Text myText = Text.Parse(inputText);

            Console.WriteLine("\n--- Нормалізований текст (ООП модель) ---");
            Console.WriteLine(myText.ToString());

            myText.ReplaceWordsOfLength(targetLength, replacement);

            Console.WriteLine($"\n--- Результат (заміна слів з {targetLength} літер) ---");
            Console.WriteLine(myText.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine($"Сталася помилка: {e.Message}");
        }
    }
}