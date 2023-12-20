using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_adminlogin.Data;
using user_adminlogin.Models;
using System;
using System.Data;
using Microsoft.Data.SqlClient; // Add this for SQL Server
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AddFlightController : Controller
{
    private readonly ApplicationDbContext _context;

    public AddFlightController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var flights = _context.Flights.ToList();
        return View(flights);
    }

    [HttpGet]
    public IActionResult AddFlight()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddFlight(Flight flight)
    {
        if (!ModelState.IsValid)
        {
            try
            {
                ExecuteAddFlightProcedure(flight);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while adding the flight.");
            }
        }

        return View("Index", flight);
    }

    [HttpGet]
    public IActionResult UpdateFlight(int id)
    {
        var flight = _context.Flights.Find(id);
        if (flight == null)
        {
            return NotFound();
        }

        return View(flight);
    }
    [HttpPost]
    [HttpPost]
    public IActionResult UpdateFlight(Flight updatedFlight)
    {
        if (!ModelState.IsValid)
        {
            try
            {
                _context.Database.ExecuteSqlInterpolated($"EXEC UpdateFlightProcedure {updatedFlight.Id}, {updatedFlight.flight_Name}, {updatedFlight.Departure}, {updatedFlight.Destenation}, {updatedFlight.Arrival_time}, {updatedFlight.Departure_time}, {updatedFlight.flight_date}");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                // For now, let's return an error view with the exception details
                return RedirectToAction("Index");
            }
        }

        // If ModelState is not valid, return back to the form with validation errors
        return View(updatedFlight);
    }


    [HttpGet]
    public IActionResult DeleteFlight(int id)
    {
        var flight = _context.Flights.Find(id);
        if (flight == null)
        {
            return NotFound();
        }

        return View(flight);
    }

    [HttpPost, ActionName("DeleteFlight")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteFlightConfirmed(Flight flight)
    {
        var flightToDelete = _context.Flights.Find(flight.Id);
        if (flightToDelete == null)
        {
            return NotFound();
        }

        ExecuteDeleteFlightProcedure(flightToDelete.Id);
        return RedirectToAction("Index");
    }

    // Helper methods to execute stored procedures

    private void ExecuteAddFlightProcedure(Flight flight)
    {
        using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("AddFlightProcedure", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FlightName", flight.flight_Name);
                command.Parameters.AddWithValue("@Departure", flight.Departure);
                command.Parameters.AddWithValue("@Destination", flight.Destenation);
                command.Parameters.AddWithValue("@ArrivalTime", flight.Arrival_time);
                command.Parameters.AddWithValue("@DepartureTime", flight.Departure_time);
                command.Parameters.AddWithValue("@FlightDate", flight.flight_date);
                command.ExecuteNonQuery();
            }
        }
    }

    private void ExecuteUpdateFlightProcedure(Flight flight)
    {
        using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("UpdateFlightProcedure", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", flight.Id);
                command.Parameters.AddWithValue("@FlightName", flight.flight_Name);
                command.Parameters.AddWithValue("@Departure", flight.Departure);
                command.Parameters.AddWithValue("@Destination", flight.Destenation);
                command.Parameters.AddWithValue("@ArrivalTime", flight.Arrival_time);
                command.Parameters.AddWithValue("@DepartureTime", flight.Departure_time);
                command.Parameters.AddWithValue("@FlightDate", flight.flight_date);
                command.ExecuteNonQuery();
            }
        }
    }

    private void ExecuteDeleteFlightProcedure(int flightId)
    {
        using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("DeleteFlightProcedure", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", flightId);
                command.ExecuteNonQuery();
            }
        }
    }
}
