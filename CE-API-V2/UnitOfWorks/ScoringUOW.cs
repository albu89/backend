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
        private IGenericRepository<Biomarkers> _biomarkersRepository;
    
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
        
        public IGenericRepository<Biomarkers> BiomarkersRepository
        {
            get
            {
                if (_biomarkersRepository == null)
                    _biomarkersRepository = new GenericRepository<Biomarkers>(_context);
    
                return _biomarkersRepository;
            }
        }

        public Biomarkers StoreBiomarkers(Guid scoringRequestId, Biomarkers biomarkers)
        {
            try
            {
                biomarkers.RequestId = scoringRequestId;
                BiomarkersRepository.Insert(biomarkers);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw new NotImplementedException(e.Message);
            }
            return biomarkers;
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
    
        public ScoringRequestModel RetrieveScoringRequest(Guid scoringRequestId, string userId)
        {
            var scoringRequest = ScoringRequestRepository.Get(x => x.Id == scoringRequestId, null, "Biomarkers.Response").SingleOrDefault();
    
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
                var scoringRequests = ScoringRequestRepository.Get(x => x.UserId == UserId, null, "Responses,Biomarkers");
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
                                                                             x.PatientId.Equals(PatientId), null, "Responses,Biomarkers").ToList();
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
                    x.RequestId.Equals(scoringRequestId), null, "Request,Request.Biomarkers").MaxBy(x => x.CreatedOn) ?? null;
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
    
            return scoringResponse;
        }
    
        public async Task<ScoringResponse> ProcessScoringRequest(ScoringRequest value, string userId, string patientId, Guid? existingScoringRequest = null)
        {
            var (scoringRequest, biomarkers) = _valueConversionUow.ConvertToScoringRequest(value, userId, patientId);
            if (existingScoringRequest is null && StoreScoringRequest(scoringRequest, userId) is null)
            {
                return null;
            }
            existingScoringRequest ??= scoringRequest.Id;
            if (StoreBiomarkers(existingScoringRequest.Value, biomarkers) is null)
            {
                return null;
            }

            if (scoringRequest.Id == Guid.Empty)
                scoringRequest.Id = existingScoringRequest.Value;

            scoringRequest.AddBiomarkers(biomarkers);
            
            var convertedSiValue = await _valueConversionUow.ConvertToSiValues(value);

            var (convertedSiScoringRequest, convertedBiomarkers) = _valueConversionUow.ConvertToScoringRequest(convertedSiValue, userId, patientId);
            convertedSiScoringRequest.AddBiomarkers(convertedBiomarkers);

            var scoringResponse = await RequestScore(convertedSiScoringRequest) ?? throw new Exception();

            scoringResponse.RequestId = scoringRequest.Id;
            scoringResponse.Biomarkers = biomarkers;
            var fullResponse = GetScoreSummary(scoringResponse, scoringResponse.Biomarkers);
            fullResponse.Biomarkers = biomarkers;
            _mapper.Map(fullResponse, scoringResponse);
            scoringResponse.BiomarkersId = scoringRequest.LatestBiomarkers.Id;
            
            if(StoreScoringResponse(scoringResponse) is null)
            {
            }
            scoringResponse.Request = scoringRequest;
            
            return fullResponse;
        }
        public ScoringResponse GetScoreSummary(ScoringResponseModel recentScore, Biomarkers biomarkers)
        {
            var scoreSummary = _mapper.Map<ScoringResponseModel, ScoringResponse>(recentScore);
            scoreSummary.Biomarkers = biomarkers;
            
            return _scoreSummaryUtility.SetAdditionalScoringParams(scoreSummary, CultureInfo.CurrentUICulture.Name);
        }

        private async Task<ScoringResponseModel?> RequestScore(ScoringRequestModel scoringRequest)
        {
            ScoringResponseModel requestedScore = null;
    
            bool scoreIsSucessfullyRetrieved = false;
            int retry = 0;
    
            while (!scoreIsSucessfullyRetrieved && retry < 3)
            { 
                requestedScore = await _aiRequestService.RequestScore(scoringRequest);
    
                if (requestedScore is not null)
                {
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
                throw new Exception();
            }
    
            return requestedScore;
        }
    }
}
