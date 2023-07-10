﻿using AutoMapper;
using CE_API_V2.Controllers;
using CE_API_V2.Models.DTO;
using CE_API_V2.Models.Mapping;
using CE_API_V2.Services;
using CE_API_V2.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace CE_API_Test.UnitTests.Controllers;

[TestFixture]
public class BiomarkersControllerTests
{
    private BiomarkersController _biomarkersController;
    private IBiomarkersTemplateService _templateService;

    [OneTimeSetUp]
    public void Setup()
    {
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mapperConfig.CreateMapper();
        _templateService = new BiomarkersTemplateService(mapper);
        _biomarkersController = new BiomarkersController(_templateService);
    }

    [Test]
    public async Task TestSchemaEndpoint()
    {
        var getTemplateTask = () => _biomarkersController.GetInputFormTemplate("en-GB");
        var result = await getTemplateTask.Should().NotThrowAsync();
        result.Subject.Should().BeOfType<OkObjectResult>();
        var template = ((OkObjectResult) result.Subject).Value;
        template.Should().NotBeNull();
        template.Should().BeOfType<List<BiomarkerSchemaDto>>();
        var biomarkersTemplate = (IEnumerable<BiomarkerSchemaDto>) template;
        biomarkersTemplate.Count().Should().Be(33);
    }
}