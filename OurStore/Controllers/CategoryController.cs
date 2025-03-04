using AutoMapper;
using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OurStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        ICategoryService categoryService;
        IMapper mapper;
        IMemoryCache cache;
        public CategoryController(ICategoryService _categoryService,IMapper _mapper, IMemoryCache _cache)
        {
            categoryService = _categoryService;
            mapper = _mapper;
            cache= _cache;
        }

// GET: api/<CategoryController>
[HttpGet]
public async Task<ActionResult<List<Category>>> Get()
{
    if (!cache.TryGetValue("categories", out List<Category> categories))
      {
             categories = await categoryService.getAll();
             cache.Set("categories", categories, TimeSpan.FromMinutes(30));
     }
    List<CategoryDTO> categoryDTOList = mapper.Map<List<Category>, List<CategoryDTO>>(categories);
    return categoryDTOList != null ? Ok(categoryDTOList) : BadRequest();
       }
    }
}
