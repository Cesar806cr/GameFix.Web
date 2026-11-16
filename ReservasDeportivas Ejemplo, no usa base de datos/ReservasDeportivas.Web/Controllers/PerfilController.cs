using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ReservasDeportivas.Web.Controllers;

public class PerfilController : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cambiar()
    {
        var perfil = HttpContext.Session.GetString("Perfil")
            ?? "Cliente";

        var nuevoPerfil = perfil == "Cliente"
            ? "Administrador"
            : "Cliente";

        HttpContext.Session.SetString("Perfil", nuevoPerfil);

        if (nuevoPerfil == "Administrador")
        {
            return RedirectToAction("Index", "Admin");
        }

        return RedirectToAction("Index", "Reservas");
    }
}