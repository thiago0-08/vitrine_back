using Database;
using Vitrine.DTO;
using Vitrine.Model;

namespace Vitrine.Endpoints
{
    public static class Produtos
    {
        public static void RegistrarEndpointsProduto(this IEndpointRouteBuilder rotas)
        {
            RouteGroupBuilder rotaProdutos = rotas.MapGroup("/produtos");

            // Define a rota para produtos por categoria
            RouteGroupBuilder rotaProdutosPorCategoria = rotas.MapGroup("/produtosPorCategoria");

            rotaProdutos.MapGet("/", (VitrineDbContext contexto, string? nome, int pagina = 1, int tamanhoPagina = 10) =>
            {
                IQueryable<Produto> produtosFiltrados = contexto.Produtos;

                if (!string.IsNullOrEmpty(nome))
                {
                    produtosFiltrados = produtosFiltrados.Where(p => p.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
                }

                var totalProdutos = produtosFiltrados.Count();
                var produtosPaginados = produtosFiltrados.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();

                var resultadoPaginado = new
                {
                    TotalItems = totalProdutos,
                    Page = pagina,
                    PageSize = tamanhoPagina,
                    Products = produtosPaginados
                };

                return TypedResults.Ok(resultadoPaginado);
            });

            rotaProdutos.MapGet("/{id}", (VitrineDbContext contexto, int id) =>
            {
                Produto produtoFiltrado = contexto.Produtos.Find(id);

                if (produtoFiltrado == null)
                {
                    return Results.NotFound();
                }

                return TypedResults.Ok(produtoFiltrado);
            });

            rotaProdutos.MapPost("/", (VitrineDbContext contexto, ProdutoDTO produto) =>
            {
                // Buscar a categoria com base no produto.IdCategoria
                Categoria categoria = contexto.Categorias.Find(produto.IdCategoria);

                // Verificar se a categoria existe
                if (categoria == null)
                {
                    return Results.NotFound("Categoria não encontrada");
                }

                // Insere a categoria encontrada no produto
                Produto novoProduto = new Produto
                {
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    Preco = produto.Preco,
                    Tamanhos = produto.Tamanhos,
                    Cores = produto.Cores,
                    Imagem = produto.Imagem,
                    Categoria = categoria // atribuindo a categoria ao produto
                };

                contexto.Produtos.Add(novoProduto);
                contexto.SaveChanges();

                return TypedResults.Created($"/produtos/{novoProduto.Id}", produto);
            });


            rotaProdutos.MapPut("/{Id}", (VitrineDbContext contexto, int Id, Produto produtoAtualizado) =>
            {
                Produto produtoExistente = contexto.Produtos.Find(Id);

                if (produtoExistente == null)
                {
                    return Results.NotFound();
                }

                // Atualize as propriedades do produto existente
                produtoExistente.Nome = produtoAtualizado.Nome;
                produtoExistente.Descricao = produtoAtualizado.Descricao;
                // Atualize outras propriedades conforme necessário

                contexto.SaveChanges();

                return Results.NoContent();
            });

            rotaProdutos.MapDelete("/{Id}", (VitrineDbContext contexto, int Id) =>
            {
                Produto produto = contexto.Produtos.Find(Id);

                if (produto == null)
                {
                    return Results.NotFound();
                }

                contexto.Produtos.Remove(produto);
                contexto.SaveChanges();

                return Results.NoContent();
            });

            rotaProdutosPorCategoria.MapGet("/{IdCategoria}", (VitrineDbContext contexto, int id) =>
            {
                List<Produto> produtoCategoriaFiltrado = contexto.Produtos
                     .Where(p => p.Categoria.Id == id)
                     .ToList();
                if (produtoCategoriaFiltrado == null || produtoCategoriaFiltrado.Count <= 0)
                {
                    return Results.NotFound();
                }

                return TypedResults.Ok(produtoCategoriaFiltrado);
            });

        }
    }
}
