using System.Globalization;
using AutoMapper;
using CE_API_V2.Data;
using CE_API_V2.Data.Repositories;
using CE_API_V2.Data.Repositories.Interfaces;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using static CE_API_V2.Models.Enum.PatientDataEnums;

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
        private IGenericRepository<BiomarkersDraft> _draftBiomarkersRepository;

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

        public IGenericRepository<BiomarkersDraft> DraftBiomarkersRepository
        {
            get
            {
                if (_draftBiomarkersRepository == null)
                    _draftBiomarkersRepository = new GenericRepository<BiomarkersDraft>(_context);

                return _draftBiomarkersRepository;
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
                throw new NotImplementedException(e.Message);
            }
            return biomarkers;
        }

        public BiomarkersDraft? UpdateDraftRequest(ScoringRequestDraft value, Guid requestId)
        {
            var scoringRequest = ScoringRequestRepository
                 .Get(x => x.Id == requestId, null, "BiomarkersDraft").FirstOrDefault();

            ManualMapper.UpdateLatestBiomarkers(value, scoringRequest.LatestBiomarkersDraft);
            BiomarkersDraft? updatedBiomarkers;

            try
            {
                updatedBiomarkers = DraftBiomarkersRepository.Update(scoringRequest.LatestBiomarkersDraft);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }

            return updatedBiomarkers;
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
                throw new NotImplementedException();
            }

            return scoringRequestModel;
        }

        public ScoringRequestModel RetrieveScoringRequest(Guid scoringRequestId, string userId)
        {
            var scoringRequest = ScoringRequestRepository
                .Get(x => x.Id == scoringRequestId, null, "Biomarkers.Response").SingleOrDefault();
            var draftBiomarkers = DraftBiomarkersRepository.Get(x => x.RequestId == scoringRequestId);

            if (draftBiomarkers.Any())
            {
                scoringRequest.BiomarkersDraft = draftBiomarkers;
            }

            if (scoringRequest is null)
            {
                throw new Exception("Scoring Request cannot be null.");
            }
            if (!scoringRequest.UserId.Equals(userId))
            {
                throw new Exception("The user id of the scoring request does not match the one passed.");
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
            catch (Exception)
            {
                throw new NotImplementedException("Something went wrong while saving the scoring response.");
            }

            return scoringResponseModel;
        }

        public IEnumerable<SimpleScore>? RetrieveScoringHistoryForUser(string UserId)
        {
            IEnumerable<SimpleScore> scoringHistory;

            try
            {
                var scoringRequests = ScoringRequestRepository.Get(x => x.UserId == UserId, null, "Responses,Biomarkers,BiomarkersDraft").ToList();
                scoringHistory = _mapper.Map<List<SimpleScore>>(scoringRequests);

                foreach (var req in scoringRequests)
                {
                    scoringHistory.FirstOrDefault(x => x.RequestId == req.Id).CanEdit = _scoreSummaryUtility.CalculateIfUpdatePossible(req);
                    scoringHistory.FirstOrDefault(x => x.RequestId == req.Id).RequestTimeStamp = GetDisplayTime(req);
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException("Something went wrong while retrieving the scoring history, for the user.");
            }

            return scoringHistory;
        }

        public IEnumerable<SimpleScore> RetrieveScoringHistoryForPatient(string PatientId, string UserId)
        {
            var scoringHistory = new List<SimpleScore>();

            try
            {
                var scoringRequests = ScoringRequestRepository.Get(x => x.UserId.Equals(UserId) &&
                                                                             x.PatientId.Equals(PatientId), null, "Responses,Biomarkers,BiomarkersDraft").ToList();
                scoringHistory = _mapper.Map<List<SimpleScore>>(scoringRequests);
                foreach (var req in scoringRequests)
                {
                    var createscoringHistory = scoringRequests.FirstOrDefault(x => x.Id == req.Id);
                    var createdOn = createscoringHistory.CreatedOn;
                    var updatedOn = req.BiomarkersDraft.FirstOrDefault(x=> x.RequestId == req.Id)?.UpdatedOn;

                    scoringHistory.FirstOrDefault(x => x.RequestId == req.Id).CanEdit = _scoreSummaryUtility.CalculateIfUpdatePossible(req);
                    scoringHistory.FirstOrDefault(x => x.RequestId == req.Id).RequestTimeStamp = updatedOn is null || createdOn > (DateTimeOffset)updatedOn ? createdOn : (DateTimeOffset)updatedOn;
                    var isDraft = req.BiomarkersDraft.Any() && !req.Biomarkers.Any();
                    scoringHistory.FirstOrDefault(x => x.RequestId == req.Id).IsDraft = isDraft;
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException("Something went wrong while retrieving the scoring history, for the patient.");
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
            catch (Exception)
            {
                throw new NotImplementedException("Something went wrong while retrieving the scoring response.");
            }

            return scoringResponse;
        }

        public async Task<ScoringResponse> ProcessScoringRequest(ScoringRequest value, string userId, string patientId, ClinicalSetting clinicalSetting, Guid? existingScoringRequest = null)
        {
            var (scoringRequest, biomarkers) = _valueConversionUow.ConvertToScoringRequest(value, userId, patientId, clinicalSetting);
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

            var (convertedSiScoringRequest, convertedBiomarkers) = _valueConversionUow.ConvertToScoringRequest(convertedSiValue, userId, patientId, clinicalSetting);
            convertedSiScoringRequest.AddBiomarkers(convertedBiomarkers);

            var scoringResponse = await RequestScore(convertedSiScoringRequest) ?? throw new Exception();

            scoringResponse.RequestId = scoringRequest.Id;
            scoringResponse.Biomarkers = biomarkers;
            var fullResponse = GetScoringResponse(scoringResponse, scoringResponse.Biomarkers, scoringRequest.Id);

            var responseBiomarkers = GetResponseBiomarkers(biomarkers, scoringRequest.Id);
            fullResponse.Biomarkers = responseBiomarkers;
            _mapper.Map(fullResponse, scoringResponse);

            scoringResponse.BiomarkersId = scoringRequest.LatestBiomarkers.Id;

            if (StoreScoringResponse(scoringResponse) is null)
            {
            }
            scoringResponse.Request = scoringRequest;

            return fullResponse;
        }

        public BiomarkersDraft StoreDraftRequest(ScoringRequestDraft value, string userId, string patientId, ClinicalSetting clinicalSetting)
        {
            var (scoringRequestDraft, biomarkers) = _valueConversionUow.ConvertToScoringRequestDraft(value, userId, patientId, clinicalSetting);
            IsDraft(patientId, userId, out BiomarkersDraft biomarkersDraft); //Todo - refactor

            StoreScoringRequest(scoringRequestDraft, userId);

            return scoringRequestDraft.LatestBiomarkersDraft;
        }

        public ScoringResponse GetScoringResponse(ScoringResponseModel recentScore, Biomarkers biomarkers, Guid requestId)
        {
            ScoringResponse scoringResponse;
            var responseBiomarkers = GetResponseBiomarkers(biomarkers, requestId);

            if (recentScore is null)
            {
                scoringResponse = new ScoringResponse
                {
                    Biomarkers = responseBiomarkers,
                    IsDraft = true
                };
                return scoringResponse;
            }
            scoringResponse = _mapper.Map<ScoringResponseModel, ScoringResponse>(recentScore);
            scoringResponse.Biomarkers = responseBiomarkers;

            return _scoreSummaryUtility.SetAdditionalScoringParams(scoringResponse, CultureInfo.CurrentUICulture.Name);
        }

        public ScoringResponse GetScoringResponseFromDraft(BiomarkersDraft biomarkers, Guid requestId)
        {
            ScoringResponse scoringResponse;
            var responseBiomarkers = GetDraftResponseBiomarkers(biomarkers, requestId);

            scoringResponse = new ScoringResponse
            {
                Biomarkers = responseBiomarkers,
                IsDraft = true,
                RequestId = requestId
            };
            return scoringResponse;
        }

        public bool RequestIsDraft(ScoringRequestModel request) => request.LatestBiomarkersDraft is not null;

        public BiomarkersDraft? RemoveDraftBiomarkers(Guid requestId)
        {
            
            var scoringRequest = ScoringRequestRepository
                .Get(x => x.Id == requestId, null, "BiomarkersDraft").FirstOrDefault();

            if (scoringRequest is null || scoringRequest.LatestBiomarkersDraft is null)
            {
                return null;
            }

            BiomarkersDraft deletedBiomarkers;
            try
            {
                _context.ChangeTracker.Clear();
                deletedBiomarkers = DraftBiomarkersRepository.Delete(scoringRequest.LatestBiomarkersDraft);
                _context.SaveChanges();
                
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred removing the draft biomarkers");
            }

            return deletedBiomarkers;
        }

        public bool IsDraft(string patientId, string userId, out BiomarkersDraft draftBiomarkers)
        {
            draftBiomarkers = null;
            var request = ScoringRequestRepository.Get(x => x.PatientId == patientId && x.UserId == userId).MaxBy(t => t.CreatedOn) ?? null;

            if (request is null)
            {
                return false;
            }

            ScoringResponseModel? response = RetrieveScoringResponse(request.Id, userId);

            if (response is not null)
            {
                return false; //Response is only added with actual request
            }

            draftBiomarkers = DraftBiomarkersRepository.Get(x => x.RequestId == request.Id).FirstOrDefault();

            return true;
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
                throw new Exception("Something went wrong while requesting the score.");
            }

            return requestedScore;
        }

        private StoredBiomarkers GetResponseBiomarkers(Biomarkers biomarkers, Guid scoringRequestIs)
        {
            return new StoredBiomarkers()
            {
                CreatedOn = biomarkers.CreatedOn,
                Id = biomarkers.Id,
                RequestId = scoringRequestIs,
                Values = ManualMapper.MapFromBiomarkersToValues(biomarkers)
            };
        }

        private StoredBiomarkers GetDraftResponseBiomarkers(BiomarkersDraft biomarkers, Guid scoringRequestIs)
        {
            return new StoredBiomarkers()
            {
                CreatedOn = biomarkers.CreatedOn,
                Id = biomarkers.Id,
                RequestId = scoringRequestIs,
                Values = ManualMapper.MapFromDraftBiomarkersToValues(biomarkers)
            };
        }

        private DateTimeOffset GetDisplayTime(ScoringRequestModel requestModel)
        {
            if (requestModel.LatestBiomarkers is not null)
            {
                return requestModel.LatestBiomarkers.CreatedOn;
            }

            return requestModel.LatestBiomarkersDraft.UpdatedOn > requestModel.LatestBiomarkersDraft.CreatedOn
                ? requestModel.LatestBiomarkersDraft.UpdatedOn
                : requestModel.LatestBiomarkersDraft.CreatedOn;
        }
    }
}
