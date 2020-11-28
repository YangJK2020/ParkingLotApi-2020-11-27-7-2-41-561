﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.DTO;
using ParkingLotApi.Repository;
using ParkingLotApi.Service;
using Xunit;
using System.Linq;

namespace ParkingLotApiTest.ServiceTest
{
    [Collection("ParkingLotTest")]
    public class ParkingLotApiServiceTest : TestBase
    {
        public ParkingLotApiServiceTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_Create_Parkinglot_Success_Via_ParkingLotService()
        {
            //Given
            var context = GetParkingLotDbContext();
            ParkinglotDTO parkinglotDto = new ParkinglotDTO()
            {
                Name = "SuperPark_1",
                Capacity = 10,
                Location = "WuDaoKong"
            };

            ParkingLotApiService parkingLotApiService = new ParkingLotApiService(context);

            //When
            var addedParkinglotID = await parkingLotApiService.AddParkingLotAsnyc(parkinglotDto);
            var returnParkinglot = await parkingLotApiService.GetById(addedParkinglotID);

            //Then
            Assert.Equal(parkinglotDto, returnParkinglot);
        }

        [Fact]
        public async Task Should_Delete_Parkinglot_Success_Via_ParkingLotService()
        {
            //Given
            var context = GetParkingLotDbContext();
            ParkingLotApiService parkingLotApiService = new ParkingLotApiService(context);
            List<ParkinglotDTO> parkingLotDtos = GenerateSomeParkinglots();
            foreach (var parkingLotDto in parkingLotDtos)
            {
                await parkingLotApiService.AddParkingLotAsnyc(parkingLotDto);
            }

            var expectedCount = context.Parkinglots.ToList().Count - 1;

            //When
            await parkingLotApiService.DeleteById(context.Parkinglots.ToList()[0].ID);
            var actualCount = context.Parkinglots.ToList().Count;

            //Then
            Assert.Equal(expectedCount, actualCount);
        }

        private ParkingLotContext GetParkingLotDbContext()
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            ParkingLotContext context = scopedServices.GetRequiredService<ParkingLotContext>();
            return context;
        }

        private List<ParkinglotDTO> GenerateSomeParkinglots()
        {
            ParkinglotDTO parkinglotDto = new ParkinglotDTO()
            {
                Name = "SuperPark_1",
                Capacity = 10,
                Location = "WuDaoKong"
            };

            ParkinglotDTO parkinglotDto1 = new ParkinglotDTO()
            {
                Name = "SuperPark_2",
                Capacity = 10,
                Location = "XiDan"
            };

            ParkinglotDTO parkinglotDto2 = new ParkinglotDTO()
            {
                Name = "SuperPark_3",
                Capacity = 10,
                Location = "XiDan"
            };

            return new List<ParkinglotDTO>() { parkinglotDto, parkinglotDto1, parkinglotDto2 };
        }
    }
}
