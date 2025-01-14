﻿using Final_Project_Api.Data;
using Final_Project_Api.Data.Enums;
using Final_Project_Api.Data.Models;
using Final_Project_Api.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly AppDbContext dbContext;

        public BookingController(IBookingService bookingService, AppDbContext dbContext)
        {
            _bookingService = bookingService;
            this.dbContext = dbContext;
        }



        [HttpGet("BookingCount")]
        public async Task<IActionResult> BookingCount(string patientId)
        {
            try
            {
                var count = await _bookingService.BookingCountAsync(patientId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }





        [HttpGet("GetAllBookingsByDocId")]
        public async Task<IActionResult> GetAllBookingsByDocId(string doctorId)
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsByDocIdAsync(doctorId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        [HttpGet("GetAllBookingsByPatientId")]
        public async Task<IActionResult> GetAllBookingsByPatientId(string patientId)
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsByPatientIdAsync(patientId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        [HttpGet("SearchBookingsByDocId")]
        public async Task<IActionResult> SearchBookingsByDocId(string doctorId)
        {
            try
            {
                var bookings = await _bookingService.SearchBookingsByDocIdAsync(doctorId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        [HttpGet("SearchBookingsByPatientId")]
        public async Task<IActionResult> SearchBookingsByPatientId(string patientId)
        {
            try
            {
                var bookings = await _bookingService.SearchBookingsByPatientIdAsync(patientId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        [HttpPost("AddBooking")]
        public async Task<IActionResult> AddBooking(string patientId, int appointmentId)
        {
            try
            {
                var patient = dbContext.Patients.Find(patientId);
                var appointment = dbContext.Appointments.Find(appointmentId);

                if (patient != null && appointment != null)
                {
                    var newBooking = new Booking
                    {
                        Status = BookingStatusEnum.Pending,
                        TotalBookings = patient.TotalBookings + 1,
                        Price =  appointment.Price,
                        AppointmentId = appointmentId,
                        PatientId = patientId,
                        Patient = patient,
                        Appointment = appointment,
                    };

                    dbContext.Bookings.Add(newBooking);
                    appointment.booked = true;

                    dbContext.SaveChanges();

                    return Ok(newBooking);
                }

                return NotFound("Patient or appointment not found");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        [HttpPut("CancelBooking")]
        public async Task<IActionResult> CancelBooking(int bookingId, string doctorId)
        {
            try
            {
                var result = await _bookingService.CancelBookingAsync(bookingId, doctorId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


        [HttpPut("ConfirmBooking")]
        public async Task<IActionResult> ConfirmBooking(int bookingId, string docID)
        {
            try
            {
                var result = await _bookingService.ConfirmBookingAsync(bookingId, docID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


       
    }
}

