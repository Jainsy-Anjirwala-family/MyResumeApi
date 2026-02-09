// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using MyResumeApi.Models;

// [Route("api/[controller]")]
// [ApiController]
// public class WorkExperienceController : ControllerBase {
//     private readonly AppDbContext _context;

//     public WorkExperienceController(AppDbContext context) {
//         _context = context;
//     }

// //     [HttpGet]
// //     public async Task<ActionResult<IEnumerable<WorkExperience>>> GetExperience() {
// //         return await _context.WorkExperiences.ToListAsync();
// //     }

// //     [HttpPost]
// //     public async Task<ActionResult<WorkExperience>> PostExperience(WorkExperience work) {
// //         _context.WorkExperiences.Add(work);
// //         await _context.SaveChangesAsync();
// //         return Ok(work);
// //     }
// // }
//     [HttpGet("full-profile")]
//         public async Task<ActionResult<ProfileResponseDto>> GetFullProfile()
//         {
//             // Start all tasks simultaneously (Parallel Execution)
//             var personTask = _context.PersonDetails.ToListAsync();
//             var eduTask = _context.EducationDetails.ToListAsync();
//             var techTask = _context.TechnicalDetails.ToListAsync();
//             var achTask = _context.AchievementDetails.ToListAsync();
//             var achDesTask = _context.AchievementDesDetails.ToListAsync();
//             var expTask = _context.WorkExperiences.ToListAsync();
//             var expDesTask = _context.WorkExperienceDescriptions.ToListAsync();

//             // Wait for all to complete
//             await Task.WhenAll(personTask, eduTask, techTask, achTask, achDesTask, expTask, expDesTask);

//             var response = new ProfileResponseDto
//             {
//                 PersonDetails = await personTask,
//                 EducationDetails = await eduTask,
//                 TechnicalDetails = await techTask,
//                 AchievementDetails = await achTask,
//                 AchievementDesDetails = await achDesTask,
//                 Experience = await expTask,
//                 ExperienceDes = await expDesTask
//             };

//             return Ok(response);
//         }
//     }
//     // [HttpGet("custom-sql")]
//     // public async Task<IActionResult> GetCustomData()
//     // {
//     //     var query = @"
//     //         SELECT p.first_name, w.company_name, w.description 
//     //         FROM persondetail p
//     //         JOIN workexperiencedes w ON p.job_title = w.job_title";

//     //     // You can execute this if you have a specific model mapped for it
//     //     var results = await _context.Database.SqlQueryRaw<dynamic>(query).ToListAsync();
        
//     //     return Ok(results);
//     // }
// }
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using MyResumeApi.Models; // Essential to find AppDbContext and DTOs
// using Dapper; // Add this
// using Microsoft.Data.SqlClient; // Add this

// namespace MyResumeApi.Controllers
// {
//     [Route("api/profile")]
//     [ApiController]
//     public class WorkExperienceController : ControllerBase
//     {
//         private readonly AppDbContext _context;

//         public WorkExperienceController(AppDbContext context)
//         {
//             _context = context;
//         }

//         // GET: api/WorkExperience/full-profile
//             [HttpGet("full-profile")]
//             public async Task<ActionResult<ProfileResponseDto>> GetFullProfile()
//             {
//                 try 
//                 {
//                     using (var connection = new SqlConnection(_context.Database.GetDbConnection().GetConnectionString()))
//                     {
//                         // Fetch each list one by one to respect DbContext thread safety
//                         var personDetails = await _context.PersonDetails.ToListAsync();
//                         var eduDetails = await _context.EducationDetails.ToListAsync();
//                         var techDetails = await _context.TechnicalDetails.ToListAsync();
//                         var achDetails = await _context.AchievementDetails.ToListAsync();
//                         var achDesDetails = await _context.AchievementDesDetails.ToListAsync();
//                         var experience = await _context.WorkExperiences.ToListAsync();
//                         var experienceDes = await _context.WorkExperienceDescriptions.ToListAsync();
//                         var experienceList = new List<WorkExperience>();
//                         foreach (var item in experience)
//                         {
//                             // Instead of console.log, use Console.WriteLine
//                             Console.WriteLine($"Processing work_id: {item.work_id}");
//                             var des = experienceDes.Where(d => d.job_description == item.job_title)?.Select(d => d.description).ToList() ?? new List<string>();
//                             experienceList.Add(new WorkExperience
//                             {
//                                 work_id = item.work_id,
//                                 company_name = item.company_name,
//                                 job_title = item.job_title,
//                                 description = des
//                             });
//                         }
//                         Console.WriteLine($"Experience List Count: {experienceList.Count}");
//                         var response = new ProfileResponseDto
//                         {
//                             PersonDetails = personDetails,
//                             EducationDetails = eduDetails,
//                             TechnicalDetails = techDetails,
//                             AchievementDetails = achDetails,
//                             AchievementDesDetails = achDesDetails,
//                             Experience = experienceList,
//                             ExperienceDescriptions = experienceWithDes
//                         };

//                         return Ok(response);
//                     }
//                 }
//                 catch (Exception ex)
//                 {
//                     return StatusCode(500, $"Internal server error: {ex.Message}");
//                 }
//             }
//     }
// }

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyResumeApi.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MyResumeApi.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class WorkExperienceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WorkExperienceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("full-profile")]
        public async Task<ActionResult<ProfileResponseDto>> GetFullProfile()
        {
            try 
            {
                // 1. Standard EF Core fetches
                // var personDetails = await _context.PersonDetails.ToListAsync();
                var personDetails = await _context.PersonDetails.FirstOrDefaultAsync();
                var eduDetails = await _context.EducationDetails.ToListAsync();
                var techDetails = await _context.TechnicalDetails.ToListAsync();
                var achDetails = await _context.AchievementDetails.ToListAsync();
                var achDesDetails = await _context.AchievementDesDetails.ToListAsync();
                var experience = await _context.WorkExperiences.ToListAsync();
                var experienceDes = await _context.WorkExperienceDescriptions.ToListAsync();
                var experienceList = experience.Select(exp => new WorkExperience
                {
                    work_id = exp.work_id,
                    company_name = exp.company_name,
                    job_title = exp.job_title,
                    year_id = exp.year_id,
                    // Filter descriptions that belong to this specific job_title/work_id
                    description = experienceDes
                        .Where(d => d.job_description == exp.job_title) // Adjust 'job_title' to 'work_id' if that is your Foreign Key
                        .Select(d => d.description ?? "")
                        .ToList()
                }).ToList();
                var achievementList = achDetails.Select(exp => new Achievementdetail
                {
                    achievement_id = exp.achievement_id,
                    achievement_name = exp.achievement_name,
                    // Filter descriptions that belong to this specific job_title/work_id
                    achievements_details = achDesDetails
                        .Where(d => d.achievement_name == exp.achievement_name) // Adjust 'job_title' to 'work_id' if that is your Foreign Key
                        .Select(d => d.achievements_detail ?? "")
                        .ToList()
                }).ToList();

                var response = new ProfileResponseDto
                {   
                    Status = 200,
                    Ok = true,
                    PersonDetails = personDetails,
                    EducationDetails = eduDetails,
                    TechnicalDetails = techDetails,
                    AchievementDetails = achievementList,
                    Experience = experienceList
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}