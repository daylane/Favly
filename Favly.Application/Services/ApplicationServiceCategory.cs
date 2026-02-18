using Favly.Application.DTO.DTOs.Response;
using Favly.Application.Interfaces;
using Favly.Application.Mappers;
using Favly.Domain.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Services
{
    public class ApplicationServiceCategory(IServiceCategory _serviceCategory) : IApplicationServiceCategory
    {
        public void Add(CategoryDTO obj)
        {
            var objCategory = CategoryMapper.ToEntity(obj);
            _serviceCategory.Add(objCategory);
        }

        public void Dispose()
        {
            _serviceCategory.Dispose();
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            var objCategory = _serviceCategory.GetAll();
            return CategoryMapper.ToList(objCategory);
        }

        public CategoryDTO GetById(int id)
        {
            var objCategory = _serviceCategory.GetById(id);
            return CategoryMapper.ToDTO(objCategory);
        }

        public void Remove(CategoryDTO obj)
        {
            var objCategory = CategoryMapper.ToEntity(obj);
            _serviceCategory.Remove(objCategory);
        }

        public void Update(CategoryDTO obj)
        {
            var objCategory = CategoryMapper.ToEntity(obj);
            _serviceCategory.Update(objCategory);
        }
    }
}
