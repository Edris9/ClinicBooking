

using System.Threading;
using System.Threading.Tasks;

public class UpdatePatientHandler : IRequestHandler<UpdatePatientCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdatePatientHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Result> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(request.Id);
        if (patient == null)
        {
            return Result.Failure("Patient not found.");
        }
        _mapper.Map(request, patient);
        _unitOfWork.Patients.Update(patient);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}