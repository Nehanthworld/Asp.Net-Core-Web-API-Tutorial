
namespace CollegeApp.Data.Repository
{
    public class StudentRepository : CollegeRepository<Student>, IStudentRepository
    {
        private readonly CollegeDBContext _dbContext;

        public StudentRepository(CollegeDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Student>> GetStudentsByFeeStatusAsync(int feeStatus)
        {
            //Write code to return students having fee status pending
            return null;
        }
    }
}
