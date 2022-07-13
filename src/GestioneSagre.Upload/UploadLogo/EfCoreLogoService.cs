using GestioneSagre.Business.Services;
using GestioneSagre.Image.Services.Interfaces;
using GestioneSagre.Infrastructure.Entities;
using GestioneSagre.Models.InputModels.Logo;
using GestioneSagre.Upload.UploadLogo.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SequentialGuid;

namespace GestioneSagre.Upload.UploadLogo;

public class EfCoreLogoService : ILogoService
{
    private readonly ILogger<EfCoreLogoService> logger;
    private readonly GestioneSagreDbContext dbContext;
    private readonly IImagePersister imagePersister;

    public EfCoreLogoService(ILogger<EfCoreLogoService> logger, GestioneSagreDbContext dbContext, IImagePersister imagePersister)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.imagePersister = imagePersister;
    }

    public async Task UploadLogoAsync(LogoEditInputModel inputModel)
    {
        int IdFesta = await dbContext.Feste
            .Where(x => x.GuidFesta == inputModel.GuidFesta)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        int IdIntestazione = await dbContext.Intestazioni
            .Where(x => x.FestaId == IdFesta)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        IntestazioneEntity intestazione = await dbContext.Intestazioni.FindAsync(IdIntestazione);

        string imageGuid = SequentialGuidGenerator.Instance.NewGuid().ToString();

        string imagePath = await imagePersister.SaveImageAsync(imageGuid, "jpg", "images", inputModel.Logo);

        intestazione.ChangeLogo(imagePath);

        await dbContext.SaveChangesAsync();
    }
}