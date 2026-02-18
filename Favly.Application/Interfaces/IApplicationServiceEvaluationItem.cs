using Favly.Application.DTO.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Interfaces
{
    public interface IApplicationServiceEvaluationItem
    {
        void Add(EvaluationItemDTO obj);

        EvaluationItemDTO GetById(int id);

        IEnumerable<EvaluationItemDTO> GetAll();

        void Update(EvaluationItemDTO obj);

        void Remove(EvaluationItemDTO obj);

        void Dispose();

    }
}
