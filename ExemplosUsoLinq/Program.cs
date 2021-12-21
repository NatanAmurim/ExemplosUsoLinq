using ExemplosUsoLinq.Entidades;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ExemplosUsoLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            Categoria ferramentas = new Categoria { Id = 1, Nome = "Ferramentas", Tier = Tier.Media };
            Categoria computadores = new Categoria { Id = 2, Nome = "Computadores", Tier = Tier.Alta };
            Categoria eletronicos = new Categoria { Id = 3, Nome = "Eletronico", Tier = Tier.Alta };

            List<Produto> produtos = new List<Produto>()
            {
                new Produto { Id = 1, Nome = "Notebook", Categoria = eletronicos, Preco = 5000 },
                new Produto { Id = 2, Nome = "Martelo", Categoria = ferramentas, Preco = 100 },
                new Produto { Id = 3, Nome = "TV", Categoria = eletronicos , Preco = 2500 },
                new Produto { Id = 4, Nome = "Computador", Categoria = computadores, Preco = 5000 },
                new Produto { Id = 5, Nome = "Prego", Categoria = ferramentas, Preco = 0.5 },
                new Produto { Id = 6, Nome = "Chave de fenda", Categoria = ferramentas, Preco = 80 },
                new Produto { Id = 7, Nome = "Tablet", Categoria = eletronicos, Preco = 900 },
                new Produto { Id = 8, Nome = "Impressora", Categoria = eletronicos, Preco = 300 },
                new Produto { Id = 9, Nome = "Nivel", Categoria = ferramentas, Preco = 50 },
                new Produto { Id = 10, Nome = "Celular", Categoria = eletronicos, Preco = 1000 },
                new Produto { Id = 11, Nome = "Camera", Categoria = eletronicos, Preco = 900 }
            };
            //LinqMetodosExtensao(produtos);
            LinqExpressaoSQL(produtos);
        }

        private static void LinqMetodosExtensao(List<Produto> produtos)
        {
            //Where - Selecionando apenas os produtos de categoria Alta e que custam mais que 900 
            produtos.Where(p => p.Categoria.Tier == Tier.Alta && p.Preco > 900).Imprimir();

            //Select - Projetando apenas o nome das ferramentas
            produtos.Where(p => p.Categoria.Id == 1)
                .Select(p => p.Nome)
                .Imprimir();

            //Where - Projetando alguns campos de um produto gerando um objeto anonimo
            produtos.Where(p => p.Nome.StartsWith("M"))
                .Select(p => new { Nome = p.Nome, Preco = p.Preco, Categoria = p.Categoria.Nome })
                .Imprimir();

            //OrderBy - Ordenando por preços e por nome
            produtos.Where(p => p.Categoria.Tier == Tier.Alta)
                .OrderBy(p => p.Preco)
                .ThenBy(p => p.Nome)
                .Imprimir();

            //Skip e take - Pulando o primeiro elemento e pegando os próximos 3.
            produtos.Skip(2).Take(3).Imprimir();

            //First - Pegando o primeiro elemento da lista (Em caso de lista vazia, uma exceção é lançada)
            var produto = produtos.First();
            Console.WriteLine(produto.ToString());

            //FirstOrDefault - Tentando pegar o primeiro elemento de uma lista vazia, por padrão o retorno é nulo.
            var produtoNulo = produtos.Where(p => p.Preco > 5500).FirstOrDefault();
            Console.WriteLine($"Produto: {produtoNulo}");

            //Max e singleOrDefault - Selecionando o produto de maior valor e, como o resultado será um único, utilizando o método para retornar um produto e não uma lista.  
            var max = produtos.Where(p => p.Preco == produtos.Max(p => p.Preco)).SingleOrDefault();
            Console.WriteLine($"Produto: {max}");

            //Sum - Somando preços de todos os produtos de uma categoria
            var valorTotal = produtos.Where(p => p.Categoria.Tier == Tier.Alta).Sum(p => p.Preco);
            Console.WriteLine($"Valor total: {valorTotal}");

            //Average - Calculando a média de preço da uma categoria. Se o número de itens for 0, uma exceção será lançada
            var valorMedio = produtos.Where(p => p.Categoria.Tier == Tier.Alta).Average(p => p.Preco);
            Console.WriteLine($"Valor média: {valorMedio}");

            //Average - Calculando a média de preço da uma categoria. Com proteção contra exceção.
            var valorMedioProtegido = produtos.Where(p => p.Categoria.Tier == Tier.Baixa).Select(p => p.Preco).DefaultIfEmpty(0).Average();
            Console.WriteLine($"Média: {valorMedioProtegido}");

            //Aggregate - Utilizando o agregate para criar nossa própria função
            var valorMultiplicado = produtos.Where(p => p.Categoria.Tier == Tier.Alta).Select(p => p.Preco).Aggregate(1.0, (x, y) => x * y);
            Console.WriteLine($"Preços multiplicados: {valorMultiplicado}");

            //GroupBy - Agrupando por categoria
            var listaCategorias = produtos.GroupBy(p => p.Categoria);

            foreach (IGrouping<Categoria, Produto> categoria in listaCategorias)
            {
                Console.WriteLine($"Categoria: {categoria.Key.Nome}");

                foreach (Produto prod in categoria)
                {
                    Console.WriteLine(prod);
                }
                Console.WriteLine();
            }
        }
        private static void LinqExpressaoSQL(List<Produto> produtos)
        {
            //Where - Selecionando apenas os produtos de categoria Alta e que custam mais que 900            
            var prod1 = from p in produtos
                       where p.Categoria.Tier == Tier.Alta && p.Preco > 900
                       select p;
            prod1.Imprimir();

            //Select - Projetando apenas o nome das ferramentas            
            
            var prod2 = from p in produtos
                        where p.Categoria.Id == 1
                        select p.Nome;
            prod2.Imprimir();

            ////Where - Projetando alguns campos de um produto gerando um objeto anonimo
            var prod3 = from p in produtos
                        where p.Nome.StartsWith("M")
                        select new { p.Nome, p.Preco, categoria = p.Categoria.Nome };
            prod3.Imprimir();

            ////OrderBy - Ordenando por preços e por nome
            //produtos.Where(p => p.Categoria.Tier == Tier.Alta)
            //    .OrderBy(p => p.Preco)
            //    .ThenBy(p => p.Nome)
            //    .Imprimir();
            var prod4 = from p in produtos
                        where p.Categoria.Tier == Tier.Alta
                        orderby p.Preco, p.Nome                        
                        select p;
            prod4.Imprimir();

            ////Skip e take - Pulando o primeiro elemento e pegando os próximos 3.
            //produtos.Skip(2).Take(3).Imprimir();
            var prod5 = (from p in produtos
                         select p).Skip(2).Take(3);
            prod5.Imprimir();

            //First - Pegando o primeiro elemento da lista (Em caso de lista vazia, uma exceção é lançada)
            var prod6 = (from p in produtos
                         select p).First();
            Console.WriteLine(prod6.ToString());


            var listadoPorCategoria = from p in produtos
                        group p by p.Categoria;

            foreach (IGrouping<Categoria, Produto> categoria in listadoPorCategoria)
            {
                Console.WriteLine($"Categoria: {categoria.Key.Nome}");

                foreach (Produto prod in categoria)
                {
                    Console.WriteLine(prod);
                }
                Console.WriteLine();
            }
        }
    }
}
