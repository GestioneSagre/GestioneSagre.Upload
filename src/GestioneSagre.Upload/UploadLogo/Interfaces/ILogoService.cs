using GestioneSagre.Models.InputModels.Logo;

namespace GestioneSagre.Upload.UploadLogo.Interfaces;

public interface ILogoService
{
    public Task UploadLogoAsync(LogoEditInputModel inputModel);
}
