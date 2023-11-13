using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.Models;
using CE_API_V2.Models.DTO;
using CE_API_V2.UnitOfWorks.Interfaces;
using CE_API_V2.Utility;
using CE_API_V2.Utility.CustomAnnotations;

namespace CE_API_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Administrator")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class AdministrativeEntitiesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IInputValidationService _inputValidationService;
        private readonly IAdministrativeEntitiesUOW _administrativeEntitiesUow;
        private readonly IUserInformationExtractor _userInformationExtractor;
        private readonly IUserUOW _userUOW;
        private readonly UserHelper _userHelper;

        /// <summary>
        /// All endpoints concerning Countries and Administrators
        /// </summary>
        /// <remarks>
        /// Controller handling administrative entities interactions.
        /// </remarks>
        /// <param name="mapper">The mapper.</param>
        /// <param name="inputValidationService">The input validation service.</param>
        /// <param name="administrativeEntitiesUow">The administrative entities unit of work.</param>
        /// <param name="userInformationExtractor">The user information extractor.</param>
        public AdministrativeEntitiesController(IMapper mapper,
            IInputValidationService inputValidationService,
            IAdministrativeEntitiesUOW administrativeEntitiesUow,
            IUserInformationExtractor userInformationExtractor,
            IUserUOW userUOW,
            UserHelper userHelper)
        {
            _mapper = mapper;
            _inputValidationService = inputValidationService;
            _administrativeEntitiesUow = administrativeEntitiesUow;
            _userInformationExtractor = userInformationExtractor;
            _userUOW = userUOW;
            _userHelper = userHelper;
        }

        /// <summary>Get countries</summary>
        /// <remarks>
        /// Returns a list of all countries present in the database.
        /// If there is no country, Status 404 is returned.
        /// If the user does not have the required rights, Status 403 is returned.
        /// </remarks>
        [HttpGet("countries", Name = "GetCountries")]
        [AllowAnonymous]
        [Produces("application/json", Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCountries()
        {
            var countryModelList = _administrativeEntitiesUow.GetCountries();

            var countries = _mapper.Map<IEnumerable<Country>>(countryModelList);
            return countries.Any() ? Ok(countries) : NotFound();
        }

        /// <summary>Get organizations</summary>
        /// <remarks>
        /// Returns a list of all organizations present in the database.
        /// If there is no country, Status 404 is returned.
        /// If the user does not have the required rights, Status 403 is returned.
        /// </remarks>
        [HttpGet("organizations", Name = "GetOrganizations")]
        [AllowAnonymous]
        [Produces("application/json", Type = typeof(IEnumerable<Organization>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOrganizations()
        {
            var organizationModelList = _administrativeEntitiesUow.GetOrganizations();
            var organizations = _mapper.Map<IEnumerable<Organization>>(organizationModelList);

            return organizations.Any() ? Ok(organizations) : NotFound();
        }

        /// <summary>Add country</summary>
        /// <remarks>
        /// Adds a country to the database.
        /// If the given country is not valid or the country is already present in the database, Status 400 is returned.
        /// If the user does not have the required rights, Status 401 is returned.
        /// </remarks>
        /// <returns>The added country.</returns>
        [HttpPost("country", Name = "CreateCountry")]
        [UserActive]
        [Produces("application/json", Type = typeof(CountryModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddCountry([FromBody] CreateCountry createCountry)
        {
            try
            {
                var convertedCountry = _mapper.Map<CountryModel>(createCountry);
                var storedCountryModel = _administrativeEntitiesUow.AddCountry(convertedCountry);
                var storedCountry = _mapper.Map<CreateCountry>(storedCountryModel);

                return storedCountry is not null ? Ok(storedCountry) : BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest("Organization already exists.");
            }
        }

        /// <summary>Add organization</summary>
        /// <remarks>
        /// Adds an organization to the database.
        /// If the given organization is not valid or the organization is already present in the database, Status 400 is returned.
        /// If the user does not have the required rights, Status 401 is returned.
        /// </remarks>
        /// <returns>The added organization.</returns>
        [HttpPost("organization", Name = "CreateOrganization")]
        [UserActive]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddOrganization([FromBody] CreateOrganization organization)
        {
            if (!_inputValidationService.ValidateOrganization(organization))
            {
                return BadRequest();
            }

            var convertedOrganization = _mapper.Map<OrganizationModel>(organization);
            try
            {
                var addedOrganizationModel = _administrativeEntitiesUow.AddOrganizations(convertedOrganization);
                var addedOrganization = _mapper.Map<Organization>(addedOrganizationModel);

                return addedOrganization is not null ? Ok(addedOrganization) : BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest("Organization already exists.");
            }
        }

        /// <summary>Update country</summary>
        /// <remarks>
        /// Adds a country to the database.
        /// If the given updated country information is not valid or the country is not present in the database, Status 400 is returned.
        /// If the user does not have the required rights, Status 401 is returned.
        /// </remarks>
        /// <returns>The updated country.</returns>
        [HttpPatch("country/{id}", Name = "UpdateCountry")]
        [UserActive]
        [Produces("application/json", Type = typeof(CountryModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateCountry(Guid id, CreateCountry updatedCreateCountry)
        {
            var updatedModel = _mapper.Map<CountryModel>(updatedCreateCountry);
            updatedModel.Id = id;
            var result = _administrativeEntitiesUow.UpdateCountry(updatedModel);
            var country = _mapper.Map<CreateCountry>(result);

            return country is not null ? Ok(country) : BadRequest();
        }

        /// <summary>Update organization</summary>
        /// <remarks>
        /// Adds an organization to the database.
        /// If the given updated organization information is not valid or the organization is not present in the database, Status 400 is returned.
        /// If the user does not have the required rights, Status 401 is returned.
        /// </remarks>
        /// <returns>The updated organization.</returns>
        [HttpPatch("organization/{id}", Name = "UpdateOrganization")]
        [UserActive]
        [Produces("application/json", Type = typeof(Organization))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult UpdateOrganization(Guid id, CreateOrganization updatedOrganization)
        {
            if (!_inputValidationService.ValidateOrganization(updatedOrganization))
            {
                return BadRequest();
            }
            var convertedOrganizationModel = _mapper.Map<OrganizationModel>(updatedOrganization);
            convertedOrganizationModel.Id = id;
            var updatedOrganizationModel = _administrativeEntitiesUow.UpdateOrganization(convertedOrganizationModel);
            var updatedResult = _mapper.Map<Organization>(updatedOrganizationModel);

            return updatedResult is not null ? Ok(updatedResult) : BadRequest();
        }

        /// <summary>Delete country by id</summary>
        /// <remarks>
        /// Deletes a country from the database identified by its id.
        /// If the given country is not present in the database, Status 400 is returned.
        /// If the user does not have the required rights, Status 401 is returned.
        /// </remarks>
        /// <returns>The updated organization.</returns>
        [HttpDelete("country", Name = "DeleteCountry")]
        [UserActive]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult DeleteCountryById(Guid id)
        {
            var deletedCountry = _administrativeEntitiesUow.DeleteCountry(id);

            return deletedCountry is not null ? Ok() : BadRequest();
        }

        /// <summary>
        /// Deletes a stored organization by its id.
        /// </summary>
        /// <remarks>
        /// Deletes an organization from the database identified by its id.
        /// If the given organization is not present in the database, Status 400 is returned.
        /// If the user does not have the required rights, Status 401 is returned.
        /// </remarks>
        /// <returns>The updated organization.</returns>
        [HttpDelete("organization", Name = "DeleteOrganization")]
        [UserActive]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult DeleteOrganizationById(Guid id)
        {
            var deletedOrganizationModel = _administrativeEntitiesUow.DeleteOrganization(id);

            return deletedOrganizationModel is not null ? Ok() : BadRequest();
        }
    }
}