using Favly.Application.DTO.DTOs.Response;
using Favly.Application.DTOs;
using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Mappers
{
    public static class CategoryMapper
    {

        public static Category ToEntity(this CategoryDTO request)
        {
            return new Category
            {
               isDeleted = false,
               Name = request.Name,
            };
        }
        public static IEnumerable<CategoryDTO> ToList(this IEnumerable<Category> request)
        {
            return request.Select(x => new CategoryDTO
            { 
                Id = x.Id,
                Name = x.Name,
            }).AsEnumerable();
        }
        public static CategoryDTO ToDTO(this Category request)
        {
            return new CategoryDTO
            {
                Id  = request.Id,
                Name = request.Name,
            };
        }
    }
}
