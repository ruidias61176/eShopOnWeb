using Microsoft.eShopWeb.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.eShopWeb.Web.ViewModels;
using System;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.eShopWeb.ApplicationCore.Constants;

namespace Microsoft.eShopWeb.Web.Controllers.Api
{
    [Authorize(Roles = AuthorizationConstants.Roles.ADMINISTRATORS)]
    public class BrandController : BaseApiController
    {
        private readonly IAsyncRepository<CatalogBrand> _brandRepository;

        public BrandController(IAsyncRepository<CatalogBrand> brandRepository) => _brandRepository = brandRepository;

        //CREATE
        [HttpPost]
        public async Task<ActionResult<CatalogBrand>> CreateBrand([FromForm]string newBrand, [FromBody]int id)
        {
            if (newBrand == null)
            {
                return NoContent();
            }
            try
            {
                var brandsNames = await _brandRepository.ListAllAsync();
                var brandToMatch = brandsNames.Where(x => string.Compare(x.Brand, newBrand, StringComparison.OrdinalIgnoreCase) == 0);
                if (brandToMatch != null)
                {
                    return Forbid();
                }
                return BadRequest();
            }
            catch
            {
                CatalogBrand brandToAdd = new CatalogBrand()
                {
                    Id = id,
                    Brand = newBrand
                };
                await _brandRepository.AddAsync(brandToAdd);
                await _brandRepository.UpdateAsync(brandToAdd);

                return Ok(brandToAdd);
            }
        }

        //RETRIEVE
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<CatalogBrand>> ListBrands()
        {
            var brandsList = await _brandRepository.ListAllAsync();
            return Ok(brandsList);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<CatalogBrand>> GetByBrandId(int id)
        {
            try
            {
                var brandId = await _brandRepository.GetByIdAsync(id);
                return Ok(brandId);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
        [AllowAnonymous]
        [HttpGet("{name}")]
        public async Task<ActionResult<CatalogBrand>> GetByBrandName(string name)
        {
            try
            {
                var brandsNames = await _brandRepository.ListAllAsync();
                var brandsList = brandsNames.Where(x => string.Compare(x.Brand, name, StringComparison.OrdinalIgnoreCase) == 0);
                return Ok(brandsList);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        //UPDATE
        [HttpPut]
        public async Task<ActionResult<IReadOnlyList<CatalogBrand>>> UpdateBrand(CatalogBrand catalogBrand)
        {
            try
            {
                var brandsNames = await _brandRepository.ListAllAsync();
                var brandsList = brandsNames
                        .Where(x => string.Compare(x.Brand, catalogBrand.Brand, StringComparison.OrdinalIgnoreCase) == 0);
                if (brandsList != null)
                {
                    return Forbid();
                }

                await _brandRepository.UpdateAsync(catalogBrand);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<IReadOnlyList<CatalogBrand>>> DeleteBrand([FromForm]int id)
        {
            try
            {
                var brand = await _brandRepository.GetByIdAsync(id);
                if (brand == null) { return NotFound(); }

                await _brandRepository.DeleteAsync(brand);
                return Ok(brand);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}