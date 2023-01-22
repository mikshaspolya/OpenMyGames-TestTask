using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskExam
{
    internal class TaskSolver
    {
        public static void Main(string[] args)
        {
            TestGenerateWordsFromWord();
            TestMaxLengthTwoChar();
            TestGetPreviousMaxDigital();
            TestSearchQueenOrHorse();
            TestCalculateMaxCoins();

            Console.WriteLine("All Test completed!");
        }


        /// задание 1) Слова из слова
        public static List<string> GenerateWordsFromWord(string word, List<string> wordDictionary)
        {
            List<string> findWords = new List<string>();
            bool isWordFromDictionaryMatched;

            foreach (var wordToCheck in wordDictionary)
            {
                string wordCopy = new string(word);
                isWordFromDictionaryMatched = true;

                for (var j = 0; j < wordToCheck.Length && isWordFromDictionaryMatched; j++)
                {
                    if (wordCopy.Contains(wordToCheck[j]))
                    {
                        wordCopy = wordCopy.Remove(wordCopy.IndexOf(wordToCheck[j]), 1);
                    }
                    else 
                    {
                        isWordFromDictionaryMatched = false;
                    }
                }

                if (isWordFromDictionaryMatched)
                {
                    findWords.Add(wordToCheck);
                }
            }

            findWords.Sort();
            return findWords;
        }

        /// задание 2) Два уникальных символа
        public static int MaxLengthTwoChar(string word)
        {
            if (word.Length <= 1)
            {
                return 0;
            }

            List<char> listOfCharacters = FindAllUniqueCharacters(word);
            List<string> listOfCombinations = FindAllCombinations(listOfCharacters);
            List<string> sequencesWithTwoUniqueCharacters = GenerateSequences(word, listOfCombinations);
            int maxLength = 0;

            foreach (var sequence in sequencesWithTwoUniqueCharacters)
            {
                int currentSequenceLength = sequence.Length;
                for (var i = 0; i < sequence.Length - 1; i++)
                {
                    if (sequence[i] == sequence[i + 1])
                    {
                        currentSequenceLength = 0;
                    }
                }

                if (maxLength < currentSequenceLength)
                {
                    maxLength = currentSequenceLength;
                }
            }

            return maxLength;
        }

        private static List<string> GenerateSequences(string word, List<string> listOfCombinations)
        {
            List<string> resultList = new List<string>();
            foreach (var sequence in listOfCombinations)
            {
                string resultWord = String.Concat(word.Split(sequence.ToCharArray()));
                resultWord = String.Concat(word.Split(resultWord.ToCharArray()));
                resultList.Add(resultWord);
            }

            return resultList;
        }

        private static List<string> FindAllCombinations(List<char> listOfCharacters)
        {
            List<string> resultList = new List<string>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < listOfCharacters.Count - 1; i++)
            {
                for (int j = i + 1; j < listOfCharacters.Count; j++)
                {
                    sb.Append(listOfCharacters[i]).Append(listOfCharacters[j]);
                    resultList.Add(sb.ToString());
                    sb.Clear();
                }
            }

            return resultList;
        }

        private static List<char> FindAllUniqueCharacters(string word)
        {
            List<char> chars = new List<char>();
            foreach (var c in word)
            {
                if (!chars.Contains(c))
                {
                    chars.Add(c);
                }
            }
            return chars;
        }

        /// задание 3) Предыдущее число
        public static long GetPreviousMaxDigital(long value)
        {
            string number = value.ToString();
            int[] digits = new int[number.Length];
            for (var i = 0; i < number.Length; i++)
            {
                digits[i] = int.Parse(number[i].ToString());
            }

            bool wasChanged = false;
            int index = -1;
            for (var i = digits.Length - 1; i >= 1 && !wasChanged; i--)
            {
                for (var j = i - 1; j >= 0 && !wasChanged; j--)
                {
                    if (digits[i] < digits[j])
                    {
                        int temp = digits[i];
                        digits[i] = digits[j];
                        digits[j] = temp;

                        wasChanged = true;
                        index = j;
                    }
                }
            }

            for (var i = digits.Length - 1; i > index && wasChanged; i--)
            {
                for (var j = i - 1; j > index; j--)
                {
                    if (digits[i] > digits[j])
                    {
                        int temp = digits[i];
                        digits[i] = digits[j];
                        digits[j] = temp;
                    }
                }
            }

            number = number.Remove(0);
            for (var i = 0; i < digits.Length; i++)
            {
                number += digits[i].ToString();
            }

            if (digits[0] == 0 || long.Parse(number) == value)
            {
                return -1;
            }

            return long.Parse(number);
        }

        /// задание 4) Конь и Королева
        public static List<int> SearchQueenOrHorse(char[][] gridMap)
        {
            int[] start = new int[] { -1, -1 };
            int[] end = new int[] { -1, -1 };

            for (var i = 0; i < gridMap.Length; i++)
            {
                for (var j = 0; j < gridMap[i].Length; j++)
                {
                    if (gridMap[i][j] == 's')
                    {
                        start = new int[] { i, j };
                    }
                    if (gridMap[i][j] == 'e')
                    {
                        end = new int[] { i, j };
                    }
                }
            }

            if (start[0] == -1 || end[0] == -1)
            {
                return new List<int> { -1, -1 };
            }

            int[] knightDx = { 2, 1, -1, -2, -2, -1, 1, 2 };
            int[] knightDy = { 1, 2, 2, 1, -1, -2, -2, -1 };
            int[] queenDx = { 1, 0, -1, 0, 1, 1, -1, -1 };
            int[] queenDy = { 0, 1, 0, -1, 1, -1, 1, -1 };

            int n = gridMap.Length;
            int m = gridMap[0].Length;

            List<int> result = new List<int> { -1, -1 };
            int[] distance = new int[n * m];
            Array.Fill(distance, int.MaxValue);

            Queue<int> q = new Queue<int>();
            int startPos = start[0] * m + start[1];
            distance[startPos] = 0;
            q.Enqueue(startPos);
            bool isFound = false;

            while (q.Count > 0 && !isFound)
            {
                int currentPos = q.Dequeue();
                int x = currentPos / m;
                int y = currentPos % m;
                for (var i = 0; i < 8; i++)
                {
                    int newX = x + knightDx[i];
                    int newY = y + knightDy[i];

                    if (newX >= 0 && newX < n && newY >= 0 && newY < m && gridMap[newX][newY] != 'x')
                    {
                        int newPos = newX * m + newY;
                        if (distance[newPos] == int.MaxValue)
                        {
                            distance[newPos] = distance[currentPos] + 1;
                            q.Enqueue(newPos);

                            if (distance[end[0] * m + end[1]] == distance[newPos])
                            {
                                result[0] = distance[newPos];
                                isFound = true;
                                break;
                            }
                        }
                    }
                }
            }

            Array.Fill(distance, int.MaxValue);
            q.Clear();
            q.Enqueue(startPos);
            distance[startPos] = 0;
            while (q.Count > 0)
            {
                int curr = q.Dequeue();
                int x = curr / m;
                int y = curr % m;
                for (int i = 0; i < 8; i++)
                {
                    int newX = x + queenDx[i];
                    int newY = y + queenDy[i];
                    while (newX >= 0 && newX < n && newY >= 0 && newY < m && gridMap[newX][newY] != 'x')
                    {
                        int newPos = newX * m + newY;
                        if (distance[newPos] == int.MaxValue)
                        {
                            distance[newPos] = distance[curr] + 1;
                            q.Enqueue(newPos);

                            if (distance[end[0] * m + end[1]] == distance[newPos])
                            {
                                result[1] = distance[newPos];
                                return result;
                            }
                        }
                        newX += queenDx[i];
                        newY += queenDy[i];
                    }
                }
            }
            return result;
        }

        /// задание 5) Жадина
        public static long CalculateMaxCoins(int[][] mapData, int idStart, int idFinish)
        {
            Dictionary<int, City> cities = new Dictionary<int, City>();
            for (var i = 0; i < mapData.Length; i++)
            {
                if (!cities.ContainsKey(mapData[i][0]))
                {
                    cities.Add(mapData[i][0], new City(mapData[i][0], -1));
                }

                if (!cities.ContainsKey(mapData[i][1]))
                {
                    cities.Add(mapData[i][1], new City(mapData[i][1], -1));
                }

                cities[mapData[i][0]].Neighbours.Add(cities[mapData[i][1]]);
                cities[mapData[i][1]].Neighbours.Add(cities[mapData[i][0]]);
                cities[mapData[i][0]].Money.Add(cities[mapData[i][1]], mapData[i][2]);
                cities[mapData[i][1]].Money.Add(cities[mapData[i][0]], mapData[i][2]);
            }

            City startCity = cities[idStart];
            City endCity = cities[idFinish];
            if (startCity == null || endCity == null)
            {
                return -1;
            }

            CheckAllNeighbours(startCity, 0, new Dictionary<City, bool>());
            return cities[idFinish].MaxMoney;
        }

        private static void CheckAllNeighbours(City city, int currentMoney, Dictionary<City, bool> visitedCities)
        {
            visitedCities.Add(city, true);
            if (city.MaxMoney < currentMoney)
            {
                city.MaxMoney = currentMoney;
            }

            foreach (var cityNeighbour in city.Neighbours)
            {
                if (!visitedCities.ContainsKey(cityNeighbour))
                {
                    CheckAllNeighbours(cityNeighbour, currentMoney + city.Money[cityNeighbour], new Dictionary<City, bool>(visitedCities));
                }
            }
        }

        public class City
        {
            private int id;
            private long maxMoney;
            private List<City> neighbours;
            private Dictionary<City, int> money;

            public City(int id, long maxMoney)
            {
                this.id = id;
                this.maxMoney = maxMoney;
                neighbours = new List<City>();
                money = new Dictionary<City, int>();
            }

            public int Id
            {
                get => id;
            }

            public long MaxMoney
            {
                get => maxMoney;
                set => maxMoney = value;
            }

            public List<City> Neighbours
            {
                get => neighbours;
                set => neighbours = value ?? throw new ArgumentNullException(nameof(value));
            }

            public Dictionary<City, int> Money
            {
                get => money;
                set => money = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// Тесты (можно/нужно добавлять свои тесты) 

        private static void TestGenerateWordsFromWord()
        {
            var wordsList = new List<string>
            {
                "кот", "ток", "око", "мимо", "гром", "ром", "мама",
                "рог", "морг", "огр", "мор", "порог", "бра", "раб", "зубр"
            };

            AssertSequenceEqual(GenerateWordsFromWord("арбуз", wordsList), new[] { "бра", "зубр", "раб" });
            AssertSequenceEqual(GenerateWordsFromWord("лист", wordsList), new List<string>());
            AssertSequenceEqual(GenerateWordsFromWord("маг", wordsList), new List<string>());
            AssertSequenceEqual(GenerateWordsFromWord("погром", wordsList), new List<string> { "гром", "мор", "морг", "огр", "порог", "рог", "ром" });
        }

        private static void TestMaxLengthTwoChar()
        {
            AssertEqual(MaxLengthTwoChar("beabeeab"), 5);
            AssertEqual(MaxLengthTwoChar("а"), 0);
            AssertEqual(MaxLengthTwoChar("ab"), 2);
            AssertEqual(MaxLengthTwoChar("baccdaba"), 3);
            AssertEqual(MaxLengthTwoChar("beaeeab"), 0);
            AssertEqual(MaxLengthTwoChar("abcd"), 2);
        }

        private static void TestGetPreviousMaxDigital()
        {
            AssertEqual(GetPreviousMaxDigital(21), 12l);
            AssertEqual(GetPreviousMaxDigital(531), 513l);
            AssertEqual(GetPreviousMaxDigital(1027), -1l);
            AssertEqual(GetPreviousMaxDigital(2071), 2017l);
            AssertEqual(GetPreviousMaxDigital(207034), 204730l);
            AssertEqual(GetPreviousMaxDigital(135), -1l);
            AssertEqual(GetPreviousMaxDigital(9), -1l);
            AssertEqual(GetPreviousMaxDigital(6923), 6392l);
        }

        private static void TestSearchQueenOrHorse()
        {
            char[][] gridA =
            {
                new[] {'s', '#', '#', '#', '#', '#'},
                new[] {'#', 'x', 'x', 'x', 'x', '#'},
                new[] {'#', '#', '#', '#', 'x', '#'},
                new[] {'#', '#', '#', '#', 'x', '#'},
                new[] {'#', '#', '#', '#', '#', 'e'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridA), new[] { 3, 2 });

            char[][] gridB =
            {
                new[] {'s', '#', '#', '#', '#', 'x'},
                new[] {'#', 'x', 'x', 'x', 'x', '#'},
                new[] {'#', 'x', '#', '#', 'x', '#'},
                new[] {'#', '#', '#', '#', 'x', '#'},
                new[] {'x', '#', '#', '#', '#', 'e'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridB), new[] { -1, 3 });

            char[][] gridC =
            {
                new[] {'s', '#', '#', '#', '#', 'x'},
                new[] {'x', 'x', 'x', 'x', 'x', 'x'},
                new[] {'#', '#', '#', '#', 'x', '#'},
                new[] {'#', '#', '#', 'e', 'x', '#'},
                new[] {'x', '#', '#', '#', '#', '#'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridC), new[] { 2, -1 });


            char[][] gridD =
            {
                new[] {'e', '#'},
                new[] {'x', 's'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridD), new[] { -1, 1 });

            char[][] gridE =
            {
                new[] {'e', '#'},
                new[] {'x', 'x'},
                new[] {'#', 's'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridE), new[] { 1, -1 });

            char[][] gridF =
            {
                new[] {'x', '#', '#', 'x'},
                new[] {'#', 'x', 'x', '#'},
                new[] {'#', 'x', '#', 'x'},
                new[] {'e', 'x', 'x', 's'},
                new[] {'#', 'x', 'x', '#'},
                new[] {'x', '#', '#', 'x'},
            };

            AssertSequenceEqual(SearchQueenOrHorse(gridF), new[] { -1, 5 });
        }

        private static void TestCalculateMaxCoins()
        {
            var mapA = new[]
            {
                new []{0, 1, 1},
                new []{0, 2, 4},
                new []{0, 3, 3},
                new []{1, 3, 10},
                new []{2, 3, 6},
            };

            AssertEqual(CalculateMaxCoins(mapA, 0, 3), 11l);

            var mapB = new[]
            {
                new []{0, 1, 1},
                new []{1, 2, 53},
                new []{2, 3, 5},
                new []{5, 4, 10}
            };

            AssertEqual(CalculateMaxCoins(mapB, 0, 5), -1l);

            var mapC = new[]
            {
                new []{0, 1, 1},
                new []{0, 3, 2},
                new []{0, 5, 10},
                new []{1, 2, 3},
                new []{2, 3, 2},
                new []{2, 4, 7},
                new []{3, 5, 3},
                new []{4, 5, 8}
            };

            AssertEqual(CalculateMaxCoins(mapC, 0, 5), 19l);
        }

        /// Тестирующая система, лучше не трогать этот код

        private static void Assert(bool value)
        {
            if (value)
            {
                return;
            }

            throw new Exception("Assertion failed");
        }

        private static void AssertEqual(object value, object expectedValue)
        {
            if (value.Equals(expectedValue))
            {
                return;
            }

            throw new Exception($"Assertion failed expected = {expectedValue} actual = {value}");
        }

        private static void AssertSequenceEqual<T>(IEnumerable<T> value, IEnumerable<T> expectedValue)
        {
            if (ReferenceEquals(value, expectedValue))
            {
                return;
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (expectedValue is null)
            {
                throw new ArgumentNullException(nameof(expectedValue));
            }

            var valueList = value.ToList();
            var expectedValueList = expectedValue.ToList();

            if (valueList.Count != expectedValueList.Count)
            {
                throw new Exception($"Assertion failed expected count = {expectedValueList.Count} actual count = {valueList.Count}");
            }

            for (var i = 0; i < valueList.Count; i++)
            {
                if (!valueList[i].Equals(expectedValueList[i]))
                {
                    throw new Exception($"Assertion failed expected value at {i} = {expectedValueList[i]} actual = {valueList[i]}");
                }
            }
        }

    }

}