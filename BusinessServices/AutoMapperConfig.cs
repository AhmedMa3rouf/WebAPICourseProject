using AutoMapper;
using BusinessEntities;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public static class AutoMapperConfig
    {
        public static void CreateMapping()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Product, ProductEntity>();
            });
        }
    }
}
