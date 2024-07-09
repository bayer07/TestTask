using System;
using System.Collections.Generic;
using System.Linq;

namespace TestTask
{
    public class Program
    {


        /// <summary>
        /// Программа принимает на входе 2 пути до файлов.
        /// Анализирует в первом файле кол-во вхождений каждой буквы (регистрозависимо). Например А, б, Б, Г и т.д.
        /// Анализирует во втором файле кол-во вхождений парных букв (не регистрозависимо). Например АА, Оо, еЕ, тт и т.д.
        /// По окончанию работы - выводит данную статистику на экран.
        /// </summary>
        /// <param name="args">Первый параметр - путь до первого файла.
        /// Второй параметр - путь до второго файла.</param>
        public static void Main(string[] args)
        {
            using (IReadOnlyStream inputStream1 = GetInputStream(args[0]))
            {
                IList<LetterStats> singleLetterStats = FillSingleLetterStats(inputStream1);
                RemoveCharStatsByType(ref singleLetterStats, CharType.Vowel);
                PrintStatistic(singleLetterStats);
            }

            using (IReadOnlyStream inputStream2 = GetInputStream(args[1]))
            {
                IList<LetterStats> doubleLetterStats = FillDoubleLetterStats(inputStream2);
                RemoveCharStatsByType(ref doubleLetterStats, CharType.Consonants);
                PrintStatistic(doubleLetterStats);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Ф-ция возвращает экземпляр потока с уже загруженным файлом для последующего посимвольного чтения.
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        /// <returns>Поток для последующего чтения.</returns>
        private static IReadOnlyStream GetInputStream(string fileFullPath)
        {
            return new ReadOnlyStream(fileFullPath);
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            stream.ResetPositionToStart();
            var letterStats = new List<LetterStats>();
            while (!stream.IsEof)
            {
                char c = stream.ReadNextChar();
                int a = (int)c;
                if (!char.IsLetter(c))
                    continue;

                PutLetterToLetterStats(c.ToString(), letterStats);
            }

            return letterStats;
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения парных букв.
        /// В статистику должны попадать только пары из одинаковых букв, например АА, СС, УУ, ЕЕ и т.д.
        /// Статистика - НЕ регистрозависимая!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillDoubleLetterStats(IReadOnlyStream stream)
        {
            stream.ResetPositionToStart();
            var letterStats = new List<LetterStats>();
            string prev = string.Empty;
            while (!stream.IsEof)
            {
                char c = stream.ReadNextChar();
                if (!char.IsLetter(c))
                    continue;

                string letter = c.ToString().ToLower();
                if (letter == prev)
                {
                    PutLetterToLetterStats(letter, letterStats);
                    letter = string.Empty;
                }

                prev = letter;
            }

            return letterStats;
        }

        private static void PutLetterToLetterStats(string ch, List<LetterStats> letterStats)
        {
            LetterStats letterStat = new LetterStats { Letter = ch.ToString() };
            int index = letterStats.IndexOf(letterStat);
            if (index != -1)
            {
                letterStat = letterStats[index];
                IncStatistic(ref letterStat);
                letterStats[index] = letterStat;
            }
            else
            {
                letterStat = new LetterStats { Count = 1, Letter = ch };
                letterStats.Add(letterStat);
            }
        }

        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        private static void RemoveCharStatsByType(ref IList<LetterStats> letters, CharType charType)
        {
            switch (charType)
            {
                case CharType.Consonants:
                    letters = letters.Where(letter => _consonants.Any(consonant => consonant.Letter.ToUpper() == letter.Letter.ToUpper())).ToList();
                    break;
                case CharType.Vowel:
                    letters = letters.Where(letter => _vowels.Any(vowel => vowel.Letter.ToUpper() == letter.Letter.ToUpper())).ToList();
                    break;
            }
        }

        private static readonly List<LetterStats> _consonants = new List<LetterStats> {
            new LetterStats { Letter = "Б" },
            new LetterStats { Letter = "В" },
            new LetterStats { Letter = "Г" },
            new LetterStats { Letter = "Д" },
            new LetterStats { Letter = "Ж" },
            new LetterStats { Letter = "З" },
            new LetterStats { Letter = "Й" },
            new LetterStats { Letter = "К" },
            new LetterStats { Letter = "Л" },
            new LetterStats { Letter = "М" },
            new LetterStats { Letter = "Н" },
            new LetterStats { Letter = "П" },
            new LetterStats { Letter = "Р" },
            new LetterStats { Letter = "С" },
            new LetterStats { Letter = "Т" },
            new LetterStats { Letter = "Ф" },
            new LetterStats { Letter = "Х" },
            new LetterStats { Letter = "Ц" },
            new LetterStats { Letter = "Ч" },
            new LetterStats { Letter = "Ш" },
            new LetterStats { Letter = "Щ" }
        };
        private static readonly List<LetterStats> _vowels = new List<LetterStats> {
             new LetterStats { Letter = "А" },
             new LetterStats { Letter = "О" },
             new LetterStats { Letter = "У" },
             new LetterStats { Letter = "Ы" },
             new LetterStats { Letter = "И" },
             new LetterStats { Letter = "Э" },
             new LetterStats { Letter = "Е" },
             new LetterStats { Letter = "Ё" },
             new LetterStats { Letter = "Ю" },
             new LetterStats { Letter = "Я" }
        };

        /// <summary>
        /// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}"
        /// Каждая буква - с новой строки.
        /// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
        /// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
        /// </summary>
        /// <param name="letters">Коллекция со статистикой</param>
        private static void PrintStatistic(IEnumerable<LetterStats> letters)
        {
            letters = letters.OrderBy(letter => letter.Letter);
            letters.ToList().ForEach(letter => Console.WriteLine($"{letter.Letter} : {letter.Count}"));
            Console.WriteLine($"ИТОГО: {letters.Count()}");
        }

        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        /// <param name="letterStats"></param>
        private static void IncStatistic(ref LetterStats letterStats)
        {
            letterStats.Count++;
        }
    }
}
