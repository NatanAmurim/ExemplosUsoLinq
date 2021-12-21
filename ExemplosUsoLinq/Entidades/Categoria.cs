using System;
using System.Collections.Generic;
using System.Text;

namespace ExemplosUsoLinq.Entidades
{
    class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Tier Tier { get; set; }
    }
}
