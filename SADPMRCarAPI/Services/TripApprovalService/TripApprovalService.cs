using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.Model;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace SADPMRCarAPI.Services.TripApprovalService
{
    public class TripApprovalService : ITripApprovalService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUserIdentity> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public TripApprovalService(ApplicationDbContext dbContext, UserManager<ApplicationUserIdentity> userManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration; 
        }

        private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<IActionResult> ApproveTripByManager(int tripId, int statusId)
        {
            var trip = await _dbContext.Trips.FindAsync(tripId);
            if (trip == null)
            {
                return new NotFoundObjectResult("Trip not found.");
            }

            // Currently logged in user
            var userIdentity = await _dbContext.Users.FirstOrDefaultAsync(c => c.Id == GetUserId());

            if (userIdentity == null)
            {
                return new NotFoundObjectResult("User not found.");
            }

            if (userIdentity != null && userIdentity.JobTitleId != 2)
            {
                return new NotFoundObjectResult("Only managers can approve trips.");
            }

            // Retrieve the selected status from the database
            var selectedStatus = await _dbContext.statuses.FindAsync(statusId);
            if (selectedStatus == null)
            {
                return new NotFoundObjectResult("Invalid status selected.");
            }

            // Update the trip status based on the selected status
            trip.StatusId = selectedStatus.StatusId;
            trip.ManagerApprovalUser = userIdentity.UserName;
            trip.StatusUpdatedDate = DateTime.Now;

            // Check if the trip is rejected by the manager
            if (selectedStatus.StatusName == "Reject")
            {
                // Set the approval flags and user for rejection
                trip.IsApprovedByManager = false;
                trip.IsApprovedByGeneralManager = false;
                trip.GeneralManagerApprovalUser = null;
            }
            else
            {
                trip.IsApprovedByManager = true;
            }

            await _dbContext.SaveChangesAsync();

            try
            {
                // Get SMTP server configuration from app settings
                var smtpServer = _configuration["EmailConfiguration:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailConfiguration:SmtpPort"]);
                var smtpUsername = _configuration["EmailConfiguration:SmtpUsername"];
                var smtpPassword = _configuration["EmailConfiguration:SmtpPassword"];

                // Configure SMTP settings
                var smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                // Create the email message
                var message = new MailMessage();
                message.From = new MailAddress("admin@sadpmr.co.za", "SADPMR");
                message.To.Add(trip.UserEmail);
                message.Subject = "Trip Status Update";
                message.Body = "Dear User, Your trip status has been updated.";

                // Send the email
                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email notification: {ex.Message}");
            }

            return new OkObjectResult("Trip status updated by the manager successfully.");
        }


        public async Task<IActionResult> ApproveTripByGeneralManager(int tripId , int statusId)
        {
            var trip = await _dbContext.Trips.FindAsync(tripId);
            if (trip == null)
            {
                return new NotFoundObjectResult("Trip not found.");
            }
            // Get the logged-in user's identity
            var userIdentity = await _dbContext.Users.FirstOrDefaultAsync(c => c.Id == GetUserId());

            // Check if the user is a general manager
            if (userIdentity.JobTitleId != 3) // Assuming JobTitleId 3 represents the "General Manager" job title
            {
                return new NotFoundObjectResult("Only general managers can approve trips.");
            }

            // Check if the user is in the same department as the trip
            if (userIdentity.DepartmentId != trip.TripId)
            {
                return new NotFoundObjectResult("You can only approve trips in your department.");
            }

            // Check if the trip is approved by the manager
            if (!trip.IsApprovedByManager)
            {
                return new NotFoundObjectResult("The trip must be approved by the manager first.");
            }


            var selectedStatus = await _dbContext.statuses.FindAsync(statusId);
            // Approve the trip by the general manager
            trip.StatusId = selectedStatus.StatusId;
            trip.IsApprovedByGeneralManager = true;
            trip.GeneralManagerApprovalUser = userIdentity.UserName;
            trip.StatusUpdatedDate = DateTime.Now; // Set the status updated date
            await _dbContext.SaveChangesAsync();

            return new OkObjectResult("Trip status updated by the general manager successfully.");
        }
    }
}
