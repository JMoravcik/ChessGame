using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Common.Data;

public class ListResponse<TData> : Response<List<TData>>
    where TData : class
{
    public int CurrentPage { get; set; } = 1;
    public int PageCount { get; set; } = 1;
}
