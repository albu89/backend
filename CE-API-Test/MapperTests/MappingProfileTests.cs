using System.Collections.ObjectModel;
using AutoMapper;
using CE_API_Test.TestUtilities;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Utility;

namespace CE_API_Test.MapperTests;

[TestFixture]
public class MappingProfileTests
{
    private IMapper _mapper;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

        _mapper = mapperConfig.CreateMapper();
    }

    [Test]
    public void ScoringRequestToScoringRequestModel()
    {
        //Arrange 
        var dto = MockDataProvider.CreateScoringRequest();

        //Act
        var request = _mapper.Map<ScoringRequestModel>(dto);

        //Assert
        request.Should().NotBeNull();
        request?.LatestBiomarkers?.Should().NotBeNull();
        request?.LatestBiomarkers?.Age.Should().Be(dto.Age.Value);
        request?.LatestBiomarkers?.Alat.Should().Be(dto.Alat.Value);
        request?.LatestBiomarkers?.Albumin.Should().Be(dto.Albumin.Value);
        request?.LatestBiomarkers?.Bilirubin.Should().Be(dto.Bilirubin.Value);
        request?.LatestBiomarkers?.Cholesterol.Should().Be(dto.Cholesterol.Value);
        request?.LatestBiomarkers?.Diabetes.Should().Be(dto.Diabetes.Value);
        request?.LatestBiomarkers?.Diuretic.Should().Be(dto.Diuretic.Value);
        request?.LatestBiomarkers?.Glucose.Should().Be(dto.GlucoseFasting.Value);
        request?.LatestBiomarkers?.Hdl.Should().Be(dto.Hdl.Value);
        request?.LatestBiomarkers?.Height.Should().Be(dto.Height.Value);
        request?.LatestBiomarkers?.Hstroponint.Should().Be(dto.HsTroponinT.Value);
        request?.LatestBiomarkers?.Ldl.Should().Be(dto.Ldl.Value);
        request?.LatestBiomarkers?.Leukocyte.Should().Be(dto.Leukocytes.Value);
        request?.LatestBiomarkers?.Mchc.Should().Be(dto.Mchc.Value);
        request?.LatestBiomarkers?.Nicotine.Should().Be(dto.NicotineConsumption.Value);
        request?.LatestBiomarkers?.Protein.Should().Be(dto.Protein.Value);
        request?.LatestBiomarkers?.Sex.Should().Be(dto.Sex.Value);
        request?.LatestBiomarkers?.Statin.Should().Be(dto.CholesterolLowering_Statin.Value);
        request?.LatestBiomarkers?.Systolicbp.Should().Be(dto.SystolicBloodPressure.Value);
        request?.LatestBiomarkers?.Urea.Should().Be(dto.Urea.Value);
        request?.LatestBiomarkers?.Albumin.Should().Be(dto.Albumin.Value);
        request?.LatestBiomarkers?.Aceinhibitor.Should().Be(dto.ACEInhibitor.Value);
        request?.LatestBiomarkers?.Alkaline.Should().Be(dto.AlkalinePhosphatase.Value); // TODO: Check naming
        request?.LatestBiomarkers?.Betablocker.Should().Be(dto.Betablocker.Value);
        request?.LatestBiomarkers?.Qwave.Should().Be(dto.RestingECG.Value);
        request?.LatestBiomarkers?.Calciumant.Should().Be(dto.CaAntagonist.Value);
        request?.LatestBiomarkers?.Chestpain.Should().Be(dto.ChestPain.Value);
        request?.LatestBiomarkers?.Amylasep.Should().Be(dto.PancreaticAmylase.Value);
        request?.LatestBiomarkers?.Tcagginhibitor.Should().Be(dto.TCAggregationInhibitor.Value);
        request?.LatestBiomarkers?.PriorCAD.Should().Be(dto.prior_CAD.Value);
    }

    [Test]
    public void CreateUserToUserModel()
    {
        //Arrange
        var user = MockDataProvider.GetMockedCreateUser();

        //Act
        var userModel = _mapper.Map<UserModel>(user);

        //Assert
        userModel.Should().NotBeNull();
        userModel.UserId.Should().Be(string.Empty);
        userModel.TenantID.Should().Be(string.Empty);
        userModel.Address.Should().Be(user.Address);
        userModel.Salutation.Should().Be(user.Salutation);
        userModel.Surname.Should().Be(user.Surname);
        userModel.FirstName.Should().Be(user.FirstName);
        userModel.ProfessionalSpecialisation.Should().Be(user.ProfessionalSpecialisation);
        userModel.Department.Should().Be(user.Department);
        userModel.PreferredLab.Should().Be(user.PreferredLab);
        userModel.Address.Should().Be(user.Address);
        userModel.City.Should().Be(user.City);
        userModel.CountryCode.Should().Be(user.CountryCode);
        userModel.Country.Should().Be(user.Country);
        userModel.TelephoneNumber.Should().Be(user.TelephoneNumber);
        userModel.Language.Should().Be(user.Language);
        userModel.UnitLabValues.Should().Be(user.UnitLabValues);
        userModel.ClinicalSetting.Should().Be(user.ClinicalSetting);
    }

    [Test]
    public void ScoringRequestToBiomarkers()
    {
        //Arrange
        var scoringRequest = MockDataProvider.CreateScoringRequest();

        //Act
        var biomarkers = _mapper.Map<Biomarkers>(scoringRequest);

        //Assert
        biomarkers.Should().NotBeNull();
        biomarkers.Age.Should().Be(scoringRequest.Age.Value);
        biomarkers.Alat.Should().Be(scoringRequest.Alat.Value);
        biomarkers.Albumin.Should().Be(scoringRequest.Albumin.Value);
        biomarkers.Bilirubin.Should().Be(scoringRequest.Bilirubin.Value);
        biomarkers.Cholesterol.Should().Be(scoringRequest.Cholesterol.Value);
        biomarkers.Diabetes.Should().Be(scoringRequest.Diabetes.Value);
        biomarkers.Diuretic.Should().Be(scoringRequest.Diuretic.Value);
        biomarkers.Glucose.Should().Be(scoringRequest.GlucoseFasting.Value);
        biomarkers.Hdl.Should().Be(scoringRequest.Hdl.Value);
        biomarkers.Height.Should().Be(scoringRequest.Height.Value);
        biomarkers.Hstroponint.Should().Be(scoringRequest.HsTroponinT.Value);
        biomarkers.Ldl.Should().Be(scoringRequest.Ldl.Value);
        biomarkers.Leukocyte.Should().Be(scoringRequest.Leukocytes.Value);
        biomarkers.Mchc.Should().Be(scoringRequest.Mchc.Value);
        biomarkers.Nicotine.Should().Be(scoringRequest.NicotineConsumption.Value);
        biomarkers.Protein.Should().Be(scoringRequest.Protein.Value);
        biomarkers.Sex.Should().Be(scoringRequest.Sex.Value);
        biomarkers.Statin.Should().Be(scoringRequest.CholesterolLowering_Statin.Value);
        biomarkers.Systolicbp.Should().Be(scoringRequest.SystolicBloodPressure.Value);
        biomarkers.Urea.Should().Be(scoringRequest.Urea.Value);
        biomarkers.Albumin.Should().Be(scoringRequest.Albumin.Value);
        biomarkers.Aceinhibitor.Should().Be(scoringRequest.ACEInhibitor.Value);
        biomarkers.Alkaline.Should().Be(scoringRequest.AlkalinePhosphatase.Value); // TODO: Check naming
        biomarkers.Betablocker.Should().Be(scoringRequest.Betablocker.Value);
        biomarkers.Qwave.Should().Be(scoringRequest.RestingECG.Value);
        biomarkers.Calciumant.Should().Be(scoringRequest.CaAntagonist.Value);
        biomarkers.Chestpain.Should().Be(scoringRequest.ChestPain.Value);
        biomarkers.Amylasep.Should().Be(scoringRequest.PancreaticAmylase.Value);
        biomarkers.Tcagginhibitor.Should().Be(scoringRequest.TCAggregationInhibitor.Value);
        biomarkers.PriorCAD.Should().Be(scoringRequest.prior_CAD.Value);
    }

    [Test]
    public void UserModelToUser()
    {
        //Arrange
        var userModel = MockDataProvider.GetMockedUserModel();
        userModel.BiomarkerOrders = GetBiomarkerCollection();

        //Act
        var user = _mapper.Map<User>(userModel);

        //Assert
        user.Should().NotBeNull();
        user.UserId.Should().BeEquivalentTo(userModel.UserId);
        user.Salutation.Should().BeEquivalentTo(userModel.Salutation);
        user.Title.Should().BeEquivalentTo(userModel.Title);
        user.Surname.Should().BeEquivalentTo(userModel.Surname);
        user.FirstName.Should().BeEquivalentTo(userModel.FirstName);
        user.ProfessionalSpecialisation.Should().BeEquivalentTo(userModel.ProfessionalSpecialisation);
        user.Department.Should().BeEquivalentTo(userModel.Department);
        user.PreferredLab.Should().BeEquivalentTo(userModel.PreferredLab);
        user.Address.Should().BeEquivalentTo(userModel.Address);
        user.City.Should().BeEquivalentTo(userModel.City);
        user.ZipCode.Should().BeEquivalentTo(userModel.ZipCode);
        user.CountryCode.Should().BeEquivalentTo(userModel.CountryCode);
        user.Country.Should().BeEquivalentTo(userModel.Country);
        user.TelephoneNumber.Should().BeEquivalentTo(userModel.TelephoneNumber);
        user.EMailAddress.Should().BeEquivalentTo(userModel.EMailAddress);
        user.Language.Should().BeEquivalentTo(userModel.Language);
        user.UnitLabValues.Should().BeEquivalentTo(userModel.UnitLabValues);
        user.ClinicalSetting.Should().Be(userModel.ClinicalSetting);
        user.Role.Should().Be(userModel.Role);
        user.IsActive.Should().Be(userModel.IsActive);
    }

    [Test]
    public void ScoringResponseToScoringResponseModel()
    {
        //Arrange
        var scoringResponse = MockDataProvider.GetMockedScoringResponse();

        //Act
        var scoringResponseModel = _mapper.Map<ScoringResponseModel>(scoringResponse);

        //Assert
        scoringResponseModel.Should().NotBeNull();
        scoringResponseModel.Id.Should().Be(scoringResponse.Id);
        scoringResponseModel.classifier_class.Should().Be(scoringResponse.classifier_class);
        scoringResponseModel.classifier_score.Should().Be(scoringResponse.classifier_score);
        scoringResponseModel.classifier_sign.Should().Be(scoringResponse.classifier_sign);
        scoringResponseModel.CreatedOn.Should().Be(scoringResponse.CreatedOn);
        scoringResponseModel.Score.Should().BeEquivalentTo(scoringResponse.Score);
        scoringResponseModel.Risk.Should().BeEquivalentTo(scoringResponse.Risk);
        scoringResponseModel.RiskClass.Should().Be(scoringResponse.RiskClass);
        scoringResponseModel.Recommendation.Should().BeEquivalentTo(scoringResponse.Recommendation);
        scoringResponseModel.RecommendationLong.Should().BeEquivalentTo(scoringResponse.RecommendationLong);
        scoringResponseModel.Warnings.Should().BeEquivalentTo(scoringResponse.Warnings);
        scoringResponseModel.Biomarkers.Should().BeEquivalentTo(scoringResponse.Biomarkers);
        scoringResponseModel.BiomarkersId.Should().Be(scoringResponse.BiomarkersId);
        scoringResponseModel.Request.Should().BeEquivalentTo(scoringResponse.Request);
        scoringResponseModel.RequestId.Should().Be(scoringResponse.RequestId);

    }

    [Test]
    public void ScoringResponseModelToScoringResponse()
    {
        //Arrange
        var scoringRequest = MockDataProvider.GetMockedScoringRequest();
        ScoringResponseModel scoringResponseModel = MockDataProvider.GetMockedScoringResponseForRequest(scoringRequest);
        scoringResponseModel.Biomarkers = scoringResponseModel.Biomarkers;
        scoringResponseModel.RiskClass = 1;

        //Act
        var scoringResponse = _mapper.Map<ScoringResponse>(scoringResponseModel);

        //Assert
        scoringResponse.classifier_score.Should().Be(scoringResponseModel.classifier_score);
        scoringResponse.RequestId.Should().Be(scoringResponseModel.RequestId);
        scoringResponse.RiskClass.Should().Be(scoringResponseModel.RiskClass);
        scoringResponse.Prevalence.Should().Be(ScoreSummaryUtility.PrevalenceClass.Primary);
        scoringResponse.CanEdit.Should().Be(false); //default 
        scoringResponse.IsDraft.Should().Be(false); //default 
    }

    [Test]
    public void CreateCountryToCountryModel()
    {
        //Arrange
        var createCountry = GetCreateCountry();

        //Act
        var countryModel = _mapper.Map<CountryModel>(createCountry);

        //Assert
        countryModel.Should().NotBeNull();
        countryModel.Name.Should().BeEquivalentTo(createCountry.Name);
        countryModel.ContactEmail.Should().BeEquivalentTo(createCountry.ContactEmail);
    }

    [Test]
    public void CountryModelToCreateCountry()
    {
        //Arrange
        var countryModel = GetCountryModel();

        //Act
        var createCountry = _mapper.Map<CreateCountry>(countryModel);

        //Assert
        createCountry.Should().NotBeNull();
        createCountry.Name.Should().BeEquivalentTo(countryModel.Name);
        createCountry.ContactEmail.Should().BeEquivalentTo(countryModel.ContactEmail);
    }

    [Test]
    public void CountryModelToCountry()
    {
        //Arrange
        var countryModel = GetCountryModel();

        //Act
        var country = _mapper.Map<Country>(countryModel);

        //Assert
        country.Should().NotBeNull();
        country.Id.Should().Be(countryModel.Id);
        country.Name.Should().BeEquivalentTo(countryModel.Name);
    }

    [Test]
    public void CreateOrganizationToOrganizationModel()
    {
        //Arrange
        var expectedNotMappedId = new Guid(); // corresponds to the default value

        var createOrganization = GetCreateOrganization();

        //Act
        var organizationModel = _mapper.Map<OrganizationModel>(createOrganization);

        //Assert
        organizationModel.Should().NotBeNull();
        organizationModel.Id.Should().Be(expectedNotMappedId); // not present in CreateOrganization
        organizationModel.TenantId.Should().Be(createOrganization.TenantId);
        organizationModel.Name.Should().BeEquivalentTo(createOrganization.Name);
        organizationModel.ContactEmail.Should().BeEquivalentTo(createOrganization.ContactEmail);
    }

    [Test]
    public void OrganizationModelToOrganizationModel()
    {
        //Arrange
        var organizationModel = GetOrganizationModel();

        //Act
        var mappedOrganizationModel = _mapper.Map<OrganizationModel>(organizationModel);

        //Assert
        mappedOrganizationModel.Should().NotBeNull();
        mappedOrganizationModel.Id.Should().NotBe(organizationModel.Id); // is ignored in mapping profile
        mappedOrganizationModel.TenantId.Should().Be(organizationModel.TenantId);
        mappedOrganizationModel.ContactEmail.Should().BeEquivalentTo(organizationModel.ContactEmail);
        mappedOrganizationModel.Name.Should().BeEquivalentTo(organizationModel.Name);
    }

    [Test]
    public void OrganizationModelToOrganization()
    {
        //Arrange
        var organizationModel = GetOrganizationModel();

        //Act
        var organization = _mapper.Map<Organization>(organizationModel);

        //Assert
        organization.Should().NotBeNull();
        organization.Id.Should().Be(organizationModel.Id);
        organization.Name.Should().BeEquivalentTo(organizationModel.Name);
    }

    [Test]
    public void OrganizationModelToCreateOrganization()
    {
        //Arrange
        var organizationModel = GetOrganizationModel();

        //Act
        var createOrganization = _mapper.Map<CreateOrganization>(organizationModel);

        //Assert
        createOrganization.Should().NotBeNull();
        createOrganization.Name.Should().BeEquivalentTo(organizationModel.Name);
        createOrganization.TenantId.Should().Be(organizationModel.TenantId);
        createOrganization.ContactEmail.Should().Be(organizationModel.ContactEmail);
    }

    [Test]
    public void ScoringRequestModelToSimpleScore()
    {
        //Arrange
        var requestModel = GetScoringRequestModelMock();
        requestModel.LatestBiomarkers.Response = new ScoringResponseModel() { Risk = "MockRisk", RiskClass = 1, classifier_score = 2.0 };

        //Act
        var simpleScore = _mapper.Map<SimpleScore>(requestModel);

        //Assert
        simpleScore.Should().NotBeNull();
        simpleScore.CanEdit.Should().BeFalse();
        simpleScore.IsDraft.Should().BeFalse();
        simpleScore.RequestId.Should().Be(requestModel.Id);
        simpleScore.RequestTimeStamp.Should().Be(requestModel.LatestBiomarkers.CreatedOn);
        simpleScore.Risk.Should().Be(requestModel.LatestResponse.Risk);
        simpleScore.RiskClass.Should().Be(requestModel.LatestResponse.RiskClass);
        simpleScore.Score.Should().Be(2.0f);
    }

    [Test]
    public void ScoringSchemaToScoreSchema()
    {
        //Arrange
        var scoringSchema = GetScoringSchema();

        //Act
        var scoreSchema = _mapper.Map<ScoreSchema>(scoringSchema);

        //Assert
        scoreSchema.Should().NotBeNull();
        scoreSchema.ScoreHeader.Should().BeEquivalentTo(scoringSchema.ScoreHeader);
        scoreSchema.Score.Should().BeEquivalentTo(scoringSchema.Score);
        scoreSchema.RiskHeader.Should().BeEquivalentTo(scoringSchema.RiskHeader);
        scoreSchema.Risk.Should().BeEquivalentTo(scoringSchema.Risk);
        scoreSchema.RecommendationHeader.Should().BeEquivalentTo(scoringSchema.RecommendationHeader);
        scoreSchema.Recommendation.Should().BeEquivalentTo(scoringSchema.Recommendation);
        scoreSchema.RecommendationExtended.Should().BeEquivalentTo(scoringSchema.RecommendationExtended);
        scoreSchema.RecommendationTableHeader.Should().BeEquivalentTo(scoringSchema.RecommendationTableHeader);
        scoreSchema.RecommendationProbabilityHeader.Should().BeEquivalentTo(scoringSchema.RecommendationProbabilityHeader);
        scoreSchema.RecommendationScoreRangeHeader.Should().BeEquivalentTo(scoringSchema.RecommendationScoreRangeHeader);
        scoreSchema.WarningHeader.Should().BeEquivalentTo(scoringSchema.WarningHeader);
        scoreSchema.Warnings.Should().BeEquivalentTo(scoringSchema.Warnings);
        scoreSchema.InfoText.Should().BeEquivalentTo(scoringSchema.InfoText);
        scoreSchema.Abbreviations.Should().BeEquivalentTo(scoringSchema.Abbreviations);
        scoreSchema.CadDefinitionHeader.Should().BeEquivalentTo(scoringSchema.CadDefinitionHeader);
        scoreSchema.CadDefinition.Should().BeEquivalentTo(scoringSchema.CadDefinition);
        scoreSchema.FootnoteHeader.Should().BeEquivalentTo(scoringSchema.FootnoteHeader);
        scoreSchema.Footnote.Should().BeEquivalentTo(scoringSchema.Footnote);
        scoreSchema.IntendedUseHeader.Should().BeEquivalentTo(scoringSchema.IntendedUseHeader);
        scoreSchema.IntendedUse.Should().BeEquivalentTo(scoringSchema.IntendedUse);
    }

    [Test]
    public void RecommendationCategoryToScoringResponse()
    {
        //Arrange
        var recommendationCategory = GetRecommendationCategory();

        //Act
        var scoringResponse = _mapper.Map<ScoringResponse>(recommendationCategory);

        //Assert
        scoringResponse.Should().NotBeNull();
        scoringResponse.RecommendationSummary.Should().Be(recommendationCategory.ShortText);
        scoringResponse.RecommendationLongText.Should().Be(recommendationCategory.LongText);
        scoringResponse.RiskValue.Should().Be(recommendationCategory.RiskValue);
        scoringResponse.RiskClass.Should().Be(recommendationCategory.Id);
    }

    [Test]
    public void RecommendationCategoryLocalizedPartToScoringResponse()
    {
        //Arrange
        var categoryLocalizedPart = GetRecommendationCategoryLocalizedPart();

        //Act
        var recommendationCategory = _mapper.Map<RecommendationCategory>(categoryLocalizedPart);

        //Assert
        recommendationCategory.Should().NotBeNull();
        recommendationCategory.ShortText.Should().Be(categoryLocalizedPart.ShortText);
        recommendationCategory.LongText.Should().Be(categoryLocalizedPart.LongText);
        recommendationCategory.Id.Should().Be(categoryLocalizedPart.Id);
    }

    [Test]
    public void RecommendationCategoryStaticPartToScoringResponse()
    {
        //Arrange
        var categoryStaticPart = GetRecommendationCategoryStaticPart();

        //Act
        var recommendationCategory = _mapper.Map<RecommendationCategory>(categoryStaticPart);

        //Assert
        recommendationCategory.Should().NotBeNull();
        recommendationCategory.LowerLimit.Should().Be(categoryStaticPart.LowerLimit);
        recommendationCategory.UpperLimit.Should().Be(categoryStaticPart.UpperLimit);
        recommendationCategory.Id.Should().Be(categoryStaticPart.Id);
    }

    [Test]
    public void UserToUserModel()
    {
        //Arrange
        var user = MockDataProvider.GetMockedUser();

        //Act
        var userModel = _mapper.Map<UserModel>(user);

        //Assert
        userModel.Should().NotBeNull();
        userModel.UserId.Should().BeEquivalentTo(user.UserId);
        userModel.Salutation.Should().BeEquivalentTo(user.Salutation);
        userModel.Title.Should().BeEquivalentTo(user.Title);
        userModel.Surname.Should().BeEquivalentTo(user.Surname);
        userModel.FirstName.Should().BeEquivalentTo(user.FirstName);
        userModel.ProfessionalSpecialisation.Should().BeEquivalentTo(user.ProfessionalSpecialisation);
        userModel.Department.Should().BeEquivalentTo(user.Department);
        userModel.PreferredLab.Should().BeEquivalentTo(user.PreferredLab);
        userModel.Address.Should().BeEquivalentTo(user.Address);
        userModel.City.Should().BeEquivalentTo(user.City);
        userModel.ZipCode.Should().BeEquivalentTo(user.ZipCode);
        userModel.CountryCode.Should().BeEquivalentTo(user.CountryCode);
        userModel.Country.Should().BeEquivalentTo(user.Country);
        userModel.TelephoneNumber.Should().BeEquivalentTo(user.TelephoneNumber);
        userModel.EMailAddress.Should().BeEquivalentTo(user.EMailAddress);
        userModel.Language.Should().BeEquivalentTo(user.Language);
        userModel.UnitLabValues.Should().BeEquivalentTo(user.UnitLabValues);
        userModel.IsActive.Should().Be(user.IsActive);
        userModel.TenantID.Should().BeNull();
        userModel.CreatedOn.Should().BeBefore(DateTimeOffset.Now);
        userModel.ScoringRequestModels.Should().BeNull();
    }

    [Test]
    public void UserInputFormSchemaHeadersToUserInputFormSchema()
    {
        //Arrange
        var expectedMockedStringValue = "MockSchemaHeaders";
        var expectedBillingHeaders = MockDataProvider.GetSchemaHeaders();
        var userInputFormSchemaHeader = MockDataProvider.GetInPutFormSchemaHeaders();

        //Act
        var userInputFormSchema = _mapper.Map<UserInputFormSchema>(userInputFormSchemaHeader);

        //Assert
        userInputFormSchema.Should().NotBeNull();
        userInputFormSchema.ClinicalSettingHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.DepartmentHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.OrganizationHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.SalutationHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.TitleHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.SurnameHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.FirstNameHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.ProfessionalSpecialisationHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.PreferredLabHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.AddressHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.CityHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.ZipCodeHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.CountryCodeHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.CountryHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.TelephoneNumberHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.EMailAddressHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.LanguageHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.UnitLabValuesHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.IsActiveHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.IsSeparateBillingHeader.Should().Be(expectedMockedStringValue);
        userInputFormSchema.Billing.Should().BeEquivalentTo(expectedBillingHeaders);
        userInputFormSchema.ChangeClinicalSettingHeader.Should().Be(expectedMockedStringValue);

        userInputFormSchema.Department.Should().BeNullOrEmpty();
        userInputFormSchema.Organization.Should().BeNullOrEmpty();
        userInputFormSchema.Salutation.Should().BeNullOrEmpty();
        userInputFormSchema.Title.Should().BeNullOrEmpty();
        userInputFormSchema.Surname.Should().BeNullOrEmpty();
        userInputFormSchema.FirstName.Should().BeNullOrEmpty();
        userInputFormSchema.ProfessionalSpecialisation.Should().BeNullOrEmpty();
        userInputFormSchema.PreferredLab.Should().BeNullOrEmpty();
        userInputFormSchema.Address.Should().BeNullOrEmpty();
        userInputFormSchema.City.Should().BeNullOrEmpty();
        userInputFormSchema.ZipCode.Should().BeNullOrEmpty();
        userInputFormSchema.CountryCode.Should().BeNullOrEmpty();
        userInputFormSchema.Country.Should().BeNullOrEmpty();
        userInputFormSchema.TelephoneNumber.Should().BeNullOrEmpty();
        userInputFormSchema.EMailAddress.Should().BeNullOrEmpty();
        userInputFormSchema.Language.Should().BeNullOrEmpty();
        userInputFormSchema.UnitLabValues.Should().BeNullOrEmpty();
        userInputFormSchema.IsActive.Should().BeFalse();
        userInputFormSchema.IsSeparateBilling.Should().BeFalse();
        userInputFormSchema.ChangeClinicalSetting.Should().BeNullOrEmpty();
    }

    [Test]
    public void CreateUserToUserInputFormSchema()
    {
        //Arrange
        var createUser = MockDataProvider.GetCreateUser();

        //Act
        var userInputFormSchema = _mapper.Map<UserInputFormSchema>(createUser);

        //Assert
        userInputFormSchema.Should().NotBeNull();
        userInputFormSchema.ClinicalSettingHeader.Should().BeNullOrEmpty();
        userInputFormSchema.DepartmentHeader.Should().BeNullOrEmpty();
        userInputFormSchema.OrganizationHeader.Should().BeNullOrEmpty();
        userInputFormSchema.SalutationHeader.Should().BeNullOrEmpty();
        userInputFormSchema.TitleHeader.Should().BeNullOrEmpty();
        userInputFormSchema.SurnameHeader.Should().BeNullOrEmpty();
        userInputFormSchema.FirstNameHeader.Should().BeNullOrEmpty();
        userInputFormSchema.ProfessionalSpecialisationHeader.Should().BeNullOrEmpty();
        userInputFormSchema.PreferredLabHeader.Should().BeNullOrEmpty();
        userInputFormSchema.AddressHeader.Should().BeNullOrEmpty();
        userInputFormSchema.CityHeader.Should().BeNullOrEmpty();
        userInputFormSchema.ZipCodeHeader.Should().BeNullOrEmpty();
        userInputFormSchema.CountryCodeHeader.Should().BeNullOrEmpty();
        userInputFormSchema.CountryHeader.Should().BeNullOrEmpty();
        userInputFormSchema.TelephoneNumberHeader.Should().BeNullOrEmpty();
        userInputFormSchema.EMailAddressHeader.Should().BeNullOrEmpty();
        userInputFormSchema.LanguageHeader.Should().BeNullOrEmpty();
        userInputFormSchema.UnitLabValuesHeader.Should().BeNullOrEmpty();
        userInputFormSchema.IsActiveHeader.Should().BeNullOrEmpty();
        userInputFormSchema.IsSeparateBillingHeader.Should().BeNullOrEmpty();
        userInputFormSchema.Billing.Should().BeNull();
        userInputFormSchema.ChangeClinicalSettingHeader.Should().BeNullOrEmpty();

        userInputFormSchema.Salutation.Should().Be("MockedCreateUser");
        userInputFormSchema.Title.Should().Be("MockedCreateUser");
        userInputFormSchema.Surname.Should().Be("MockedCreateUser");
        userInputFormSchema.FirstName.Should().Be("MockedCreateUser");
        userInputFormSchema.ProfessionalSpecialisation.Should().Be("MockedCreateUser");
        userInputFormSchema.PreferredLab.Should().Be("MockedCreateUser");
        userInputFormSchema.Address.Should().Be("MockedCreateUser");
        userInputFormSchema.City.Should().Be("MockedCreateUser");
        userInputFormSchema.ZipCode.Should().Be("MockedCreateUser");
        userInputFormSchema.CountryCode.Should().Be("MockedCreateUser");
        userInputFormSchema.Country.Should().Be("MockedCreateUser");
        userInputFormSchema.TelephoneNumber.Should().Be("MockedCreateUser");
        userInputFormSchema.EMailAddress.Should().Be("MockedCreateUser");
        userInputFormSchema.Language.Should().Be("MockedCreateUser");
        userInputFormSchema.UnitLabValues.Should().Be("MockedCreateUser");
        userInputFormSchema.IsActive.Should().BeTrue();
        userInputFormSchema.IsSeparateBilling.Should().BeTrue();
        userInputFormSchema.Billing.Should().BeNull();
    }

    [Test]
    public void BillingToBillingTemplate()
    {
        //Arrange
        var mockedValue = "mockedValue";
        var billing = GetBillingMock(mockedValue);

        var billingHeader = new BillingTemplate()
        {
            BillingAddressHeader = "mockedTemplateHeader"
        };

        //Act
        _mapper.Map(billing, billingHeader);

        //Assert
        billingHeader.Should().NotBeNull();
        billingHeader.BillingCity.Should().Be(mockedValue);
        billingHeader.BillingAddress.Should().Be(mockedValue);
        billingHeader.BillingCountry.Should().Be(mockedValue);
        billingHeader.BillingCountryCode.Should().Be(mockedValue);
        billingHeader.BillingName.Should().Be(mockedValue);
        billingHeader.BillingName.Should().Be(mockedValue);
        billingHeader.BillingPhone.Should().Be(mockedValue);
        billingHeader.BillingZip.Should().Be(mockedValue);
    }

    [Test]
    public void BillingToBillingModel()
    {
        //Arrange
        var mockedValue = "mockedValue";
        var billing = GetBillingMock(mockedValue);

        //Act
        var billingModel = _mapper.Map<BillingModel>(billing);

        //Assert
        var mappedModelProperties = billingModel.GetType().GetProperties().ToList();
        var billingProperties = billing.GetType().GetProperties().Where(x => !x.Name.Equals("Id") || !x.Name.Equals("UserModel")).ToList(); //Id and UserModel are not present in the source.

        foreach (var mappedProperty in mappedModelProperties)
        {
            if (mappedProperty.Name.Equals("Id"))
            {
                mappedProperty.GetValue(billingModel).Should().Be(new Guid());
                continue;
            }
            if (mappedProperty.Name.Equals("UserModel"))
            {
                mappedProperty.GetValue(billingModel).Should().BeNull();
                continue;
            }

            var valueToTest = billingProperties.FirstOrDefault(x => x.Name.Equals(mappedProperty.Name)).GetValue(billing);
            mappedProperty.GetValue(billingModel).Should().Be(valueToTest);
        }
    }

    [Test]
    public void UserModelToCreateUser()
    {
        //Arrange
        var userModel = MockDataProvider.GetMockedUserModel();
        var expectedBillingValue = "BillingMock";

        //Act
        var result = _mapper.Map<CreateUser>(userModel);

        //Assert
        result.Should().NotBeNull();

        result.ClinicalSetting.Should().Be(userModel.ClinicalSetting);
        result.ProfessionalSpecialisation.Should().Be(userModel.ProfessionalSpecialisation);
        result.Department.Should().Be(userModel.Department);
        result.Organization.Should().Be(userModel.Organization);
        result.PreferredLab.Should().Be(userModel.PreferredLab);
        result.Salutation.Should().Be(userModel.Salutation);
        result.Title.Should().Be(userModel.Title);
        result.Surname.Should().Be(userModel.Surname);
        result.FirstName.Should().Be(userModel.FirstName);
        result.Address.Should().Be(userModel.Address);
        result.City.Should().Be(userModel.City);
        result.ZipCode.Should().Be(userModel.ZipCode);
        result.CountryCode.Should().Be(userModel.CountryCode);
        result.Country.Should().Be(userModel.Country);
        result.TelephoneNumber.Should().Be(userModel.TelephoneNumber);
        result.EMailAddress.Should().Be(userModel.EMailAddress);
        result.Language.Should().Be(userModel.Language);
        result.UnitLabValues.Should().Be(userModel.UnitLabValues);
        result.IsActive.Should().Be(userModel.IsActive);
        result.IsSeparateBilling.Should().Be(userModel.IsSeparateBilling);
        result.Billing.Should().NotBeNull();
        result.Billing!.BillingName.Should().Be(expectedBillingValue);
        result.Billing.BillingAddress.Should().Be(expectedBillingValue);
        result.Billing.BillingZip.Should().Be(expectedBillingValue);
        result.Billing.BillingCity.Should().Be(expectedBillingValue);
        result.Billing.BillingCountry.Should().Be(expectedBillingValue);
        result.Billing.BillingCountryCode.Should().Be(expectedBillingValue);
        result.Billing.BillingPhone.Should().Be(expectedBillingValue);
    }

    [Test]
    public void BillingTemplateToBillingModel()
    {
        //Arrange
        var userModel = GetBillingTemplate();
        var expectedValue = "Mock";

        //Act
        var result = _mapper.Map<BillingModel>(userModel);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(new Guid());
        result.BillingName.Should().Be(expectedValue);
        result.BillingAddress.Should().Be(expectedValue);
        result.BillingZip.Should().Be(expectedValue);
        result.BillingCity.Should().Be(expectedValue);
        result.BillingCountry.Should().Be(expectedValue);
        result.BillingCountryCode.Should().Be(expectedValue);
        result.BillingPhone.Should().Be(expectedValue);
        result.UserModel.Should().BeNull();
    }

    [Test]
    public void BillingModelToBilling()
    {
        //Arrange
        var billingModel = GetBillingModel();

        //Act
        var result = _mapper.Map<Billing>(billingModel);

        //Assert
        result.BillingName.Should().Be(billingModel.BillingName);
        result.BillingAddress.Should().Be(billingModel.BillingAddress);
        result.BillingZip.Should().Be(billingModel.BillingZip);
        result.BillingCity.Should().Be(billingModel.BillingCity);
        result.BillingCountry.Should().Be(billingModel.BillingCountry);
        result.BillingCountryCode.Should().Be(billingModel.BillingCountryCode);
        result.BillingPhone.Should().Be(billingModel.BillingPhone);
    }

    private BillingModel GetBillingModel()
    {
        return new()
        {
            Id = Guid.NewGuid(),
            BillingName = "Mock",
            BillingAddress = "Mock",
            BillingZip = "Mock",
            BillingCity = "Mock",
            BillingCountry = "Mock",
            BillingCountryCode = "Mock",
            BillingPhone = "Mock",
            UserModel = new UserModel(),
        };
    }

    private BillingTemplate GetBillingTemplate()
    {
        return new()
        {
            BillingName = "Mock",
            BillingAddress = "Mock",
            BillingZip = "Mock",
            BillingCity = "Mock",
            BillingCountry = "Mock",
            BillingCountryCode = "Mock",
            BillingPhone = "Mock",
            BillingNameHeader = "Mock",
            BillingAddressHeader = "Mock",
            BillingZipHeader = "Mock",
            BillingCityHeader = "Mock",
            BillingCountryHeader = "Mock",
            BillingCountryCodeHeader = "Mock",
            BillingPhoneHeader = "Mock"
        };
    }

    private Billing GetBillingMock(string mockedValue) => new()
    {
        BillingCity = mockedValue,
        BillingCountryCode = mockedValue,
        BillingAddress = mockedValue,
        BillingCountry = mockedValue,
        BillingName = mockedValue,
        BillingPhone = mockedValue,
        BillingZip = mockedValue
    };

    private ScoringRequestModel GetScoringRequestModelMock()
    {
        return new()
        {
            Id = Guid.NewGuid(),
            UserId = "MockUserId",
            User = MockDataProvider.GetMockedUserModel(),
            PatientId = "MockPatientId",
            Biomarkers = GetBiomarkerList(),
            Responses = GetMockedResponses(),
            CreatedOn = DateTimeOffset.Now,
        };
    }

    private IEnumerable<Biomarkers> GetBiomarkerList()
    {
        return MockDataProvider.GetFakeBiomarkersList().FirstOrDefault(); ;
    }

    private IEnumerable<ScoringResponseModel> GetMockedResponses()
    {
        return new List<ScoringResponseModel>()
        {
            GetScoringResponseModelMock(),
        };
    }

    private ScoringResponseModel GetScoringResponseModelMock()
    {
        return new()
        {
            Id = Guid.NewGuid(),
            classifier_class = 1,
            classifier_score = 2.0,
            classifier_sign = 1,
            Score = "22",
            Risk = "MockRisk",
            RiskClass = 1,
            Recommendation = "Recommendation",
            RecommendationLong = "RecommendationLong",
            Warnings = new string[] { },
            Biomarkers = new Biomarkers(),
            BiomarkersId = Guid.NewGuid(),
            Request = new ScoringRequestModel(),
            RequestId = Guid.NewGuid(),
        };
    }

    private ICollection<BiomarkerOrderModel> GetBiomarkerCollection()
    {
        return new Collection<BiomarkerOrderModel>
        {
            new() { OrderNumber = 1, BiomarkerId = "first", PreferredUnit = "unit", User = null, UserId = "id" },
            new() { OrderNumber = 2, BiomarkerId = "second", PreferredUnit = "unit", User = null, UserId = "id" }
        };
    }

    private CreateCountry GetCreateCountry()
    {
        return new()
        {
            Name = "MockedName",
            ContactEmail = "MockedEmail"
        };
    }

    private CountryModel GetCountryModel()
    {
        return new()
        {
            Id = Guid.NewGuid(),
            ContactEmail = "MockContactEmail",
            Name = "mockedName"
        };
    }

    private CreateOrganization GetCreateOrganization()
    {
        return new CreateOrganization()
        {
            ContactEmail = "MockContactEmail",
            Name = "mockedName",
            TenantId = Guid.NewGuid().ToString()
        };
    }

    private CountryModel GetMockedCountryModel()
    {
        return new()
        {
            Id = Guid.NewGuid(),
            ContactEmail = "MockedContactEmail",
            Name = "MockedName"
        };
    }

    private OrganizationModel GetOrganizationModel()
    {
        return new()
        {
            Id = Guid.NewGuid(),
            TenantId = Guid.NewGuid().ToString(),
            ContactEmail = "MockContactEmail",
            Name = "mockedName"
        };
    }

    private RecommendationCategory GetRecommendationCategory()
    {
        return new()
        {
            Id = 0,
            ShortText = "MockShortText",
            LongText = "MockLongText",
            LowerLimit = "MockLowerLimit",
            UpperLimit = "MockUpperLimit",
            RiskValue = "MockRiskValue",
            Prevalence = ScoreSummaryUtility.PrevalenceClass.Secondary
        };
    }

    private RecommendationCategoryStaticPart GetRecommendationCategoryStaticPart()
    {
        return new()
        {
            Id = 1,
            LowerLimit = "MockedLowerLimit",
            UpperLimit = "MockedUpperLimit",
            RiskValue = "MockedRiskValue"
        };
    }

    private RecommendationCategoryLocalizedPart GetRecommendationCategoryLocalizedPart()
    {
        return new()
        {
            Id = 1,
            ShortText = "MockedShortText",
            LongText = "MockedLongText",
        };
    }

    private ScoringSchema GetScoringSchema()
    {
        return new()
        {
            ScoreHeader = "MockScoreHeader",
            Score = "MockScore",
            RiskHeader = "MockRiskHeader",
            Risk = "MockRisk",
            RecommendationHeader = "MockRecommendationHeader",
            Recommendation = "MockRecommendation",
            RecommendationExtended = "MockRecommendationExtended",
            RecommendationTableHeader = "MockRecommendationTableHeader",
            RecommendationProbabilityHeader = "MockRecommendationProbabilityHeader",
            RecommendationScoreRangeHeader = "MockRecommendationScoreRangeHeader",
            WarningHeader = "MockWarningHeader",
            Warnings = new[] { "MockWarning", "MockWarning2" },
            InfoText = "MockInfoText",
            Abbreviations = new Dictionary<string, string>()
            {
                { "mockKey1", "mockValue1" },
                { "mockKey2", "mockValue2" }
            },
            CadDefinitionHeader = "MockCadDefinitionHeader",
            CadDefinition = "MockCadDefinition",
            FootnoteHeader = "MockFootnoteHeader",
            Footnote = "MockFootnote",
            IntendedUseHeader = "MockIntendedUseHeader",
            IntendedUse = "MockIntendedUse",
        };
    }
}