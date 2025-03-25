using AutoMapper;
using DevIO.Api.DTOs;
using DevIO.Business.Models;

namespace DevIO.Api.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, FornecedorDto>().ReverseMap();
            CreateMap<Endereco, EnderecoDto>().ReverseMap();
            CreateMap<ProdutoDto, Produto>().ReverseMap();

            CreateMap<Produto, ProdutoDto>()
                .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome));
        }
    }
}
