using AutoMapper;
using DevIO.Api.DTOs;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public ProdutosController(INotificador notificador, 
            IProdutoRepository produtoRepository, 
            IProdutoService produtoService, 
            IMapper mapper) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProdutoDto>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoDto>>(await _produtoRepository.ObterProdutosFornecedores());;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoDto>> ObterPorId(Guid id)
        {
            var produto = await ObterProduto(id);
            if (produto == null) return NotFound();
            return produto;
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDto>> Adicionar(ProdutoDto produtoDto)
        {
            if (!ModelState.IsValid) return CustumResponse(ModelState);

            var imagemNome = Guid.NewGuid() + "_" + produtoDto.Imagem;
            if (!UploadArquivo(produtoDto.ImagemUpload, imagemNome)) return CustumResponse(produtoDto);

            produtoDto.Imagem = imagemNome;

            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoDto));
            return CustumResponse(produtoDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProdutoDto>> Excluir(Guid id)
        {
            var produto = await ObterProduto(id);
            if (produto == null) return NotFound();
            await _produtoService.Remover(id);
            return CustumResponse(produto);
        }

        private async Task<ProdutoDto> ObterProduto(Guid id)
        {
            return _mapper.Map<ProdutoDto>(await _produtoRepository.ObterProdutoFornecedor(id));
        }

        private bool UploadArquivo(string arquivo, string imgNome)
        {
            if (string.IsNullOrEmpty(arquivo))
            {
                NotificarErro("Forneça uma imagem para este produto!");
                return false;
            }
            var imageDataByteArray = Convert.FromBase64String(arquivo);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgNome);
            if (System.IO.File.Exists(filePath))
            {
                NotificarErro("Já existe um arquivo com este nome!");
                return false;
            }
            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);
            return true;
        }

    }
}
