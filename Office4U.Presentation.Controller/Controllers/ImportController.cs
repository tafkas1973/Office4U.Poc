using Microsoft.AspNetCore.Authorization;

namespace Office4U.Presentation.Controller.Controllers
{
    [Authorize(Policy = "RequireImportArticlesRole")]
    public class ImportController: BaseApiController
    {
        
    }
}