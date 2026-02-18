using Favly.Application.DTO.DTOs.Response;
using Favly.Application.Interfaces;
using Favly.Application.Mappers;
using Favly.Domain.Core.Interfaces.Services;
using Favly.Domain.Services.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Favly.Application.Services
{
    public class ApplicationServiceEvaluationItem(IServiceItem _serviceItem) : IApplicationServiceEvaluationItem
    {
        public void Add(EvaluationItemDTO obj)
        {
            var objItem = EvaluationItemMapper.ToEntity(obj);
            _serviceItem.Add(objItem);
        }

        public void Dispose()
        {
            _serviceItem.Dispose();
        }

        public IEnumerable<EvaluationItemDTO> GetAll()
        {
            var objItem = _serviceItem.GetAll();
            return EvaluationItemMapper.ToList(objItem);
        }

        public EvaluationItemDTO GetById(int id)
        {
            var objItem = _serviceItem.GetById(id);
            return EvaluationItemMapper.ToDTO(objItem);
        }

        public void Remove(EvaluationItemDTO obj)
        {
            var objItem = EvaluationItemMapper.ToEntity(obj);
            _serviceItem.Remove(objItem);
        }

        public void Update(EvaluationItemDTO obj)
        {
            var objItem = EvaluationItemMapper.ToEntity(obj);
            _serviceItem.Update(objItem);
        }
    }
}
