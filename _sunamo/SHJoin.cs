using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoCollectionOnDrive._sunamo;
internal class SHJoin
{
    internal static string JoinNL<T>(List<T> list)
    {
        var ts = list.ConvertAll(d => d.ToString());
        return string.Join("\n", ts);
    }
}
