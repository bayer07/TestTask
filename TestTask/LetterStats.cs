using System;

namespace TestTask
{
    /// <summary>
    /// Статистика вхождения буквы/пары букв
    /// </summary>
    public struct LetterStats
    {
        /// <summary>
        /// Буква/Пара букв для учёта статистики.
        /// </summary>
        public string Letter;

        /// <summary>
        /// Кол-во вхождений буквы/пары.
        /// </summary>
        public int Count;

        public override int GetHashCode()
        {
            int result = Int32.Parse(Letter);
            return result;
        }

        public override bool Equals(object obj)
        {
            var isLetterStats = obj is LetterStats;
            if (isLetterStats)
            {
                var letterStats = (LetterStats)obj;
                return letterStats.Letter == Letter;
            }

            return base.Equals(obj);
        }
    }
}
