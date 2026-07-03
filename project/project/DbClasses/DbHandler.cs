using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace project.DbClasses
{
    public class DbHandler : FileInterface
    {
        private AppDbContext CreateContext()
        {
            return new AppDbContext();
        }

        public async Task<List<Transaction>> DownloadData()
        {
            using var db = CreateContext();

            return await db.Transactions
                           .AsNoTracking()
                           .OrderBy(t => t.Id)
                           .ToListAsync();
        }

        /// <summary>
        /// Полностью асинхронное сохранение/перезапись данных в PostgreSQL
        /// </summary>
        public async Task WriteData(List<Transaction> data)
        {
            if (data == null) return;

            using var db = CreateContext();

            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                string tableName = "Transactions";

                // Очищаем таблицу и сбрасываем счетчик ID асинхронно
                await db.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{tableName}\" RESTART IDENTITY CASCADE;");

                // Добавляем данные в контекст
                await db.Transactions.AddRangeAsync(data);

                // Асинхронно сохраняем в базу данных (генерирует пачку INSERT запросов)
                await db.SaveChangesAsync();

                // Коммитим данные
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // При любой ошибке асинхронно откатываем базу к исходному состоянию
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}