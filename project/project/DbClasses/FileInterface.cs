using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DbClasses
{
    public interface FileInterface
    {
        Task<List<Transaction>> DownloadData();
        Task WriteData(List<Transaction> data);
    }
}
