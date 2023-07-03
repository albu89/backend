using AutoMapper;
using CE_API_V2.Data;
using CE_API_V2.Data.Repositories;
using CE_API_V2.Data.Repositories.Interfaces;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;

namespace CE_API_V2.UnitOfWorks
{
    public class ScoringUOW : IScoringUOW
    {
        private readonly IAiRequestService _aiRequestService;
        private readonly IMapper _mapper;
    
        private CEContext _context;
    
        private IGenericRepository<ScoringRequest> _scoringRequestRepository;
        private IGenericRepository<ScoringResponse> _scoringResponseRepository;
    
        public ScoringUOW(CEContext context,
            IAiRequestService requestService,
            IMapper mapper)
        {
            _context = context;
            _aiRequestService = requestService;
            _mapper = mapper;
        }        
    
        public IGenericRepository<ScoringRequest> ScoringRequestRepository
        {
            get
            {
                if (_scoringRequestRepository == null)
                    _scoringRequestRepository = new GenericRepository<ScoringRequest>(_context);
    
                return _scoringRequestRepository;
            }
        }
    
        public IGenericRepository<ScoringResponse> ScoringResponseRepository
        {
            get
            {
                if (_scoringResponseRepository == null)
                    _scoringResponseRepository = new GenericRepository<ScoringResponse>(_context);
    
                return _scoringResponseRepository;
            }
        }
    
        public ScoringRequest StoreScoringRequest(ScoringRequest scoringRequest, string UserId)
        {
            try
            {
                scoringRequest.UserId = UserId;
                ScoringRequestRepository.Insert(scoringRequest);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
    
            return scoringRequest;
        }
    
        public ScoringRequest RetrieveScoringRequest(Guid ScoringRequestId, string userId)
        {
            var scoringRequest = ScoringRequestRepository.GetByGuid(ScoringRequestId);
    
            if (scoringRequest is null || !scoringRequest.UserId.Equals(userId))
            {
                throw new Exception();
            }
            return scoringRequest;
        }
    
        public ScoringResponse StoreScoringResponse(ScoringResponse scoringResponse)
        {
            try
            {
                ScoringResponseRepository.Insert(scoringResponse);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
    
            return scoringResponse;
        }
    
        public IEnumerable<ScoringHistoryDto>? RetrieveScoringHistoryForUser(string UserId)
        {
            IEnumerable<ScoringHistoryDto> scoringHistory;
            
            try
            {
                var scoringRequests = ScoringRequestRepository.Get(x => x.UserId == UserId, null, "Response");
                scoringHistory = _mapper.Map<List<ScoringHistoryDto>>(scoringRequests);
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
    
            
            return scoringHistory;
        }
    
        public IEnumerable<ScoringHistoryDto> RetrieveScoringHistoryForPatient(string PatientId, string UserId)
        {
            var scoringHistory = new List<ScoringHistoryDto>();
    
            try
            {
                var scoringRequests = ScoringRequestRepository.Get(x => x.UserId.Equals(UserId) &&
                                                                             x.PatientId.Equals(PatientId)).ToList();
                scoringHistory = _mapper.Map<List<ScoringHistoryDto>>(scoringRequests);
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
            return scoringHistory;
        }
    
        public ScoringResponse? RetrieveScoringResponse(Guid scoringRequestId, string userId)
        {
            ScoringResponse? scoringResponse;
            try
            {
                scoringResponse = ScoringResponseRepository.Get(x => x.Request.UserId.Equals(userId) &&
                    x.RequestId.Equals(scoringRequestId), null, "Request").FirstOrDefault() ?? null;
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
    
            return scoringResponse;
        }
    
        public async Task<ScoringResponse> ProcessScoringRequest(ScoringRequest scoringRequest, string userId, string patientId)
        {
            if (StoreScoringRequest(scoringRequest, userId) is null)
            {
                // TODO: Better error handling
                return null;
            }
            
            var scoringResponse = await RequestScore(scoringRequest) ?? throw new Exception();

            scoringResponse.Request = scoringRequest;
            scoringResponse.RequestId = scoringRequest.Id;

            if(StoreScoringResponse(scoringResponse) is null)
            {
                // TODO: Handling for failed store
            }
            
            return scoringResponse;
        }
    
        private async Task<ScoringResponse?> RequestScore(ScoringRequest scoringRequest)
        {
            ScoringResponse requestedScore = null;
    
            bool scoreIsSucessfullyRetrieved = false;
            int retry = 0;
    
            // TODO: Implement HttpMessageHandler for retries
            while (!scoreIsSucessfullyRetrieved && retry < 3)
            { 
                requestedScore = await _aiRequestService.RequestScore(scoringRequest);
    
                if (requestedScore is not null)
                {
                    //Todo - check if requestedScore has valid values?
                    scoreIsSucessfullyRetrieved = true;
                }
                else
                {
                    retry++;
                    await Task.Delay(1000);
                }
            }
    
            if (!scoreIsSucessfullyRetrieved)
            {
                throw new Exception(); //Todo
            }
    
            return requestedScore;
        }
    }
}
