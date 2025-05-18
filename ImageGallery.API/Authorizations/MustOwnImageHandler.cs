using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace ImageGallery.API.Authorizations
{
    public class MustOwnImageHandler : AuthorizationHandler<MustOwnImageRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGalleryRepository _galleryRepository;

        public MustOwnImageHandler(IGalleryRepository galleryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _galleryRepository = galleryRepository ??
                throw new ArgumentNullException(nameof(galleryRepository));
            _httpContextAccessor = httpContextAccessor ??
                throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            MustOwnImageRequirement requirement)
        {
            var imageId = _httpContextAccessor.HttpContext?.GetRouteValue("id")?.ToString();

            if (!Guid.TryParse(imageId, out Guid imageIdAsGuid))
            {
                context.Fail();
                return; 
            }

            var ownerId = context.User.Claims
                .FirstOrDefault(c => c.Type == "sub")?.Value;
            if (ownerId == null)
            {
                context.Fail();
                return;
            }

            if (!await _galleryRepository.IsImageOwnerAsync(imageIdAsGuid, ownerId))
            {
                context.Fail();
                return;
            }
            
            context.Succeed(requirement);

        }
    }

}