namespace PetStoreWorkshop.Web.Areas.Administration.Controllers
{
    using PetStoreWorkshop.Common;
    using PetStoreWorkshop.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
