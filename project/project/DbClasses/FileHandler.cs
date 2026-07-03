using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.db
{
    public static class FileHandler
    {
        private static string _currentPath = "";
        /// <summary>
        /// Присваивание пути файла
        /// </summary>
        public static string CurrentPath
        {
            get { return _currentPath; }
            set
            {
                if (File.Exists(value))
                {
                    _currentPath = value;
                } else
                {
                    throw new FileNotFoundException("Некорректный путь");
                }
            }
        }
        /// <summary>
        /// загрузка данных из файла
        /// </summary>
        /// <exception cref="FileNotFoundException">выбрасываем, если пользователь не ввел путь до файла</exception>
        public static async Task<List<Transaction>> DownloadData()
        {
            if (string.IsNullOrEmpty(_currentPath))
            {
                throw new FileNotFoundException("Файл не загружен");
            }

            string[] strings = await File.ReadAllLinesAsync(_currentPath);

            // cоздаем список задач для параллельной обработки строк
            var tasks = new List<Task<Transaction>>();

            for (int i = 0; i < strings.Length; ++i)
            {
                string[] s = strings[i].Split(";");

                if (s.Length == 1 && s[0] == "") { continue; }
                if (s.Length != 9)
                {
                    throw new ArgumentException($"Неверная запись транзакции в строке {i + 1}");
                }

                bool f = uint.TryParse(s[0], out uint id);
                f &= DateTime.TryParse(s[1], out DateTime dt);
                f &= uint.TryParse(s[2], out uint prodId);
                string name = s[3];
                f &= uint.TryParse(s[4], out uint count);
                f &= double.TryParse(s[5], out double price); // цена в рублях из файла
                f &= double.TryParse(s[6], out double priceCur);
                string currency = s[7];
                f &= byte.TryParse(s[8], out byte reg);

                if (!f)
                {
                    throw new ArgumentException($"Неверный формат данных в строке {i + 1}");
                }

                var task = Transaction.CreateAsync(dt, prodId, name, count, priceCur, currency, reg, id, price);
                tasks.Add(task);
            }

            Transaction[] transactions = await Task.WhenAll(tasks);

            return transactions.ToList();
        }
        /// <summary>
        /// Сохранение данных в файл
        /// </summary>
        /// <param name="data">транзакции</param>
        public static void WriteData(List<Transaction> data) { 
            int n = data.Count;
            string[] output = new string[n];
            for (int i = 0; i < n; ++i)
            {
                output[i] = data[i].ToString();
            }
            File.WriteAllLines(_currentPath, output);
        }
    }
}
