﻿using Linq.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IProductRepository
    {
        public List<Product> GetAllProduct(ProductFilter filter);
    }
}