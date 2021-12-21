using System.Linq;

namespace System.Collections.Generic
{
    public static class MetodosExtensaoIEnumerable
    {
        public static void Imprimir<T>(this IEnumerable<T> lista)
        {
            lista.ToList()
                .ForEach(obj => Console.WriteLine(obj));
            Console.WriteLine("");
        }
    }
}
