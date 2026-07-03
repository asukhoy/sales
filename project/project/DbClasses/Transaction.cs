using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.DbClasses
{
    /// <summary>
    /// Класс транзакции, адаптированный для Entity Framework Core и PostgreSQL
    /// </summary>
    public class Transaction
    {
        private DateTime _date;
        private string _currency = string.Empty;
        private byte _region;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Date
        {
            get => _date;
            set
            {
                if (value.Date <= DateTime.Now.Date && value.Date >= new DateTime(1992, 7, 1))
                {
                    // Если дата пришла без указания зоны (Unspecified), принудительно говорим, что это UTC
                    // Если это локальное время (Local), переводим его в UTC
                    _date = value.Kind == DateTimeKind.Unspecified
                        ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
                        : value.ToUniversalTime();
                }
                else
                {
                    throw new ArgumentException("Неверная дата");
                }
            }
        }

        public int ProdId { get; set; }

        [Required]
        [MaxLength(255)] // Ограничение длины строки в БД
        public string Name { get; set; } = string.Empty;

        public int Count { get; set; }

        public double PricePerUnit { get; set; }

        public double PriceInCurrency { get; set; }

        [Required]
        [MaxLength(3)] // Например, для кодов валют вроде USD, RUB
        public string Currency
        {
            get => _currency;
            set
            {
                // Проверку оставляем, но EF Core при чтении из БД тоже будет её триггерить.
                // Убедитесь, что в БД не попадут невалидные данные.
                if (CurrencyConverter.IsValid(value))
                {
                    _currency = value;
                }
                else
                {
                    throw new ArgumentException("Неверный код валюты");
                }
            }
        }

        public byte Region
        {
            get => _region;
            set
            {
                if (value >= 1 && value <= 89)
                {
                    _region = value;
                }
                else
                {
                    throw new ArgumentException("Неверный регион");
                }
            }
        }
        protected Transaction() { }

        public Transaction(DateTime date, int prodId, string name, int count, double priceInCurrency, string currency, double pricePerUnit, byte region)
        {
            if (priceInCurrency <= 0)
                throw new ArgumentException("Цена не может быть <= 0");

            Date = date;
            ProdId = prodId;
            Name = name;
            Count = count;
            PriceInCurrency = priceInCurrency;
            Currency = currency;
            PricePerUnit = pricePerUnit;
            Region = region;
        }

        public override string ToString()
        {
            return $"{Id};{Date:dd.MM.yyyy};{ProdId};{Name};{Count};{PricePerUnit:f2};{PriceInCurrency:f2};{Currency};{Region}";
        }

        public static async Task<Transaction> CreateAsync(DateTime date, int prodId, string name, int count, double priceInCurrency, string currency, byte region, double price = -1)
        {
            // Считаем цену в рублях асинхронно
            double pricePerUnit = price == -1 ? await CurrencyConverter.CurToRub(priceInCurrency, currency, date) : price;

            return new Transaction(date, prodId, name, count, priceInCurrency, currency, pricePerUnit, region);
        }
    }
}