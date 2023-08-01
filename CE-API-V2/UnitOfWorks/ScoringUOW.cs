using System.Globalization;
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
        private readonly IScoreSummaryUtility _scoreSummaryUtility;
        private readonly IValueConversionUOW _valueConversionUow;
        private readonly IMapper _mapper;
    
        private CEContext _context;
    
        private IGenericRepository<ScoringRequestModel> _scoringRequestRepository;
        private IGenericRepository<ScoringResponseModel> _scoringResponseRepository;
    
        public ScoringUOW(CEContext context,
            IAiRequestService requestService,
            IMapper mapper,
            IValueConversionUOW valueConversionUow,
            IScoreSummaryUtility scoreSummaryUtility)
        {
            _context = context;
            _aiRequestService = requestService;
            _mapper = mapper;
            _scoreSummaryUtility = scoreSummaryUtility;
            _valueConversionUow = valueConversionUow;
        }        
    
        public IGenericRepository<ScoringRequestModel> ScoringRequestRepository
        {
            get
            {
                if (_scoringRequestRepository == null)
                    _scoringRequestRepository = new GenericRepository<ScoringRequestModel>(_context);
    
                return _scoringRequestRepository;
            }
        }
    
        public IGenericRepository<ScoringResponseModel> ScoringResponseRepository
        {
            get
            {
                if (_scoringResponseRepository == null)
                    _scoringResponseRepository = new GenericRepository<ScoringResponseModel>(_context);
    
                return _scoringResponseRepository;
            }
        }
    
        public ScoringRequestModel StoreScoringRequest(ScoringRequestModel scoringRequestModel, string UserId)
        {
            try
            {
                scoringRequestModel.UserId = UserId;
                ScoringRequestRepository.Insert(scoringRequestModel);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw new NotImplementedException();
            }
    
            return scoringRequestModel;
        }
    
        public ScoringRequestModel RetrieveScoringRequest(Guid ScoringRequestId, string userId)
        {
            var scoringRequest = ScoringRequestRepository.GetByGuid(ScoringRequestId);
    
            if (scoringRequest is null || !scoringRequest.UserId.Equals(userId))
            {
                throw new Exception();
            }
            return scoringRequest;
        }
    
        public ScoringResponseModel StoreScoringResponse(ScoringResponseModel scoringResponseModel)
        {
            try
            {
                ScoringResponseRepository.Insert(scoringResponseModel);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
    
            return scoringResponseModel;
        }
    
        public IEnumerable<SimpleScore>? RetrieveScoringHistoryForUser(string UserId)
        {
            IEnumerable<SimpleScore> scoringHistory;
            
            try
            {
                var scoringRequests = ScoringRequestRepository.Get(x => x.UserId == UserId, null, "Response");
                scoringHistory = _mapper.Map<List<SimpleScore>>(scoringRequests);
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
    
            
            return scoringHistory;
        }
    
        public IEnumerable<SimpleScore> RetrieveScoringHistoryForPatient(string PatientId, string UserId)
        {
            var scoringHistory = new List<SimpleScore>();
    
            try
            {
                var scoringRequests = ScoringRequestRepository.Get(x => x.UserId.Equals(UserId) &&
                                                                             x.PatientId.Equals(PatientId), null, "Response").ToList();
                scoringHistory = _mapper.Map<List<SimpleScore>>(scoringRequests);
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
            return scoringHistory;
        }
    
        public ScoringResponseModel? RetrieveScoringResponse(Guid scoringRequestId, string userId)
        {
            ScoringResponseModel? scoringResponse;
            try
            {
                scoringResponse = ScoringResponseRepository.Get(x => x.Request.UserId.Equals(userId) &&
                    x.RequestId.Equals(scoringRequestId), null, "Request,Request.Biomarkers").FirstOrDefault() ?? null;
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
    
            return scoringResponse;
        }
    
        public async Task<ScoringResponseModel> ProcessScoringRequest(ScoringRequest value, string userId, string patientId)
        {
            var scoringRequest = _valueConversionUow.ConvertToScoringRequest(value, userId, patientId);
            if (StoreScoringRequest(scoringRequest, userId) is null)
            {
                return null;
            }
            
            var convertedSiValue = await _valueConversionUow.ConvertToSiValues(value);

            var convertedSiScoringRequest = _valueConversionUow.ConvertToScoringRequest(convertedSiValue, userId, patientId);

            var scoringResponse = await RequestScore(convertedSiScoringRequest) ?? throw new Exception();

            scoringResponse.Request = scoringRequest;
            scoringResponse.RequestId = scoringRequest.Id;

            if(StoreScoringResponse(scoringResponse) is null)
            {
            }
            
            return scoringResponse;
        }
        public ScoringResponse GetScoreSummary(ScoringResponseModel recentScore)
        {
            var scoreSummary = _mapper.Map<ScoringResponseModel, ScoringResponse>(recentScore);
            
            return _scoreSummaryUtility.SetAdditionalScoringParams(scoreSummary, CultureInfo.CurrentUICulture.Name);
        }

        private async Task<ScoringResponseModel?> RequestScore(ScoringRequestModel scoringRequest)
        {
            ScoringResponseModel requestedScore = null;
    
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
