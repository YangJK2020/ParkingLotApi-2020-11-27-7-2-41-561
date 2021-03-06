﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingLotApi.DTO;
using ParkingLotApi.Service;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingLotApiController : ControllerBase
    {
        private readonly ParkingLotApiService parkingLotService;
        public ParkingLotApiController(ParkingLotApiService parkingLotService)
        {
            this.parkingLotService = parkingLotService;
        }

        [HttpPost("ParkingLots")]
        public async Task<ActionResult<ParkinglotDTO>> CreateParkingLot(ParkinglotDTO parkinglotDto)
        {
            if (parkinglotDto.Name == null || parkinglotDto.Capacity < 0 || parkinglotDto.Location == null
            || parkinglotDto.Name == string.Empty || parkinglotDto.Location == string.Empty)
            {
                return BadRequest(new
                {
                    message = $"Should input all information and input positive capacity."
                });
            }

            var id = await parkingLotService.AddParkingLotAsnyc(parkinglotDto);
            if (id == -1)
            {
                return Conflict(new
                {
                    message = $"An existing parkinglot " +
                                                $"with the name '{parkinglotDto.Name}' was already found."
                });
            }

            return CreatedAtAction(nameof(GetById), new { id = id }, parkinglotDto);
        }

        [HttpGet("ParkingLots/{id}")]
        public async Task<ActionResult<ParkinglotDTO>> GetById(int id)
        {
            var parkinglotDto = await this.parkingLotService.GetById(id);
            return Ok(parkinglotDto);
        }

        [HttpGet("ParkingLots")]
        public async Task<ActionResult<List<ParkinglotDTO>>> GetParkingLot(int? startPage, string name)
        {
            if (startPage.HasValue)
            {
                return Ok(await parkingLotService.GetByPage(startPage.Value));
            }

            if (name != null)
            {
                return Ok(await parkingLotService.GetByName(name));
            }

            return Ok(await parkingLotService.GetAll());
        }

        [HttpDelete("ParkingLots/{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var result = await this.parkingLotService.DeleteById(id);
            if (result == false)
            {
                return NotFound(new
                {
                    message = $"Parkinglot with ID = {id} was not found"
                });
            }

            return this.NoContent();
        }

        [HttpPatch("ParkingLots")]
        public async Task<ActionResult<int>> ChangeCapacity(UpdateModel updateModel)
        {
            return Ok(await parkingLotService.ChangeCapacity(updateModel));
        }

        [HttpPost("Orders")]
        public async Task<ActionResult<OrderDto>> CreateParkingOrder(OrderDto orderDto)
        {
            var id = await parkingLotService.CreateOrder(orderDto);
            if (id == -1)
            {
                return Conflict(new
                {
                    message = "Sorry, the parkinglot is full!"
                });
            }

            return CreatedAtAction(nameof(GetOrderById), new { id = id }, orderDto);
        }

        [HttpGet("Orders/{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var orderDto = await this.parkingLotService.GetOrderById(id);
            return Ok(orderDto);
        }

        [HttpPatch("Orders")]
        public async Task<ActionResult> CloseParkingOrder(OrderDto orderDto)
        {
            var result = await parkingLotService.CloseOrder(orderDto);
            if (result == false)
            {
                return NotFound(new
                {
                    message = "Unrecognized ticket, please provide correct parking ticket"
                });
            }

            return Ok();
        }
    }
}
