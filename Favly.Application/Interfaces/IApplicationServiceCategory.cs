using Favly.Application.DTO.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Interfaces
{
    public interface IApplicationServiceCategory
    {
        void Add(CategoryDTO obj);

        CategoryDTO GetById(int id);

        IEnumerable<CategoryDTO> GetAll();

        void Update(CategoryDTO obj);

        void Remove(CategoryDTO obj);

        void Dispose();

    }
}
