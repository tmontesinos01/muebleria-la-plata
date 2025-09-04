using Business.Interfaces;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovimientosStockController : ControllerBase
    {
        private readonly IMovimientoStockBusiness _movimientoStockBusiness;

        public MovimientosStockController(IMovimientoStockBusiness movimientoStockBusiness)
        {
            _movimientoStockBusiness = movimientoStockBusiness;
        }

    }
}
