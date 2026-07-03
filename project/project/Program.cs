///Сухомлин Артём Владимирович БПИ244-2 В-3
using System;
using System.Text;
using project.ConsoleHandler;
using project.db;
using project.DbClasses;
using project.edit;

public static class Program
{
    public static async Task Main()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // устанавливаем нужную кодировку
        CurrencyConverter.GetAllCurrencyCodes(); // загружаем все возможные валюты
        bool isLoaded = false;
        List<Transaction> transactions = new List<Transaction>();
        ConsoleKeyInfo currKey = new ConsoleKeyInfo('0', ConsoleKey.D0, false, false, false);
        // цикл работы программы
        while (currKey.Key != ConsoleKey.Backspace) {
            if (!isLoaded)
            {
                ConsoleHandler.PrintInitialMenu(); // вывод изначального меню
                currKey = Console.ReadKey();
                Console.Clear();
                switch (currKey.Key)
                {
                    case ConsoleKey.D1:
                        transactions = await ConsoleHandler.DownloadData(); // загрузка данных и её результат
                        isLoaded = transactions.Count != 0;
                        break;
                    case ConsoleKey.Backspace: // выход из программы
                        Console.WriteLine("Работа завершена");
                        break;
                    default:
                        Console.WriteLine("Нажата неверная клавиша");
                        break;
                }
            } else
            {
                ConsoleHandler.PrintMenu();
                currKey = Console.ReadKey(true);
                Console.Clear();
                switch (currKey.Key)
                {
                    case ConsoleKey.D: // загрузка новых данных, в случае неудачи сохраняется старая бд
                        var tmp = await ConsoleHandler.DownloadData();
                        if (tmp.Count != 0)
                        {
                            transactions = tmp;
                        }
                        break;
                    case ConsoleKey.D1: // вывод транзакций
                        ConsoleHandler.PrintInfo(ref transactions); break;
                    case ConsoleKey.D2: // добавление транзакции
                        try {
                            await ConsoleHandler.Add(transactions);
                        } catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case ConsoleKey.D3: // удаление транзакции
                        ConsoleHandler.Delete(ref transactions); break;
                    case ConsoleKey.D4: // изменение транзакции
                        await ConsoleHandler.Edit(transactions); break;
                    case ConsoleKey.D5: // вывод информации по регионам
                        ConsoleHandler.PrintRegionInfo(transactions); break;
                    case ConsoleKey.D6: // вывод суммы всех транзакций
                        await ConsoleHandler.PrintAllSales(transactions); break;
                    case ConsoleKey.D7: // вывод ABC анализа
                        ConsoleHandler.PrintABCAnalysis(ref transactions); break;
                    case ConsoleKey.D8: // вывод XYZ анализа
                        ConsoleHandler.PrintXYZAnalysis(ref transactions); break;
                    case ConsoleKey.D9: // вывод прогноза
                        ConsoleHandler.PrintForecast(ref transactions); break;
                    case ConsoleKey.S: // сохранение данных
                        FileHandler.WriteData(transactions);
                        Console.WriteLine("Данные успешно сохранены");
                        break;
                    case ConsoleKey.Backspace: // выход из программы
                        Console.WriteLine("Работа завершена");
                        break;
                    default:
                        Console.WriteLine("Неверная кнопка");
                        break;
                }
            }
        }
    }
}
