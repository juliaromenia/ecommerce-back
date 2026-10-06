using ECommerce.Application.DTOs.Categories;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
            });
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            
            if (category == null) return null;
            
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public async Task<CategoryDto> AddCategoryAsync(CreateCategoryDto categoryDto)
        {
            if(string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }

            var category = new Domain.Entities.Category
            {
                Name = categoryDto.Name,
            };

            await _categoryRepository.AddCategoryAsync(category);

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int id, string Name)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            
            if (category == null) return null;
            
            if(string.IsNullOrWhiteSpace(Name))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }
           
            category.Name = Name;
            
            await _categoryRepository.UpdateCategoryAsync(category);
            
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public async Task<string?> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            
            if (category == null) return null;
            
            await _categoryRepository.DeleteCategoryAsync(category);
            
            return $"Category with ID {id} has been deleted.";
        }
    }
}
