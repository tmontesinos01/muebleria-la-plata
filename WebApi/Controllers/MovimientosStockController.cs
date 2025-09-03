using Business.Services;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosStockController : ControllerBase
    {
        private readonly MovimientoStockBusiness _movimientoStockBusiness;

        public MovimientosStockController(MovimientoStockBusiness movimientoStockBusiness)
        {
            _movimientoStockBusiness = movimientoStockBusiness;
        }

    }
}
