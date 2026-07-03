using project.DbClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.db
{
    /// <summary>
    /// класс, который хранится в бд
    /// </summary>
    public class Transaction
    {
        private static uint _newId = 1; // id добавленной транзакции
        private static HashSet<uint> _allId = new HashSet<uint>(); // все id 
        public uint Id { get; private set; } // id транзакции
        private DateTime _date; // дата транзакции
        public DateTime Date {
            get {
                return _date;
            }
            set {
                if (value.Date <= DateTime.Now.Date && value >= DateTime.Parse("1.07.1992")) // проверка, что такая дата существует
                { 
                    _date = value;
                } 
                else
                {
                    throw new ArgumentException("Неверная дата");
                }
            }
        }
        public uint ProdId { get; set; } // id товара
        public string Name { get; set; } // Наименование товара
        public uint Count { get; set; } // Количество
        public double PricePerUnit { get; set; } // Цена за единицу в рублях
        public double PriceInCurrency {  get; set; } // Цена за единицу в валюте
        private string _currency; // код валюты
        public string Currency
        {
            get { return _currency; }
            set
            {
                if (CurrencyConverter.IsValid(value)) // проверка существования кода валюты
                {
                    _currency = value;
                } 
                else
                {
                    throw new ArgumentException("Неверный код валюты");
                }
            }
        }
        private byte _region; // регион транзакции
        public byte Region
        {
            get
            {
                return _region;
            }
            set
            {
                if (value >= 1 && value <= 89) // проверка существования региона
                {
                    _region = value;
                } 
                else
                {
                    throw new ArgumentException("Неверный регион");
                }
            }
        }

        /// <summary>
        /// конструктор класса
        /// </summary>
        /// <param name="date">дата транзакции</param>
        /// <param name="prodId">id товара</param>
        /// <param name="name">наименование товара</param>
        /// <param name="count">количество товаров</param>
        /// <param name="priceInCurrency">цена за шт в валюте</param>
        /// <param name="currency">код валюты</param>
        /// <param name="region">номер региона</param>
        private Transaction(DateTime date, uint prodId, string name, uint count, double priceInCurrency, string currency, double pricePerUnit, byte region, uint id)
        {
            Id = id;
            Date = date;
            ProdId = prodId;
            Name = name;
            Count = count;
            PriceInCurrency = priceInCurrency;
            Currency = currency;
            PricePerUnit = pricePerUnit;
            Region = region;
        }

        /// <summary>
        /// Асинхронное создание новой транзакции с автоматическим расчетом курса в рубли
        /// </summary>
        /// /// <param name="date">дата транзакции</param>
        /// <param name="prodId">id товара</param>
        /// <param name="name">наименование товара</param>
        /// <param name="count">количество товаров</param>
        /// <param name="priceInCurrency">цена за шт в валюте</param>
        /// <param name="currency">код валюты</param>
        /// <param name="region">номер региона</param>
        public static async Task<Transaction> CreateAsync(DateTime date, uint prodId, string name, uint count, double priceInCurrency, string currency, byte region, uint? id = null, double pricePerUnit = -1)
        {
            if (priceInCurrency <= 0)
            {
                throw new ArgumentException("Цена не может быть <= 0");
            }

            pricePerUnit = pricePerUnit == -1 ? await CurrencyConverter.CurToRub(priceInCurrency, currency, date) : pricePerUnit;
            if (id == null)
            {
                id = _newId++;
            } else
            {
                if (_allId.Contains((uint)id))
                {
                    throw new Exception("Не может быть 2 транзакции с одинаковыми id");
                }
                _allId.Add((uint)id);
            }

            // Вызываем приватный конструктор и возвращаем готовый объект
            return new Transaction(date, prodId, name, count, priceInCurrency, currency, pricePerUnit, region, (uint)id);
        }
        public override string ToString()
        {
            return $"{Id};{Date.ToString(new CultureInfo("ru-RU"))[0..10]};{ProdId};" +
                $"{Name};{Count};{PricePerUnit:f2};{PriceInCurrency:f2};{Currency};{Region}";
        }
    }
}
