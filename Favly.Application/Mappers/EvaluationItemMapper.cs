using Favly.Application.DTO.DTOs.Response;
using Favly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Mappers
{
    public static class EvaluationItemMapper
    {
        public static EvaluationItem ToEntity(this EvaluationItemDTO request)
        {
            return new EvaluationItem
            {
                Description = request.Description,
                ExternalId = request.ExternalId,
                ImageUrl = request.ImageUrl,
                Name = request.Name,
                SecondaryInfo = request.SecondaryInfo,
                LastSync = request.LastSync,
            };
        }
        public static IEnumerable<EvaluationItemDTO> ToList(this IEnumerable<EvaluationItem> request)
        {
            return request.Select(x => new EvaluationItemDTO
            {
                Description = x.Description,
                ExternalId = x.ExternalId,  
                ImageUrl = x.ImageUrl,
                Name = x.Name,
                SecondaryInfo = x.SecondaryInfo,
            }).AsEnumerable();
        }

        public static EvaluationItemDTO ToDTO(this EvaluationItem request)
        {
            return new EvaluationItemDTO
            {
                Description = request.Description,
                ExternalId = request.ExternalId,
                ImageUrl = request.ImageUrl,
                Name = request.Name,
                SecondaryInfo = request.SecondaryInfo
            };
        }
    }
}
