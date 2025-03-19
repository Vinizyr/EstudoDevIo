using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        // validacao de notificacoes de erro

        // validacao de modelstate

        //validacao de operacao de negócios
    }

    [Route("api/[controller]")]
    public class FornecedoresController : MainController
    {

    }
}
